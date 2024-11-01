
using GamifiedToDo.API;
using GamifiedToDo.API.Exceptions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.RegisterOptions(configuration);
services.RegisterComponents();
services.RegisterExternalServices(configuration);
services.RegisterSecurityServices(configuration);

services.AddFluentValidation();
services.AddSwagger();

services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gamified to do v1"));
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});

app.Run();

public partial class Program {}