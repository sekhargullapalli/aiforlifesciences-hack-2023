using System.Text.Json;

namespace AIforLS.LUCAS;

public static class LUCASUtilities
{    
    public static IEnumerable<LUCAS2018Point>? GetLUCAS2018Data()
    {        
        string? json = File.ReadAllText("LUCAS-SOIL-2018.json");
        return JsonSerializer.Deserialize<List<LUCAS2018Point>>(json);
    }
}

