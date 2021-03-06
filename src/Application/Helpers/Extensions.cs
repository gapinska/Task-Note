using System;
using Domain;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static void AddResourceParametersMetadataToResponse<T>(PagedList<T> collection, HttpResponse response)
        {
            var metadata = new
            {
                collection.TotalCount,
                collection.PageSize,
                collection.CurrentPage,
                collection.TotalPages,
                collection.HasNext,
                collection.HasPrevious
            };

            response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
        }
    }
}