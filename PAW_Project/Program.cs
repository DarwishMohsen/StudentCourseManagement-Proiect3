using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PAW_Project.Data;
using PAW_Project.Models;
using PAW_Project.Repositories;
using PAW_Project.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Seed Roles
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    if (!await roleManager.RoleExistsAsync("Student"))
        await roleManager.CreateAsync(new IdentityRole("Student"));

    if (!await roleManager.RoleExistsAsync("Professor"))
        await roleManager.CreateAsync(new IdentityRole("Professor"));

    // Seed Admin User
    var adminEmail = "Admin@darwish.com";
    var adminPassword = "Admin123!";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "admin-pic.png");
        if (System.IO.File.Exists(imagePath))
        {
            adminUser.ProfilePicture = await System.IO.File.ReadAllBytesAsync(imagePath);
        }

        var createResult = await userManager.CreateAsync(adminUser, adminPassword);

        if (!createResult.Succeeded)
        {
            throw new Exception(string.Join(", ", createResult.Errors.Select(e => e.Description)));
        }
    }

    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
        if (await userManager.IsInRoleAsync(adminUser, "Student"))
            await userManager.RemoveFromRoleAsync(adminUser, "Student");
    }

    // Seed Professor User (linked to P101 - Elvira Popescu)
    var profEmail = "prof@darwish.com";
    var profPassword = "Prof123!";
    var profUser = await userManager.FindByEmailAsync(profEmail);

    if (profUser == null)
    {
        profUser = new ApplicationUser { UserName = profEmail, Email = profEmail };
        var profResult = await userManager.CreateAsync(profUser, profPassword);

        if (!profResult.Succeeded)
        {
            throw new Exception(string.Join(", ", profResult.Errors.Select(e => e.Description)));
        }
    }

    if (!await userManager.IsInRoleAsync(profUser, "Professor"))
    {
        await userManager.AddToRoleAsync(profUser, "Professor");
    }

    var seededProf = await dbContext.Professors.FindAsync("P101");
    if (seededProf != null && string.IsNullOrEmpty(seededProf.UserId))
    {
        seededProf.UserId = profUser.Id;
        seededProf.Email = profEmail;
        await dbContext.SaveChangesAsync();
    }
}

app.Run();