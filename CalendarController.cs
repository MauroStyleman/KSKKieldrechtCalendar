using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace KSKKieldrecht1
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly MatchService _matchService;

        public CalendarController(MatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("icalendar")]
        public async Task<IActionResult> GetCalendar(string apiUrl)
        {
            // Fetch matches using the provided API URL
            var matches = await _matchService.GetMatchesAsync(apiUrl);
            var icalendar = GenerateIcs(matches);

            // Return the ICS file
            return File(new System.Text.UTF8Encoding().GetBytes(icalendar), "text/calendar", "calendar.ics");
        }

        public string GenerateIcs(Match[] matches)
        {
            // Initialize the calendar string
            var calendar = "BEGIN:VCALENDAR\nPRODID:KSK. KIELDRECHT RESERVEN\nVERSION:2.0\nCALSCALE:GREGORIAN\n";

            // Generate event details for each match
            foreach (var match in matches)
            {
                var eventDetails = $"BEGIN:VEVENT\n" +
                                   $"UID:{match.Id}\n" +
                                   $"DTSTAMP:{DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture)}\n" +
                                   $"DTSTART:{match.StartTime.ToString("yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture)}\n" +
                                   $"SUMMARY:{match.HomeTeam.Name} vs {match.AwayTeam.Name}\n" +
                                   $"DESCRIPTION:Outcome: {match.Outcome.HomeTeamGoals} - {match.Outcome.AwayTeamGoals}\n" +
                                   $"STATUS:CONFIRMED\n" +
                                   $"END:VEVENT\n";
                calendar += eventDetails;
            }

            // End the calendar
            calendar += "END:VCALENDAR\n";

            // Save the calendar file to the server
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "KSK2calendar.ics");
            System.IO.File.WriteAllText(filePath, calendar);

            return calendar; // Return the calendar string (optional)
        }
    }
}
