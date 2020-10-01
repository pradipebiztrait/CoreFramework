using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using AspNetCore.ServiceRegistration.Dynamic.Extensions;
using AspNetCore.ServiceRegistration.Dynamic.Interfaces;
using AspNetCore.ServiceRegistration.Dynamic.Attributes;
using EBiz.CoreFramework.DataAccess.DbContext;
using EBiz.CoreFramework.Infrastructure.Utility;
using Microsoft.AspNetCore.Diagnostics;

namespace EBiz.CoreFramework.Web
{
	public class Startup
	{
		private readonly SiteSettings _siteSettings;

		public Startup(IConfiguration configuration, IHostingEnvironment env, IOptions<SiteSettings> siteSettings)
		{
			Configuration = configuration;
			HostingEnvironment = env;
			_siteSettings = siteSettings.Value;
		}

		public IConfiguration Configuration { get; }

		public IHostingEnvironment HostingEnvironment { get; private set; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
			services.Configure<SiteFolders>(Configuration.GetSection("SiteFolders"));
			services.Configure<SmsAuthantication>(Configuration.GetSection("SmsAuthantication"));
			services.Configure<SiteSettings>(Configuration.GetSection("SiteSettings"));
			services.Configure<AWSProperty>(Configuration.GetSection("AWSProperty"));
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => false;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			//User Identity DbContext
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
			})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidAudience = "https://localhost:44383/",
					ValidIssuer = "https://localhost:44383/",
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwertyuuoplkjhgfdsazxcvbnmqwertlkjfdslkjflksjfklsjfklsjdflskjflyuioplkjhgfdsazxcvbnmmnbv"))
				};
			});

			//External Login
			services.AddAuthentication().AddGoogle(options =>
			{
				options.ClientId = "772620662095-06vtd90npkud6jt3241m6teni5ssbf8a.apps.googleusercontent.com";
				options.ClientSecret = "ot2wGMWcBAWflu5RiP9BAlPP";
			});

			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromHours(1);
			});

			services.AddRouting();
			services.AddMemoryCache();

			//Auto Register Interface
			services.Scan(scan => scan.FromCallingAssembly().AddClasses().AsMatchingInterface());
			services.ConfigureApplicationCookie(options => options.LoginPath = "/Admin/Login");

			services.AddMvc();
			services.AddHttpContextAccessor();

			//AutoMapper
			services.AddAutoMapper(typeof(Startup));

			//Auto Register Service
			services.AddServicesOfType<IScopedService>();
			services.AddServicesWithAttributeOfType<ScopedServiceAttribute>();

            // Pascal casing
            //services.AddControllersWithViews().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //});
        }

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
		{

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseCookiePolicy();

			app.UseSession();

			//Deal with path base and proxies that change the request path
			app.Use(async (context, next) =>
			{
				var JWToken = context.Session.GetString("JWToken");
				if (!string.IsNullOrEmpty(JWToken))
				{
					context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
				}
				await next();
			});
			app.UseAuthentication();
			app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "admindefault",
					template: "{area:exists}/{controller=Login}/{action=Index}/{id?}"
					);

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

			});

            //CreateRoles(serviceProvider).Wait();
		}

		private async Task CreateRoles(IServiceProvider serviceProvider)
		{
			var _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
			var _user = await _context.users.Where(t => t.EmailAddress == "admin@gmail.com").FirstOrDefaultAsync();

			if (_user == null)
			{
				using (var transaction = _context.Database.BeginTransaction())
				{
					try
					{
						var user = new User
                        {
							UserId = 0,
							FirstName = "Admin",
							LastName = "User",
							Token = "",
							EmailAddress = "admin@gmail.com",
							Password = "fc0iUkg331qk3V8HY6MWvQ==",
							MobileNumber = "",
							ImagePath = "",
							CountryId = 0,
							StateId = 0,
							CityId = 0,
							PostalCode = "",
							Gender = 1,
							IsActive = 1,
							IsDelete = 0,
							RoleId = 1,
							CreatedOn = DateTime.Now,
							UpdatedOn = DateTime.Now,
							DeviceType = 1,
							DeviceToken = ""
						};

						await _context.users.AddAsync(user);
						await _context.SaveChangesAsync();
						transaction.Commit();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}
	}
}
