namespace KSKKieldrecht1;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

public class MatchService
{
    private readonly HttpClient _httpClient;

    public MatchService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Match[]> GetMatchesAsync(string url)
    {
        var response = await _httpClient.GetStringAsync(url);
        var data = JsonConvert.DeserializeObject<RootObject>(response);
        return data.Data.TeamCalendar;
    }

    private class RootObject
    {
        public Data Data { get; set; }
    }

    private class Data
    {
        public Match[] TeamCalendar { get; set; }
    }
}