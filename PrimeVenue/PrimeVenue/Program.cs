var builder = WebApplication.CreateBuilder(args);

app.MapGet("/", () => "Hello World!");
app.Run();
