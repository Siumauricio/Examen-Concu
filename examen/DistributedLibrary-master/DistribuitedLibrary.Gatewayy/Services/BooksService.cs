using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DistribuitedLibrary.Gatewayy.Controllers {
    public class BooksService:IBooksService {


        public string CreateFile() {
            Guid id = Guid.NewGuid();
            FileStream fs = File.Create(id+".txt");
            fs.Close();
            return fs.Name;
        }
    }
}
