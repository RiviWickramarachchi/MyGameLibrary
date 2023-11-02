using GamesLibrary.Configs;
using GamesLibrary.Models;
using GamesLibrary.Repositories;
using GamesLibrary.Security;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var settings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
//Making GUID and DateTime humanly readable
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

// Add services to the container.
builder.Services.AddControllersWithViews();
#if DEBUG
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
#endif
builder.Services.AddScoped<IIGDBRepository, IGDBRepository>();
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    return new MongoClient(settings.ConnectionString);
});
builder.Services.AddSingleton<IUsersRepository, MongoUsersRepository>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddMongoDbStores<ApplicationUser,ApplicationRole, Guid>
(
    settings.ConnectionString, settings.Name
);

//Password Hasher Interface and class
builder.Services.AddScoped<IPasswordHasher,PasswordHasher>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
