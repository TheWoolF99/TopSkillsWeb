using Core.Account;
using Data;
using Data.Services;
using Data.WebUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog.Web;
using System.Globalization;
using TopSkillsWeb.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseCustomSerilog();

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
    opts.Password.RequiredLength = 4;   // минимальная длина
    opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
    opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
    opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
    opts.Password.RequireDigit = false; // требуются ли цифры
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

#endregion Servises

var app = builder.Build();

//app.UseGlobalExceptionHandling();

app.UseCustomRequestLogging();

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
    if (context.Response.StatusCode == 404)
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