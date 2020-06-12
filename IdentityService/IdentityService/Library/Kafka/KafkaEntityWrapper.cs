using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Library.Kafka
{
    public class KafkaEntityWrapper<T>
    {
        public String actionType { get; set; }

        public T data { get; set; }
    }
}
