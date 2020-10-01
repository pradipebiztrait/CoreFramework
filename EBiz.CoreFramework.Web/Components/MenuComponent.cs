using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.Infrastructure.Utility;
using EBiz.CoreFramework.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Web.Components
{

    [ViewComponent(Name = "ManageMenuComponent")]
    public class ManageMenuComponent : ViewComponent
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly IMenuService _menuService;

        public ManageMenuComponent(IMemoryCache memoryCache, ApplicationDbContext context, IMenuService menuService)
        {
            _cache = memoryCache;
            _context = context;
            _menuService = menuService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Int64 id)
        {
            var model = new Menu();

            model.SubMenu = await _context.menu.Where(t => t.parent_menu_id == 0).ToListAsync();

            if (id > 0)
            {
                var page = await _menuService.GetAsync(id);

                model.menu_id = page.Data.menu_id;
                model.menu_title = page.Data.menu_title;
                model.menu_url = page.Data.menu_url;
                model.menu_icon = page.Data.menu_icon;
                model.parent_menu_id = page.Data.parent_menu_id;
                model.sort_order = page.Data.sort_order;
                model.is_active = page.Data.is_active;
                model.is_status = (page.Data.is_active == 1 ? true : false);
            }

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }

    [ViewComponent(Name = "MenuComponent")]
    public class MenuComponent : ViewComponent
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly IMenuService _menuService;

        public MenuComponent(IMemoryCache memoryCache, ApplicationDbContext context, IMenuService menuService)
        {
            _cache = memoryCache;
            _context = context;
            _menuService = menuService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new List<Menu>();

            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Menu, out model))
            {

                // Set cache options.
                //var cacheEntryOptions = new MemoryCacheEntryOptions()
                //    // Keep in cache for this time, reset time if accessed.
                //    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                var list = _menuService.GetUserMenuAsync(Convert.ToInt64(HttpContext.Session.GetString("_roleId")));
                var listMenu = (List<Menu>)list.Result.Data;
                model = listMenu.Where(t => t.parent_menu_id == 0).ToList();
                foreach (var item in model)
                {
                    item.SubMenu = listMenu.Where(q => q.parent_menu_id == item.menu_id).ToList();
                }

                // Save data in cache.
                _cache.Set(CacheKeys.Menu, model);
            }

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
