using System;
using Menu.Kitchen.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Menu.Kitchen.Attributes
{
    public class ModelStateTransferAttribute : ActionFilterAttribute
    {
        protected const string Key = nameof(ModelStateTransferAttribute);
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExportModelStateAttribute : ModelStateTransferAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (context.Result is RedirectResult || context.Result is RedirectToRouteResult
                    || context.Result is RedirectToActionResult)
                {
                    if (context.Controller is Controller controller && context.ModelState != null)
                    {
                        var modelState = ModelStateHelper.SerializeModelState(context.ModelState);

                        controller.TempData[Key] = modelState;
                    }
                }
            }

            base.OnActionExecuted(context);
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ImportModelStateAttribute : ModelStateTransferAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as Controller;

            if (controller?.TempData[Key] is string serializedModelState)
            {
                if (context.Result is ViewResult)
                {
                    var modelState = ModelStateHelper.DeserializeModelState(serializedModelState);

                    context.ModelState.Merge(modelState);
                }
                else
                {
                    controller.TempData.Remove(Key);
                }
            }

            base.OnActionExecuted(context);
        }
    }
}