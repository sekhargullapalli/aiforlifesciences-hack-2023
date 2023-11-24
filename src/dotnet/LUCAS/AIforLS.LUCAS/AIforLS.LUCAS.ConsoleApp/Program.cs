using AIforLS.LUCAS;

using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Text;
using System.Text.Json;

using VSG.OWMClient;
using VSG.OWMClient.Models;

var lucasPoints = LUCASUtilities.GetLUCAS2018Data();
var lucaspointsSwedish = lucasPoints!.Where(p => p.NUTS_0 == "SE");
Console.WriteLine(lucasPoints!.Count());



MergeJsonFiles("Day0Weather","MergedWeatherDataSweden");

//Get all json files in the directory and merge them to one json file and save to disk
static void MergeJsonFiles(string dirpath, string filename)
{
    var files = Directory.GetFiles(dirpath, "*.json");
    Dictionary<string,DailyAggregation> weatherData = new Dictionary<string, DailyAggregation>();
    foreach (var file in files)
    {
        var json = File.ReadAllText(file);
        var data = JsonSerializer.Deserialize<DailyAggregation>(json);
        weatherData.Add(Path.GetFileNameWithoutExtension(file), data!);
    }
    File.WriteAllText(Path.Join(dirpath, $"{filename}.json"), JsonSerializer.Serialize(weatherData, new JsonSerializerOptions { WriteIndented = true }));
    
}



//Gets weather data for all LUCAS points with a day offset to the survey date
static async Task GetWeatherDataforLucasPoints(IEnumerable<LUCAS2018Point> lucaspoints, int dayOffset=0)
{
    var services = new ServiceCollection();    
    services.AddLogging(configure =>
    {
        //configure.AddConsole(options =>
        //{
        //});
    });    
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddUserSecrets<Program>()
        .Build();
    services.AddAzureClients(clientBuilder =>
    {
        var kvName = configuration.GetValue<string>("KeyVaultName");
        clientBuilder.AddSecretClient(new Uri($"https://{kvName}.vault.azure.net/"));
        //clientBuilder.UseCredential(new DefaultAzureCredential());
        clientBuilder.UseCredential(new VisualStudioCredential());

    });
    services.AddSingleton<IConfiguration>(configuration);
    services.AddHttpClient();    
    services.AddSingleton<IOWMClientV3, OWMClientV3>();
    var serviceProvider = services.BuildServiceProvider();    
    var client = serviceProvider.GetService<IOWMClientV3>();

    // Gettting weather data for LUCAS points
    int count = 0;
    List<int> FailedPoints = new List<int>();
    foreach (var pt in lucaspoints)
    {
        try
        {
            var dirpath = $"Day{dayOffset}Weather";
            Directory.CreateDirectory(dirpath);
            var filepth = Path.Join(dirpath, $"{pt.POINTID}.json");
            var lat = pt.TH_LAT!.Value;
            var lon = pt.TH_LONG!.Value;
            var date = DateOnly.FromDateTime(pt.SURVEY_DATE).AddDays(dayOffset);
            Console.WriteLine($"Point {++count} of {lucaspoints.Count()}: lat: {lat:0.####)} lon: {lon:0.####)} date: {date}");

            DailyAggregation.DailyAggregationQuery query = new();
            query.Latitude = 55.90059m;
            query.Longitude = 12.80606m;
            query.Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2));
            query.Units = WeatherUnits.Metric;

            var response = await client!.GetDailyAggregationAsync(query);
            File.WriteAllText(filepth, JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception)
        {

            FailedPoints.Add(pt.POINTID);
        }
    }
    Console.WriteLine();
    Console.WriteLine($"Failed Points: {FailedPoints.Count()}");
    foreach (var pt in FailedPoints)
    {
        Console.WriteLine(pt);
    }
}


