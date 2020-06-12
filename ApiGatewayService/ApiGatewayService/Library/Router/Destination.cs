using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiGatewayService.Library.Router
{
    public class Destination
    {
        public string Path { get; set; }
        public bool RequiresAuthentication { get; set; }
        static HttpClient client = new HttpClient();

        public Destination(string uri, bool requiresAuthentication)
        {
            Path = uri;
            RequiresAuthentication = requiresAuthentication;
        }

        public Destination(string path)
            : this(path, false)
        {
        }

        private Destination()
        {
            Path = "/";
            RequiresAuthentication = false;
        }

        public HttpResponseMessage SendRequest(HttpRequest request)
        {
            string requestContent;
            using (Stream receiveStream = request.Body)
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    requestContent = readStream.ReadToEnd();
                }
            }

            using (var newRequest = new HttpRequestMessage(new HttpMethod(request.Method), CreateDestinationUri(request)))
            {
                SetHeaders(newRequest, request);
                newRequest.Content = new StringContent(requestContent, Encoding.UTF8, request.ContentType);
                using (var response = client.SendAsync(newRequest))
                {
                    return response.Result;
                }
            }
        }

        private string CreateDestinationUri(HttpRequest request)
        {
            string requestPath = request.Path.ToString().Replace("/gateway", "");
            string queryString = request.QueryString.ToString();

            string endpoint = "";
            string[] endpointSplit = requestPath.Substring(1).Split('/').Skip(1).ToArray();

            if (endpointSplit.Length >= 1)
                endpoint = "/" + String.Join("/", endpointSplit);


            return Path + endpoint + queryString;
        }

        private void SetHeaders(HttpRequestMessage newRequest, HttpRequest request)
        {
            newRequest.Headers.Add("User-Agent", request.Headers["User-Agent"].ToString());
            String UserID = request.Headers["UserID"].ToString();
            if (!String.IsNullOrEmpty(UserID)) {
                newRequest.Headers.Add("UserID", UserID);
            }
            String SessionID = request.Headers["SessionID"].ToString();
            if (!String.IsNullOrEmpty(SessionID))
            {
                newRequest.Headers.Add("SessionID", SessionID);
            }
        }

    }
}
