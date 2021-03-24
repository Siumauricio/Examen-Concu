using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistribuitedLibrary.Gatewayy.Controllers {
    public class Books {
        public long Isbn { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Author { get; set; }
        public int AuthorId { get; set; }
        public string Published { get; set; }
        public int Pages { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
    }
}
