using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedLibrary.OtherAuthors.Api.Services {
    public class Authors {
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Ip_Address { get; set; }
    }
}
