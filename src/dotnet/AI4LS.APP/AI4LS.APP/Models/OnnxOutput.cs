using Microsoft.ML.Data;

namespace AI4LS.APP.Models;


public class OnnxOutput
{
    [ColumnName("probabilities")]
    public VBuffer<float> probabilities { get; set; }
    [ColumnName("label")]
    public VBuffer<Int64> label { get; set; }
}
