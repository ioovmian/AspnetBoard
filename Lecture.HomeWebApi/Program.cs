var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Access-Control-Allow-Origin 에러를 위해 추가
builder.Services.AddCors((options) =>
{
    options.AddDefaultPolicy((policy) =>
    {
        policy.WithOrigins("https://localhost:7012");
        //policy.AllowAnyMethod();
        //policy.WithMethods( "get", "post" , "put", "delete");
        policy.WithMethods(new[] { "GET", "POST", "PUT", "DELETE" });
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Access-Control-Allow-Origin 에러를 위해 적용
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
