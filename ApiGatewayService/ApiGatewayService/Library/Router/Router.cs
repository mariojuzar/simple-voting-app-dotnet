using ApiGatewayService.Library.Utils;
using ApiGatewayService.Models.Request;
using ApiGatewayService.Models.Responses;
using IdentityService.Library.Logger.Interfaces;
using IdentityService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiGatewayService.Library.Router
{
    public class Router
    {

        public List<Route> Routes { get; set; }
        public Destination AuthenticationService { get; set; }

        public ILoggerManager logger;


        public Router(string routeConfigFilePath, ILoggerManager logger)
        {
            dynamic router = JsonLoader.LoadFromFile<dynamic>(routeConfigFilePath);

            Routes = JsonLoader.Deserialize<List<Route>>(Convert.ToString(router.routes));
            AuthenticationService = JsonLoader.Deserialize<Destination>(Convert.ToString(router.authenticationService));
            this.logger = logger;
        }

        public HttpResponseMessage RouteRequest(HttpRequest request)
        {
            string path = request.Path.ToString();
            string basePath = '/' + path.Split('/')[2];

            Destination destination;
            try
            {
                destination = Routes.First(r => r.Endpoint.Equals(basePath)).Destination;
            }
            catch
            {
                return ConstructErrorMessage(HttpStatusCode.NotFound, "The path could not be found.", request);
            }

            if (destination.RequiresAuthentication)
            {
                HttpResponseMessage authResponse =  GetResponse(request, basePath);
                String response = authResponse.Content.ReadAsStringAsync().Result;
                BaseResponse<PrivilegeResponse> privilegeResponse = JsonConvert.DeserializeObject<BaseResponse<PrivilegeResponse>>(response);

                if (!privilegeResponse.data.IsAllowed)
                {
                    return ConstructErrorMessage(HttpStatusCode.Unauthorized, "UnAuthorized", request);
                }
                request.Headers.Add("UserID", privilegeResponse.data.UserID.ToString());
            }

            return destination.SendRequest(request);
        }

        private HttpResponseMessage GetResponse(HttpRequest request, String basePath)
        {
            string id = request.Headers["SessionID"];
            Guid SessionID;
            try
            {
                SessionID = Guid.Parse(id);
            }
            catch
            {
                SessionID = new Guid();
            }

            var initialBody = request.Body;
            var initialPath = request.Path;
            var initialMethod = request.Method;

            PrivilegeRequest privilegeRequest = new PrivilegeRequest() { 
                SessionID = SessionID, 
                UrlPath = GetPath(request),
                Method = request.Method,
                Prefix = basePath.Replace("/", "")
            };

            String privBody = JsonConvert.SerializeObject(privilegeRequest);
            byte[] byteArray = Encoding.UTF8.GetBytes(privBody);
            MemoryStream newPrivBody = new MemoryStream(byteArray);

            request.ContentType = "application/json";
            request.Body = newPrivBody;
            request.Path = new PathString("/");
            request.Method = "POST";

            HttpResponseMessage authResponse = AuthenticationService.SendRequest(request);
            request.Body = initialBody;
            request.Path = initialPath;
            request.Method = initialMethod;

            return authResponse;
        }

        private string GetPath(HttpRequest request)
        {
            string requestPath = request.Path.ToString().Replace("/gateway", "");

            string endpoint = "";
            string[] endpointSplit = requestPath.Substring(1).Split('/').Skip(1).ToArray();

            if (endpointSplit.Length >= 1)
                endpoint = "/" + String.Join("/", endpointSplit);


            return endpoint;
        }

        private HttpResponseMessage ConstructErrorMessage(HttpStatusCode code, String error, HttpRequest request)
        {
            BaseResponse<String> content = BaseResponse<String>
                .ConstructResponse(code, error, null);
            HttpResponseMessage errorMessage = new HttpResponseMessage
            {
                StatusCode = code,
                Content = new StringContent(content.ToString())
            };
            request.HttpContext.Response.StatusCode = (int)code;
            return errorMessage;
        }

    }
}
