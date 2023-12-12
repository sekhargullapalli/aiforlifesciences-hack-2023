using Microsoft.ML.Data;

namespace AI4LS.APP.Models;

public class OnnxInput
{
    [ColumnName("pH_CaCl2")]
    public float pH_CaCl2 { get; set; }

    [ColumnName("pH_H2O")]
    public float pH_H2O { get; set; }

    [ColumnName("EC")]
    public float EC { get; set; }

    [ColumnName("OC")]
    public float OC { get; set; }

    [ColumnName("CaCO3")]
    public float CaCO3 { get; set; }

    [ColumnName("P")]
    public float P { get; set; }

    [ColumnName("N")]
    public float N { get; set; }

    [ColumnName("K")]
    public float K { get; set; }
}