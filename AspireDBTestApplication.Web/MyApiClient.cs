using AspireDBTestApplication.PeopleDB;

namespace AspireDBTestApplication.Web;

public class MyApiClient(HttpClient httpClient)
{
    public async Task<WeatherForecast[]> GetWeatherAsync()
    {
        return await httpClient.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast") ?? [];
    }

    public async Task<Person[]> GetPeopleAsync()
    {
        return await httpClient.GetFromJsonAsync<Person[]>("/getpeople") ?? [];
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
