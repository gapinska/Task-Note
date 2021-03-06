using Domain;
using Domain.ResourceParameters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class MetadataHelper<T> : IMetadataHelper<T>
    {
        public object CreateMetaData(PagedList<T> list, string previousPageLink, string nextPageLink)
        {
            return new
            {
                list.TotalCount,
                list.PageSize,
                list.CurrentPage,
                list.TotalPages,
                previousPageLink,
                nextPageLink
            };
        }
    }
}
