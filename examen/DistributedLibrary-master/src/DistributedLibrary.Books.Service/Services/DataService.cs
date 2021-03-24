using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DistributedLibrary.Books.Service.Services
{
    public class DataService : IDataService
    {
        private const string FileName = @"books.json";

   

        public IEnumerable<Books> GetEntities()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Books>>(File.ReadAllText(FileName));
        }

        public Books GetEntityById(long entityId)
        {
            List<Books> Registers = GetEntities().ToList() ;
            Books book;
            try {
                book = Registers.Single(s => s.Isbn == entityId);
                if (book != null) {
                    return book;
                }
            } catch (Exception) {

            }
                
            return null;
        }
    }
}
