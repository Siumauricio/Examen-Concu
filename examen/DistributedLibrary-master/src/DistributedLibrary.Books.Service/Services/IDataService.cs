using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedLibrary.Books.Service.Services
{
    public interface IDataService
    {
        IEnumerable<Books> GetEntities();

        Books GetEntityById(long entityId);
    }
}
