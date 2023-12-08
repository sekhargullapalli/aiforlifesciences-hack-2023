using AI4LS.APP.Models;

using AIforLS.LUCAS;

using System.Text.Json;

namespace AI4LS.APP.Services
{
    public class CountriesService
    {
        public IEnumerable<CountryCode> GetCountries()
        {
            return JsonSerializer.Deserialize<List<CountryCode>>(File.ReadAllText("CountryCodes.json"))!;
        }
    }

 
}
