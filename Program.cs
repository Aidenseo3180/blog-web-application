using Blog.Web.Data;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Dependency injection, Inject DbContext to the application (for BlogDb)
builder.Services.AddDbContext<BlogDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BlogDbConnectionString")));

//For authDb
builder.Services.AddDbContext<AuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BlogAuthDbConnectionString")));

//also have to inform that we're using this auth Db
//Tell to use IdentityUser and Identity role as T user and T role, attach AuthDb with identities
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

//password requirements
builder.Services.Configure<IdentityOptions>(options =>
{
    //default settings for password
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

//Dependency Injection
//It means add injection inside Services when someone calls ITagRepository, Give them implementation(TagRepository) instead
builder.Services.AddScoped<ITagRepository, TagRepository>();
//When IBlogPostRepository called, give BlogPostRepository
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
//When IImageRepository called, give CloudinaryImageRepository
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();
builder.Services.AddScoped<IBlogPostCommentRepository, BlogPostCommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline, order matters
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Authenticate before useAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
