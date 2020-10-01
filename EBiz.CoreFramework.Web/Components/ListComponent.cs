using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Web.Components
{
    [ViewComponent(Name= "ListComponent")]
    public class ListComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult((IViewComponentResult)View("Default"));
        }
    }
}
