using Microsoft.ML.Data;

namespace AI4LS.APP.Models;

public class OnnxInputv2 : OnnxInput
{
    [ColumnName("LC0_Desc")]
    public float LC0_Desc { get; set; }
    public static OnnxInputv2 FromOnnxInput(OnnxInput input)
    {
        return new OnnxInputv2
        {
            pH_CaCl2 = input.pH_CaCl2,
            pH_H2O = input.pH_H2O,
            EC = input.EC,
            OC = input.OC,
            CaCO3 = input.CaCO3,
            P = input.P,
            N = input.N,
            K = input.K,
            LC0_Desc = 0
        };
    }
}
