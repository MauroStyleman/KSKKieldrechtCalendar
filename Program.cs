using KSKKieldrecht1;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Register the MVC controllers
builder.Services.AddHttpClient<MatchService>(); // Register the MatchService with HttpClient

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Enable HTTPS redirection
app.UseAuthorization(); // Enable authorization middleware

app.MapControllers(); // Map controller routes

// Automatically generate the ICS file on application startup
await GenerateIcsFileAsync(app.Services.GetRequiredService<MatchService>());

app.Run();

// Method to generate the ICS file at startup
async Task GenerateIcsFileAsync(MatchService matchService)
{
    // Example API URL - replace with your actual API URL
    string apiUrl = "https://datalake-prod2018.rbfa.be/graphql?operationName=GetTeamCalendar&variables=%7B%22teamId%22%3A%22308818%22%2C%22language%22%3A%22nl%22%2C%22sortByDate%22%3A%22asc%22%7D&extensions=%7B%22persistedQuery%22%3A%7B%22version%22%3A1%2C%22sha256Hash%22%3A%2263e80713dbe3f057aafb53348ebb61a2c52d3d6cda437d8b7e7bd78191990487%22%7D%7D";

    // Fetch matches and generate ICS
    Match[] matches = await matchService.GetMatchesAsync(apiUrl);
    var calendarController = new CalendarController(matchService);
    calendarController.GenerateIcs(matches); // Call to generate ICS
}