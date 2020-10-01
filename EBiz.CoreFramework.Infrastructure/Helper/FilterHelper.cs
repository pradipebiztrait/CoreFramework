using EBiz.CoreFramework.DataAccess.Models;
using EBiz.CoreFramework.DataAccess.Models.AuxiliaryModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Infrastructure.Helper
{
    public class FilterHelper
    {
        public FilterRequest Set(FilterQuery queryString)
        {
            return new FilterRequest()
            {
                SearchType = 0,
                Skip = queryString.offset,
                Take = queryString.limit,
                SortDir = queryString.order ?? "desc",
                SortCol = queryString.sort ?? "CreatedOn",
                SearchCol = string.Empty,
                SearchText = queryString.search ?? ""
            };
        }

        public FilterRequest UserFilter(FilterQuery queryString)
        {
            var searchQuery = "";
            if (!string.IsNullOrEmpty(queryString.filter))
            {
                var userFilter = JsonConvert.DeserializeObject<UserFilter>(queryString.filter);
                searchQuery += "(";
                var searchCount = 0;
                if (!string.IsNullOrEmpty(userFilter.FirstName))
                {
                    searchQuery += " FirstName LIKE ('%" + userFilter.FirstName + "%') ";
                    searchCount++;
                }
                if (!string.IsNullOrEmpty(userFilter.LastName))
                {
                    searchQuery += (searchCount == 0) ? "" : "AND";
                    searchQuery += " LastName LIKE ('%" + userFilter.LastName + "%') ";
                    searchCount++;
                }
                if (!string.IsNullOrEmpty(userFilter.EmailAddress))
                {
                    searchQuery += (searchCount == 0) ? "" : "AND";
                    searchQuery += " EmailAddress LIKE ('%" + userFilter.EmailAddress + "%') ";
                    searchCount++;
                }
                if (!string.IsNullOrEmpty(userFilter.MobileNumber))
                {
                    searchQuery += (searchCount == 0) ? "" : "AND";
                    searchQuery += " MobileNumber LIKE ('%" + userFilter.MobileNumber + "%') ";
                    searchCount++;
                }
                if (userFilter.DateOfBirth != null)
                {
                    searchQuery += (searchCount == 0) ? "" : "AND";
                    searchQuery += " (DATE(DateOfBirth) = '"+ userFilter.DateOfBirth.Value.ToString("yyyy-MM-dd") + "') ";
                    searchCount++;
                }
                if (userFilter.IsActive != null)
                {
                    searchQuery += (searchCount == 0) ? "" : " AND";
                    searchQuery += " IsActive = " + userFilter.IsActive + " ";
                    searchCount++;
                }
                searchQuery += ")";
                
            }
            else
            {
                searchQuery += "(";
                searchQuery += " FirstName LIKE ('%" + queryString.search + "%') ";
                searchQuery += "OR LastName LIKE ('%" + queryString.search + "%') ";
                searchQuery += "OR EmailAddress LIKE ('%" + queryString.search + "%') ";
                searchQuery += "OR MobileNumber LIKE ('%" + queryString.search + "%') ";
                searchQuery += ")";
            }

            return new FilterRequest()
            {
                SearchType = 0,
                Skip = queryString.offset,
                Take = queryString.limit,
                SortDir = queryString.order ?? "desc",
                SortCol = queryString.sort ?? "CreatedOn",
                SearchCol = string.Empty,
                SearchText = searchQuery ?? ""
            };
        }

        public FilterRequest PageFilter(FilterQuery queryString)
        {
            var searchQuery = "";
            if (!string.IsNullOrEmpty(queryString.filter))
            {
                var userFilter = JsonConvert.DeserializeObject<PageFilter>(queryString.filter);
                searchQuery += "(";
                var searchCount = 0;
                if (!string.IsNullOrEmpty(userFilter.PageTitle))
                {
                    searchQuery += " PageTitle LIKE ('%" + userFilter.PageTitle + "%') ";
                    searchCount++;
                }
                if (!string.IsNullOrEmpty(userFilter.PageUrl))
                {
                    searchQuery += (searchCount == 0) ? "" : "AND";
                    searchQuery += " PageUrl LIKE ('%" + userFilter.PageUrl + "%') ";
                    searchCount++;
                }
                if (userFilter.IsActive != null)
                {
                    searchQuery += (searchCount == 0) ? "" : " AND";
                    searchQuery += " IsActive = " + userFilter.IsActive + " ";
                    searchCount++;
                }
                searchQuery += ")";

            }
            else
            {
                searchQuery += "(";
                searchQuery += " PageTitle LIKE ('%" + queryString.search + "%') ";
                searchQuery += "OR PageUrl LIKE ('%" + queryString.search + "%') ";
                searchQuery += ")";
            }

            return new FilterRequest()
            {
                SearchType = 0,
                Skip = queryString.offset,
                Take = queryString.limit,
                SortDir = queryString.order ?? "desc",
                SortCol = queryString.sort ?? "CreatedOn",
                SearchCol = string.Empty,
                SearchText = searchQuery ?? ""
            };
        }

        public FilterRequest MenuFilter(FilterQuery queryString)
        {
            var searchQuery = "";
            if (!string.IsNullOrEmpty(queryString.filter))
            {
                var _filter = JsonConvert.DeserializeObject<MenuFilter>(queryString.filter);
                searchQuery += "(";
                var searchCount = 0;
                if (!string.IsNullOrEmpty(_filter.menu_title))
                {
                    searchQuery += " menu_title LIKE ('%" + _filter.menu_title + "%') ";
                    searchCount++;
                }
                if (!string.IsNullOrEmpty(_filter.menu_url))
                {
                    searchQuery += (searchCount == 0) ? "" : "AND";
                    searchQuery += " menu_url LIKE ('%" + _filter.menu_url + "%') ";
                    searchCount++;
                }
                searchQuery += ")";

            }
            else
            {
                searchQuery += "(";
                searchQuery += " menu_title LIKE ('%" + queryString.search + "%') ";
                searchQuery += "OR menu_url LIKE ('%" + queryString.search + "%') ";
                searchQuery += ")";
            }

            return new FilterRequest()
            {
                SearchType = 0,
                Skip = queryString.offset,
                Take = queryString.limit,
                SortDir = queryString.order ?? "desc",
                SortCol = queryString.sort ?? "created_on",
                SearchCol = string.Empty,
                SearchText = searchQuery ?? ""
            };
        }

        public FilterRequest RoleFilter(FilterQuery queryString)
        {
            var searchQuery = "";
            if (!string.IsNullOrEmpty(queryString.filter))
            {
                var _filter = JsonConvert.DeserializeObject<RoleFilter>(queryString.filter);
                searchQuery += "(";
                var searchCount = 0;
                if (!string.IsNullOrEmpty(_filter.role_name))
                {
                    searchQuery += " role_name LIKE ('%" + _filter.role_name + "%') ";
                    searchCount++;
                }
                searchQuery += ")";

            }
            else
            {
                searchQuery += " role_name LIKE ('%" + queryString.search + "%') ";
            }

            return new FilterRequest()
            {
                SearchType = 0,
                Skip = queryString.offset,
                Take = queryString.limit,
                SortDir = queryString.order ?? "desc",
                SortCol = queryString.sort ?? "created_on",
                SearchCol = string.Empty,
                SearchText = searchQuery ?? ""
            };
        }
    }
}
