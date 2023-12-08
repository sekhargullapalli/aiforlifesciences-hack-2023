﻿using AI4LS.APP.Models;

using System.Text.Json;

namespace AI4LS.APP;

public static class LUCASUtilities
{
    public static IEnumerable<LUCAS2018Point>? GetLUCAS2018Data()
    {
        string? json = File.ReadAllText("LUCAS-SOIL-all-2018.json");
        return JsonSerializer.Deserialize<List<LUCAS2018Point>>(json);
    }
}
