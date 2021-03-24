using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DistributedLibrary.Authors.Api.Services
{
    public class DataService : IDataService
    {
        private const string FileName = @"authors.json";
        public IEnumerable<Authors> GetEntities()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Authors>>(File.ReadAllText(FileName));
        }

        public Authors GetAuthorById(int Id)
        {
            List<Authors> Registers = GetEntities().ToList();
            Authors authors;
            try {
                 authors = Registers.Single(s => s.Id == Id);
                if (authors != null) {
                    return authors;
                }
            } catch (Exception) {

            }
            
            return null;
        }

       
    }
}
