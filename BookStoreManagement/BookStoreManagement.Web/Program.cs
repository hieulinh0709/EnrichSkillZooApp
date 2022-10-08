using BookStoreManagement.DataAccess.Data;
using BookStoreManagement.DataAccess.DbInitializer;
using BookStoreManagement.DataAccess.Repository;
using BookStoreManagement.DataAccess.Repository.IRepository;
using BookStoreManagement.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// connect đến sql service
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// stripe api
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

// Indentity service dùng để quản lý user
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// lifecycle dependency injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // tạo 1 thể hiện mới mỗi khi nhận 1 request
builder.Services.AddScoped<IDbInitializer, DbInitializer>(); 
builder.Services.AddSingleton<IEmailSender, EmailSender>(); // service email được tạo 1 lần duy nhất

//builder.Services.AddRazorPages();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// authentication với facebook
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "533257571075733";
    options.AppSecret = "cee28519a0b06e73c034c996885febbb";
});

// sau khi kiểm tra role nếu không hợp lệ thì báo access denied
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddDistributedMemoryCache(); // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)

builder.Services.AddSession(options => //Đăng ký dịch vụ Session
{
    options.IdleTimeout = TimeSpan.FromMinutes(100); // Thời gian tồn tại của Session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Phần mềm trung gian chuyển hướng HTTPS (UseHttpsRedirection) để chuyển hướng tất cả các yêu cầu HTTP đến HTTPS.
app.UseHttpsRedirection();

// handle file
app.UseStaticFiles();
/*
 * Bất kỳ phần mềm trung gian nào xuất hiện trước lệnh gọi UseRouting () sẽ không biết điểm cuối nào sẽ chạy cuối cùng.
 * Thêm EndpointRoutingMiddleware: ánh xạ Request gọi đến Endpoint (Middleware cuối)phù hợp định nghĩa bởi EndpointMiddleware
 */
app.UseRouting();

// handle payment
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

// call function to migration data
SeedDatabase();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

// default route
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

// Migration data
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
