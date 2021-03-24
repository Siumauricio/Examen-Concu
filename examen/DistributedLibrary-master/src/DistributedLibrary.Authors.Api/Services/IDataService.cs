using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedLibrary.Authors.Api.Services
{
    public interface IDataService
    {
        IEnumerable<Authors> GetEntities();

        Authors  GetAuthorById(int id);
    }
}
