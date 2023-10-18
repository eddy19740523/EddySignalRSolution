using SignalRRedisApp.Hubs;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Redis ���Ṯ�ڿ�
var connectionString = builder.Configuration.GetConnectionString("RedisNoSQLConnection");

//AddSignalR �߰� for RedisBackplain
//builder.Services.AddSignalR().AddStackExchangeRedis(connectionString);

builder.Services.AddSignalR().AddStackExchangeRedis(connectionString, options => {
      options.Configuration.ChannelPrefix = "SignalRRedisApp";
  });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:7019/")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});


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

app.UseAuthorization();

// UseCors must be called before MapHub.
app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//���Ŭ���� �߰� 
app.MapHub<ChatHub>("/chatHub");

app.Run();