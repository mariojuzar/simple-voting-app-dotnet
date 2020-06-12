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
        public String status { get; set; }

        public String message { get; set; }

        public T data { get; set; }

        public String errors { get; set; }

        public DateTime serverTime { get; set; }

        public static BaseResponse<T> ConstructResponse(HttpStatusCode Status, String Message, T Data)
        {
            BaseResponse<T> baseResponse = new BaseResponse<T>();
            baseResponse.status = ((int)Status).ToString();
            baseResponse.message = Message;
            baseResponse.data = Data;
            baseResponse.errors = null;
            baseResponse.serverTime = DateTime.Now;
            return baseResponse;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
