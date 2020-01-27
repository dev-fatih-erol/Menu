using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Menu.Kitchen.Helpers
{
    public class ModelStateTransfer
    {
        public string Key { get; set; }

        public string Attempted { get; set; }

        public object Raw { get; set; }

        public ICollection<string> ErrorMessages { get; set; } = new List<string>();
    }

    public static class ModelStateHelper
    {
        public static string SerializeModelState(ModelStateDictionary modelState)
        {
            var errorList = modelState
                    .Select(kvp => new ModelStateTransfer
                    {
                        Key = kvp.Key,
                        Attempted = kvp.Value.AttemptedValue,
                        Raw = kvp.Value.RawValue,
                        ErrorMessages = kvp.Value.Errors.Select(err => err.ErrorMessage).ToList(),
                    });

            return JsonConvert.SerializeObject(errorList);
        }

        public static ModelStateDictionary DeserializeModelState(string serializedErrorList)
        {
            var errorList = JsonConvert.DeserializeObject<List<ModelStateTransfer>>(serializedErrorList);

            var modelState = new ModelStateDictionary();

            foreach (var item in errorList)
            {
                modelState.SetModelValue(item.Key, item.Raw, item.Attempted);

                foreach (var error in item.ErrorMessages)
                {
                    modelState.AddModelError(item.Key, error);
                }
            }

            return modelState;
        }
    }
}