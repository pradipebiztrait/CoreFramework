using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Web.Components
{
    [ViewComponent(Name= "DeleteModalComponent")]
    public class DeleteModalComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult((IViewComponentResult)View("Default"));
        }
    }

    [ViewComponent(Name = "DeleteMultipleModalComponent")]
    public class DeleteMultipleModalComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult((IViewComponentResult)View("Default"));
        }
    }
}
