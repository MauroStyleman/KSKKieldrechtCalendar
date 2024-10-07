using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace KSKKieldrecht1;

[ApiController]
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
        var matches = await _matchService.GetMatchesAsync(apiUrl);
        var icalendar = GenerateIcs(matches);

        return File(new System.Text.UTF8Encoding().GetBytes(icalendar), "text/calendar", "calendar.ics");
    }

    private string GenerateIcs(Match[] matches)
    {
        var calendar = "BEGIN:VCALENDAR\nPRODID:KSK. KIELDRECHT RESERVEN\nVERSION:2.0\nCALSCALE:GREGORIAN\n";

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
        var endCalendar = "END:VCALENDAR";
        calendar += endCalendar;

        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "KSK2calendar.ics");
        System.IO.File.WriteAllText(filePath, calendar);

        return filePath;
    }
    
    
}