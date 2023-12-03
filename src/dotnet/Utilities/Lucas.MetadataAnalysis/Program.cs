using AIforLS.LUCAS;
using Lucas.MetadataAnalysis;
using System.Globalization;
using System.Text.Json;

var ZuricModels = JsonSerializer.Deserialize<List<MetaModel>>(File.ReadAllText("ZurichMetaData.json"));
var VigoModels = JsonSerializer.Deserialize<List<MetaModel>>(File.ReadAllText("VigoMetaData.json"));
var CountryCodes = JsonSerializer.Deserialize<List<CountryCode>>(File.ReadAllText("CountrCodes.json"));

//string sequence = "Eukaryote";
string sequence = "Prokaryote";

var checkedItems = (sequence == "Eukaryote") ? VigoModels : ZuricModels;

var Bucket1Files = File.ReadAllLines("Eukaryotev2.txt").Select(x=>x.ToLower());
var Bucket2Files = File.ReadAllLines("Prokaryotev2.txt").Select(x => x.ToLower()); ;

int Bucket1Count = 0, Bucket2Count=0, MissingCount=0;
foreach (var item in checkedItems)
{
    item.Files=new List<string>();
    var matches1 = Bucket1Files.Where(x => x.StartsWith(item.Run.ToLower())).ToList();
    var matches2 = Bucket2Files.Where(x => x.StartsWith(item.Run.ToLower())).ToList();
    Bucket1Count += matches1.Count;
    Bucket2Count += matches2.Count;
    if (matches1.Count == 0 && matches2.Count == 0)
    {
        Console.WriteLine($"Missing: {item}");
        MissingCount++;
    }
    foreach (var match in matches1)
    {
        item.Files.Add(match);
    }
    foreach (var match in matches2)
    {
        item.Files.Add(match);
    }
}

Console.WriteLine($"Total Checked Items: {checkedItems.Count}");
Console.WriteLine($"Bucket1 Count: {Bucket1Count}");
Console.WriteLine($"Bucket2 Count: {Bucket2Count}");
Console.WriteLine($"Missing Count: {MissingCount}");

var lucasPoints = LUCASUtilities.GetLUCAS2018Data();

//foreach (var item in lucasPoints.Select(x => x.NUTS_0).Distinct().OrderBy(x => x).ToList())
//    Console.WriteLine(item);

Console.WriteLine();
int uniqueMatchCount = 0, duplicateMatchCount=0, noMataches =0;
foreach (var item in checkedItems)
{
    item.LUCAS2018Point = null;
    var matches = MapMetaModelToLucasPoint(item);
    if (matches.Count() == 0)
    {
        noMataches++;        
    }
    else if (matches.Count() == 1)
    {
        uniqueMatchCount++;
        item.LUCAS2018Point = matches.First();
    }
    else
    {
        duplicateMatchCount++;
    }   
}
Console.WriteLine($"Total Checked Items: {checkedItems.Count}");
Console.WriteLine($"Unique Match Count: {uniqueMatchCount}");
Console.WriteLine($"Duplicate Match Count: {duplicateMatchCount}");
Console.WriteLine($"No Match Count: {noMataches}");


var SequencesWithMatches = checkedItems.Where(x => x.LUCAS2018Point != null)
        .Select(x => x).ToList();

Console.WriteLine();
Console.WriteLine($"Average elevation error: {SequencesWithMatches.Select(x => Math.Abs(x.elev - x.LUCAS2018Point.Elev.Value)).Average()}");
Console.WriteLine($"Max elevation error: {SequencesWithMatches.Select(x => Math.Abs(x.elev - x.LUCAS2018Point.Elev.Value)).Max()}");
Console.WriteLine($"Min elevation error: {SequencesWithMatches.Select(x => Math.Abs(x.elev-x.LUCAS2018Point.Elev.Value)).Min()}");

Console.WriteLine();
Console.WriteLine("Creating extended Lucas points");

List<LUCAS2018PointExtended> extendedLucasPoints = new List<LUCAS2018PointExtended>();
foreach (var item in SequencesWithMatches)
{
    var extendedPoint = LUCAS2018PointExtended.FromLucas2018Point(item.LUCAS2018Point);
    extendedPoint.SequenceFiles = item.Files;
    extendedLucasPoints.Add(extendedPoint);
}
string filename = $"LucasPoints_{sequence}.json";
Console.WriteLine($"Writing to file: {filename}");
Console.WriteLine($"Total points: {extendedLucasPoints.Count}");
File.WriteAllText(filename, JsonSerializer.Serialize(extendedLucasPoints, new JsonSerializerOptions
{
    WriteIndented = true
}));
Console.WriteLine("Done!");


IEnumerable<LUCAS2018Point> MapMetaModelToLucasPoint(MetaModel model)
{
    string country = model.geo_loc_name_country;
    string code = CountryCodes.Where(x=>x.name.ToLower().Equals(country.ToLower())).Select(x=>x.code).First();
    var collectionDate = DateTime.ParseExact(model.Collection_Date,"yyyy-MM-dd",null);
    var latlanvec = model.lat_lon.Trim().Split(' ',StringSplitOptions.RemoveEmptyEntries);
    var lat = float.Parse(latlanvec[0], CultureInfo.InvariantCulture);
    var lon = float.Parse(latlanvec[2], CultureInfo.InvariantCulture);
    int elev = model.elev;
    string latlonFormat = "N4";
    return lucasPoints.Where(x =>
    {
        bool match = true;
        match &= x.NUTS_0.ToLower().Trim().Equals(code.ToLower().Trim());
        match &= (x.TH_LAT.Value.ToString(latlonFormat) == lat.ToString(latlonFormat));
        match &= (x.TH_LONG.Value.ToString(latlonFormat) == lon.ToString(latlonFormat));
        match &= (x.SURVEY_DATE.Year == collectionDate.Year);
        match &= (x.SURVEY_DATE.Month == collectionDate.Month);
        match &= (x.SURVEY_DATE.Day == collectionDate.Day);
        //match &= (x.Elev.Value == elev);

        var latDiff = Math.Abs(x.TH_LAT.Value - lat);
        var lonDiff = Math.Abs(x.TH_LONG.Value - lon);
        var latlonDiff = Math.Sqrt(latDiff * latDiff + lonDiff * lonDiff);
        var elevDiff = Math.Abs(x.Elev.Value - elev);

        match &= x.Depth.Replace(" ", "").ToLower().Contains(model.Depth.ToLower());

        //match &= (latlonDiff < 0.000001);
        //match &= (elevDiff <= 20);

        return match;
    }).ToList();
    
}

