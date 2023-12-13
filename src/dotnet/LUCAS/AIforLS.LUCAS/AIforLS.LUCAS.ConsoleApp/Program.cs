using AIforLS.LUCAS;
using AIforLS.LUCAS.ConsoleApp;

using Azure.Identity;

using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

using VSG.OWMClient;
using VSG.OWMClient.Models;

var lucasPoints = LUCASUtilities.GetLUCAS2018Data();
var lucaspointsSwedish = lucasPoints!.Where(p => p.NUTS_0 == "SE");
// Console.WriteLine($"Total points {lucasPoints!.Count()}");
// Console.WriteLine($"Swedish points {lucaspointsSwedish.Count()}");

//var countries = lucasPoints!.Select(p => p.NUTS_0).Distinct();
//foreach (var country in countries)
//{
//    Console.WriteLine(country);
//}



//Usage
// string directoryPath = @"Eukaryote";
// DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
// var files = directorySelected.GetFiles("*.gz");
// int count = 0;
// foreach (FileInfo fileToDecompress in files)
// {
//     Console.WriteLine($"Decompressing {++count} of {files.Length}");
//     Decompress(fileToDecompress, "Decompressed");
// }




static void Decompress(FileInfo fileToDecompress, string targetFolder)
{
    using (FileStream originalFileStream = fileToDecompress.OpenRead())
    {
        string currentFileName = fileToDecompress.FullName;
        string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);
        Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(newFileName), targetFolder));        
        newFileName = Path.Combine(Path.GetDirectoryName(newFileName),targetFolder, Path.GetFileName(newFileName));

        using (FileStream decompressedFileStream = File.Create(newFileName))
        {
            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(decompressedFileStream);
                Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
            }
        }
    }
}

//usage
//string burl1 = @"https://ai-for-life-sciences-2.s3.amazonaws.com/";
//string source1 = "Prokaryotev2";
//await GetAllFilesFromS3(burl1, source1);

//string burl2 = @"https://ai-for-life-sciences-1.s3.amazonaws.com/";
//string source2 = "Eukaryotev2";
//await GetAllFilesFromS3(burl2, source2);

static async Task GetAllFilesFromS3(string url, string source)
{
    string[] allfiles = File.ReadAllLines($"{source}.txt");
    var files = allfiles.Where(f => f.Trim()!=string.Empty);
    Directory.CreateDirectory(source);
    using (var client = new HttpClient())
    {       
        int totalCount = files.Count();
        int count = 0;
        foreach (var item in files)
        {           
            var key = item.Replace('/', '-');
            var savefilename = Path.Combine(source, key);
            if (File.Exists(savefilename))
            {
                Console.WriteLine($"File Exists. {++count} of {totalCount}: {item}");
                continue;
            }
            Console.WriteLine($"Downloading {++count} of {totalCount}: {item}");
            var endpointURL = new Uri($"{url}{item}");
            await client.DownloadFileTaskAsync(endpointURL, savefilename);
        }
    }
}


//Usage: Reading Eukaryote data from S3 bucket. Do not use since, the list terminates at 1000 files

//await ReadFromXmlFile(burl, "s3bucket1.xml", "Eukaryote");
//Read from xml files from amazon bucket 
static async Task ReadFromXmlFile(string url, string filePath, string dirname)
{
    using (var client = new HttpClient())
    {
        S3BucketListModel result;
        XmlSerializer serializer =
            new XmlSerializer(typeof(S3BucketListModel), new XmlRootAttribute("ListBucketResult"));
        Directory.CreateDirectory(dirname);
        using (StreamReader reader = new StreamReader(filePath))
        {
            result = (S3BucketListModel)serializer.Deserialize(reader);
        }
        int totalCount = result.Contents.Count();
        int count = 0;
        foreach (var item in result.Contents)
        {
            var key = item.Key.Replace('/','-');
            var filename = Path.Combine(dirname, key);
            Console.WriteLine($"Downloading {++count} of {totalCount}: {item.Key}");
            var endpointURL = new Uri($"{url}{item.Key}");
            await client.DownloadFileTaskAsync(endpointURL, filename);
        }
    }
}

//Usage
ExtendDatasetWithWeatherData(lucaspointsSwedish, "Extended", "Lucas2018PointsWithWeatherDataSweden");
//Extend a dataset with weather data
static void ExtendDatasetWithWeatherData(IEnumerable<LUCAS2018Point> lucasPoints,string dirname, string filename)
{   
    List<Lucas2018PointWithWeatherData> lucasPointsWithWeatherData 
        = new List<Lucas2018PointWithWeatherData>();
    var weathertxt = File.ReadAllText("MergedWeatherDataSweden.json");
    var weatherdata = JsonSerializer.Deserialize<Dictionary<string, DailyAggregation>>(weathertxt);
    
    foreach(var pt in lucasPoints)
    {
        Lucas2018PointWithWeatherData pt2 = new Lucas2018PointWithWeatherData();
        foreach(var prop in typeof(LUCAS2018Point).GetProperties())
        {
            foreach(var prop2 in typeof(Lucas2018PointWithWeatherData).GetProperties())
            {
                if(prop.Name == prop2.Name)
                {
                    prop2.SetValue(pt2, prop.GetValue(pt));
                }
            }
        }
        if (!weatherdata.ContainsKey(pt.POINTID.ToString()))continue;
        DailyAggregation da = weatherdata![pt.POINTID.ToString()];
        if (da is not null)
        {
            pt2.D0_CloudCover_Afternoon = da.CloudCover!.Afternoon;
            pt2.D0_Humidity_Afternoon = da.Humidity!.Afternoon;
            pt2.D0_Pressure_Afternoon = da.Pressure!.Afternoon;
            pt2.D0_Percipitation_Total = da.Precipitation!.Total;
            pt2.D0_Temperature_Min = da.Temperature!.Min;
            pt2.D0_Temperature_Max = da.Temperature.Max;
            pt2.D0_Temperature_Afternoon = da.Temperature.Afternoon;
            pt2.D0_Temperature_Night = da.Temperature.Night;
            pt2.D0_Temperature_Evening = da.Temperature.Evening;
            pt2.D0_Temperature_Morning = da.Temperature.Morning;
            pt2.D0_MaxWind_Speed = da.Wind!.MaxWind!.speed;
            pt2.D0_MaxWind_Direction = da.Wind.MaxWind.direction; 
        }

        lucasPointsWithWeatherData.Add(pt2);
    }
    Directory.CreateDirectory(dirname);    
    var json = JsonSerializer.Serialize(lucasPointsWithWeatherData, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(Path.Join(dirname, $"{filename}.json"), json);
    
}

//Usage
//MergeJsonFiles("Day0Weather","MergedWeatherDataSweden");
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

//Usage
//await GetWeatherDataforLucasPoints(lucaspointsSwedish, 0);
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
