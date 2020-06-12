using System;

namespace IdentityService.Models.Requests
{
    public class KafkaUserRequest
    {
        public Guid userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public Guid genderId { get; set; }
        public String gender { get; set; }
        public int age { get; set; }
        public DateTime createdDate { get; set; }
        public string createdBy { get; set; }
        public DateTime updatedDate { get; set; }
        public string updatedBy { get; set; }
    }
}
