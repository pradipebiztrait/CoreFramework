using EBiz.CoreFramework.DataAccess;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBiz.CoreFramework.Repository.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDapperService _dapperService;
        private readonly ApplicationDbContext _context;

        public CompanyRepository(IDapperService dapperService, ApplicationDbContext context)
        {
            _dapperService = dapperService;
            _context = context;

        }

        public async Task<ApiResponse> GetAsync()
        {
            var _apiRes = new ApiResponse();

            _apiRes.Data = await _context.company.FirstOrDefaultAsync();

            _apiRes.Message = "Success";
            _apiRes.Status = true;

            return _apiRes;
        }

        public async Task<ApiResponse> SaveAsync(Company company)
        {
            var _apiRes = new ApiResponse();

            var model = await _context.company.FirstOrDefaultAsync();

            if (model != null)
            {
                model.company_name = company.company_name;
                model.email_address = company.email_address;
                model.contact_number = company.contact_number;
                model.whatsapp_number = company.whatsapp_number;
                model.address = company.address;
                model.country = company.country;
                model.state = company.state;
                model.city = company.city;
                model.pincode = company.pincode;
                model.website_url = company.website_url;
                model.facebook_url = company.facebook_url;
                model.google_url = company.google_url;
                model.twitter_url = company.twitter_url;
                model.instagram_url = company.instagram_url;
                model.youtube_url = company.youtube_url;
                model.company_logo = company.company_logo;

                _context.company.Update(model);
                _context.SaveChanges();
            }
            else
            {
                await _context.company.AddAsync(company);
                await _context.SaveChangesAsync();
            }

            _apiRes.Message = "Company detail has been saved.";
            _apiRes.Status = true;

            return _apiRes;
        }

        public void UpdateLogoPath(Int64 id, string path)
        {
            var _apiRes = new ApiResponse();

            var model = _context.company.FirstOrDefault();

            if (model != null)
            {
                model.company_logo = path;

                _context.company.Update(model);
                _context.SaveChanges();
            }
        }
    }
}
