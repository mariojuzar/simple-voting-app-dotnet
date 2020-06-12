using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IdentityService.Models
{
    public class BaseResponse<T>
    {
        public String Status { get; set; }

        public String Message { get; set; }

        public T Data { get; set; }

        public String Errors { get; set; }

        public DateTime ServerTime { get; set; }

        public static BaseResponse<T> ConstructResponse(HttpStatusCode Status, String Message, T Data)
        {
            BaseResponse<T> baseResponse = new BaseResponse<T>();
            baseResponse.Status = ((int)Status).ToString();
            baseResponse.Message = Message;
            baseResponse.Data = Data;
            baseResponse.Errors = null;
            baseResponse.ServerTime = DateTime.Now;
            return baseResponse;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
