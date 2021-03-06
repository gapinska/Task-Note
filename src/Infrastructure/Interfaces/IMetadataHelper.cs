using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Interfaces
{
    public interface IMetadataHelper<T>
    {
        public object CreateMetaData(PagedList<T> list, string previousPageLink, string nextPageLink);
    }
}
