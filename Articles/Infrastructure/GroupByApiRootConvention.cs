using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Articles.Infrastructure
{
    public class GroupByApiRootConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.Attributes.OfType<RouteAttribute>().FirstOrDefault(); 
            var apiVersion = controllerNamespace.Template.Split('/').First().ToLowerInvariant() ?? "Default";

            controller.ApiExplorer.GroupName= apiVersion;
        }

    }
}
