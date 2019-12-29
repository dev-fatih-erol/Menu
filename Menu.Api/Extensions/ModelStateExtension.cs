using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Menu.Api.Extensions
{
    public static class ModelStateExtension
    {
        public static Dictionary<string, string[]> GetErrors(this ModelStateDictionary source)
        {
            return source.Where(m => m.Value.Errors.Any())
                                      .ToDictionary(m => m.Key,
                                                    m => m.Value.Errors
                                                          .Select(e => e.ErrorMessage)
                                                          .ToArray());
        }
    }
}