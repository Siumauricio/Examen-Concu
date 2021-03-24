using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedLibrary.OtherAuthors.Api.Services
{
    public interface IDataService
    {
        IEnumerable<Authors> GetEntities();

        Authors  GetAuthorById(int id);
    }
}
