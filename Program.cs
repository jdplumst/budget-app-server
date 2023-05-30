var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
//     {
//         builder.AllowAnyOrigin()
//                .AllowAnyMethod()
//                .AllowAnyHeader();
//     }));
builder.Services.AddCors();

DotNetEnv.Env.Load();

var app = builder.Build();// Add Cors

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors((o) => o.WithOrigins("http://localhost:3000", "https://budget-app-coral.vercel.app"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
