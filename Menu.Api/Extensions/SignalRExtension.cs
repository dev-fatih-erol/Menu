using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Menu.Api.Extensions
{
    public static class SignalRExtension
    {
        public static T GetQueryParameterValue<T>(this IQueryCollection httpQuery, string queryParameterName) =>
         httpQuery.TryGetValue(queryParameterName, out var value) && value.Any()
           ? (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T))
           : default;
    }
}