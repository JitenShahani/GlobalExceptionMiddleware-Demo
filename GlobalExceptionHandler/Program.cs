var builder = WebApplication.CreateBuilder (args);

// Add services to the container.
builder.ConfigureIoCContainer ();

var app = builder.Build ();

// Configure the HTTP request pipeline.
app.ConfigurePipeline ();

app.UseHttpsRedirection ();

new MyEndpoints ().MapMyEndpoints (app);

app.Run ();