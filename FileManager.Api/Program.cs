using FileManager.Api.Data;
using FileManager.Api.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IFileService, FileService>();    
builder.Services.AddFluentValidation().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connection));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
var staticfilePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot" ,"Uploads");
if (!Directory.Exists(staticfilePath))
{
    Directory.CreateDirectory(staticfilePath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(staticfilePath),
    RequestPath = "/Uploads"
});


app.Run();
