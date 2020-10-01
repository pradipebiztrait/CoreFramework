using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Web.Components
{
    [ViewComponent(Name= "AlertComponent")]
    public class AlertComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult((IViewComponentResult)View("Default"));
        }
    }

    [ViewComponent(Name = "DeactiveUserComponent")]
    public class DeactiveUserComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult((IViewComponentResult)View("Default"));
        }
    }

	[ViewComponent(Name = "NotificationEmailComponent")]
	public class NotificationEmailComponent : ViewComponent
	{
		public Task<IViewComponentResult> InvokeAsync()
		{
			return Task.FromResult((IViewComponentResult)View("Default"));
		}
	}
}
