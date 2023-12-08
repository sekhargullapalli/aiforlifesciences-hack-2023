using System.Text.Json.Serialization;

namespace AIforLS.LUCAS;

public class LUCAS2018Point
{
    [JsonPropertyName("POINTID")]
    public int POINTID { get; set; }
    [JsonPropertyName("Depth")]
    public string? Depth { get; set; }
    [JsonPropertyName("pH_CaCl2")]
    public float? pH_CaCl2 { get; set; }
    [JsonPropertyName("pH_H2O")]
    public float? pH_H2O { get; set; }
    [JsonPropertyName("EC")]
    public float? EC { get; set; }
    [JsonPropertyName("OC")]
    public float? OC { get; set; }
    [JsonPropertyName("CaCO3")]
    public float? CaCO3 { get; set; }
    [JsonPropertyName("P")]
    public float? P { get; set; }
    [JsonPropertyName("N")]
    public float? N { get; set; }
    [JsonPropertyName("K")]
    public float? K { get; set; }
    [JsonPropertyName("OC_2030_cm")]
    public float? OC_2030_cm { get; set; }
    [JsonPropertyName("CaCO3_2030_cm")]
    public float? CaCO3_2030_cm { get; set; }
    [JsonPropertyName("Ox_Al")]
    public float? Ox_Al { get; set; }
    [JsonPropertyName("Ox_Fe")]
    public float? Ox_Fe { get; set; }
    [JsonPropertyName("NUTS_0")]
    public string? NUTS_0 { get; set; }
    [JsonPropertyName("NUTS_1")]
    public string? NUTS_1 { get; set; }
    [JsonPropertyName("NUTS_2")]
    public string? NUTS_2 { get; set; }
    [JsonPropertyName("NUTS_3")]
    public string? NUTS_3 { get; set; }
    [JsonPropertyName("TH_LAT")]
    public float? TH_LAT { get; set; }
    [JsonPropertyName("TH_LONG")]
    public float? TH_LONG { get; set; }
    [JsonPropertyName("SURVEY_DATE")]
    public DateTime SURVEY_DATE { get; set; }
    [JsonPropertyName("Elev")]
    public int? Elev { get; set; }
    [JsonPropertyName("LC")]
    public string? LC { get; set; }
    [JsonPropertyName("LU")]
    public string? LU { get; set; }
    [JsonPropertyName("LC0_Desc")]
    public string? LC0_Desc { get; set; }
    [JsonPropertyName("LC1_Desc")]
    public string? LC1_Desc { get; set; }
    [JsonPropertyName("LU1_Desc")]
    public string? LU1_Desc { get; set; }
}
