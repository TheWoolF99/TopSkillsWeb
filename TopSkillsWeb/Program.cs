using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TopSkillsWeb.Models;
using Core;
using Data.Repository;
using Data;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Core.Account;
using Data.Services;
using Newtonsoft;
using Newtonsoft.Json;
using Data.WebUser;
using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
using NLog.Web;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson(o => { o.SerializerSettings.MaxDepth = Int32.MaxValue; o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
string ConnString = "production";

builder.Services.AddHttpContextAccessor();
//builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1440);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddEfRepositories(builder.Configuration.GetConnectionString(ConnString));
builder.Services.AddIdentity<User, UserRole>(opts =>
{
    opts.Password.RequiredLength = 4;   // ����������� �����
    opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
    opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
    opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
    opts.Password.RequireDigit = false; // ��������� �� �����
}).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();


builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.MaxDepth = Int32.MaxValue;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(2000);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.SlidingExpiration = true;
});
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var SuppotredCultures = new[]
    {
        new CultureInfo("ru"),
        new CultureInfo("en")
    };

    options.RequestCultureProviders = new[] { new CookieRequestCultureProvider() };
    options.DefaultRequestCulture = new RequestCulture("ru");
    options.SupportedCultures = SuppotredCultures;
    options.SupportedUICultures = SuppotredCultures;
});
builder.WebHost.UseNLog();



#region Servises
builder.Services.AddScoped<PhotoService>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<AttendanceService>();
builder.Services.AddScoped<LoggerService>();
builder.Services.AddScoped<AbonementService>();
builder.Services.AddScoped<GlobalOptionsService>();
builder.Services.AddScoped<WebUserService>();
builder.Services.AddScoped<AccessesService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value);

app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

app.Map("/error", ap => ap.Run(async context =>
{
    if(context.Response.StatusCode == 404)
    {
        context.Response.Redirect("/Home/NotFound");
    }
}));

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
