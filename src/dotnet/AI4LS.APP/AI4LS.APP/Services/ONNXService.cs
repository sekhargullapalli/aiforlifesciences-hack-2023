using AI4LS.APP.Models;

using Microsoft.ML;
using Microsoft.ML.Data;

namespace AI4LS.APP.Services;

public class ONNXService
{
    string ONNX_MODEL_PATH_LCO = "LC0_Desc_ONNX.onnx";
    string ONNX_MODEL_PATH_LC1 = "LC1_Desc_ONNX.onnx";
    string ONNX_MODEL_PATH_LU1 = "LU1_Desc_ONNX.onnx";

    string ONNX_MODEL_PATH_LCO_COMPRESSED = "LC0_Desc_ONNX.zip";
    string ONNX_MODEL_PATH_LC1_COMPRESSED = "LC1_Desc_ONNX.zip";
    string ONNX_MODEL_PATH_LU1_COMPRESSED = "LU1_Desc_ONNX.zip";

    MLContext mlContext = new MLContext();

    ITransformer onnxPredictionPipeline_LCO;
    ITransformer onnxPredictionPipeline_LC1;
    ITransformer onnxPredictionPipeline_LU1;

    PredictionEngine<OnnxInput, OnnxOutput> onnxPredictionEngine_LCO;
    PredictionEngine<OnnxInputv2, OnnxOutput> onnxPredictionEngine_LC1;
    PredictionEngine<OnnxInputv2, OnnxOutput> onnxPredictionEngine_LU1;


    public ONNXService()
    {
        if (!File.Exists($"./{ONNX_MODEL_PATH_LCO}"))
            System.IO.Compression.ZipFile.ExtractToDirectory(ONNX_MODEL_PATH_LCO_COMPRESSED, ".");
        if (!File.Exists($"./{ONNX_MODEL_PATH_LC1}"))
            System.IO.Compression.ZipFile.ExtractToDirectory(ONNX_MODEL_PATH_LC1_COMPRESSED, ".");
        if (!File.Exists($"./{ONNX_MODEL_PATH_LU1}"))
            System.IO.Compression.ZipFile.ExtractToDirectory(ONNX_MODEL_PATH_LU1_COMPRESSED, ".");

        onnxPredictionPipeline_LCO = GetPredictionPipeline<OnnxInput>(mlContext, ONNX_MODEL_PATH_LCO, inputColumns);
        onnxPredictionPipeline_LC1 = GetPredictionPipeline<OnnxInputv2>(mlContext, ONNX_MODEL_PATH_LC1, inputColumnsv2);
        onnxPredictionPipeline_LU1 = GetPredictionPipeline<OnnxInputv2>(mlContext, ONNX_MODEL_PATH_LU1, inputColumnsv2);

        onnxPredictionEngine_LCO = mlContext.Model.CreatePredictionEngine<OnnxInput, OnnxOutput>(onnxPredictionPipeline_LCO);
        onnxPredictionEngine_LC1 = mlContext.Model.CreatePredictionEngine<OnnxInputv2, OnnxOutput>(onnxPredictionPipeline_LC1);
        onnxPredictionEngine_LU1 = mlContext.Model.CreatePredictionEngine<OnnxInputv2, OnnxOutput>(onnxPredictionPipeline_LU1);
    }

    public OnnxOutput PredictLCO(OnnxInput input)
    {
        return onnxPredictionEngine_LCO.Predict(input);
    }
    public OnnxOutput PredictLC1(OnnxInputv2 input)
    {
        return onnxPredictionEngine_LC1.Predict(input);
    }
    public OnnxOutput PredictLU1(OnnxInputv2 input)
    {
        return onnxPredictionEngine_LU1.Predict(input);
    }


    ITransformer GetPredictionPipeline<T>(MLContext mlContext, string OnnxModelPath, string[] inputColumns) where T : class, new()
    {
        var outputColumns = new string[] { "probabilities", "label" };
        var onnxPredictionPipeline =
            mlContext
                .Transforms
                .ApplyOnnxModel(
                    outputColumnNames: outputColumns,
                    inputColumnNames: inputColumns,
                    OnnxModelPath);
        var emptyDv = mlContext.Data.LoadFromEnumerable(new T[] { });
        return onnxPredictionPipeline.Fit(emptyDv);

    }


    string[] inputColumns = new string[]
        {
            "pH_CaCl2", "pH_H2O", "EC", "OC", "CaCO3", "P", "N", "K"
        };
    string[] inputColumnsv2 = new string[]
        {
            "pH_CaCl2", "pH_H2O", "EC", "OC", "CaCO3", "P", "N", "K","LC0_Desc"
        };

    public Dictionary<int, string> LCMappings { get; } = new Dictionary<int, string>()
        {
            {0,"Artificial land"},
            {1,"Bareland"},
            {2,"Cropland"},
            {3,"Grassland"},
            {4,"Shrubland"},
            {5,"Water"},
            {6,"Wetlands"},
            {7,"Woodland"}
        };

    public Dictionary<int, string> LC1Mappings { get; } = new Dictionary<int, string>()
          {
        { 0, "Apple fruit" },
        { 1, "Arable land (only PI)" },
        { 2, "Barley" },
        { 3, "Broadleaved woodland" },
        { 4, "Cherry fruit" },
        { 5, "Clovers" },
        { 6, "Common wheat" },
        { 7, "Cotton" },
        { 8, "Dry pulses" },
        { 9, "Durum wheat" },
        { 10, "Floriculture and ornamental plants" },
        { 11, "Grassland with sparse tree/shrub cover" },
        { 12, "Grassland without tree/shrub cover" },
        { 13, "Inland fresh running water" },
        { 14, "Inland marshes" },
        { 15, "Inland salty water bodies" },
        { 16, "Lichens and Moss" },
        { 17, "Lucerne" },
        { 18, "Maize" },
        { 19, "Mix of cereals" },
        { 20, "Non built-up area features" },
        { 21, "Non built-up linear features" },
        { 22, "Nurseries" },
        { 23, "Nuts trees" },
        { 24, "Oats" },
        { 25, "Olive groves" },
        { 26, "Oranges" },
        { 27, "Other Leguminous  and mixtures for fodder" },
        { 28, "Other artificial areas" },
        { 29, "Other bare soil" },
        { 30, "Other cereals" },
        { 31, "Other citrus fruit" },
        { 32, "Other coniferous woodland" },
        { 33, "Other fibre and oleaginous crops" },
        { 34, "Other fresh vegetables" },
        { 35, "Other fruit trees and berries" },
        { 36, "Other mixed woodland" },
        { 37, "Other non-permanent industrial crops" },
        { 38, "Other root crops" },
        { 39, "Pear fruit" },
        { 40, "Peatbogs" },
        { 41, "Permanent industrial crops" },
        { 42, "Pine dominated coniferous woodland" },
        { 43, "Pine dominated mixed woodland" },
        { 44, "Potatoes" },
        { 45, "Rape and turnip rape" },
        { 46, "Rice" },
        { 47, "Rocks and stones" },
        { 48, "Rye" },
        { 49, "Salines" },
        { 50, "Sand" },
        { 51, "Shrubland with sparse tree cover" },
        { 52, "Shrubland without tree cover" },
        { 53, "Soya" },
        { 54, "Spontaneously re-vegetated surfaces" },
        { 55, "Spruce dominated coniferous woodland" },
        { 56, "Spruce dominated mixed woodland" },
        { 57, "Strawberries" },
        { 58, "Sugar beet" },
        { 59, "Sunflower" },
        { 60, "Temporary grassland" },
        { 61, "Tobacco" },
        { 62, "Tomatoes" },
        { 63, "Triticale" },
        { 64, "Vineyards" },
        };




    public Dictionary<int, string> LUMappings { get; } = new Dictionary<int, string>()
        {
        { 0, "Abandoned industrial areas"},
        { 1, "Abandoned residential areas"},
        { 2, "Abandoned transport areas"},
        { 3, "Agriculture (excluding fallow land and kitchen gardens)"},
        { 4, "Amenities, museum, leisure (e.g. parks, botanical gardens)"},
        { 5, "Commerce"},
        { 6, "Community services"},
        { 7, "Construction"},
        { 8, "Electricity, gas and thermal power distribution"},
        { 9, "Energy production"},
        { 10, "Fallow land"},
        { 11, "Financial, professional and information services"},
        { 12, "Forestry"},
        { 13, "Kitchen gardens"},
        { 14, "Logistics and storage"},
        { 15, "Mining and quarrying"},
        { 16, "Other abandoned areas"},
        { 17, "Other primary production"},
        { 18, "Protection infrastructures"},
        { 19, "Railway transport"},
        { 20, "Residential"},
        { 21, "Road transport"},
        { 22, "Semi-natural and natural areas not in use"},
        { 23, "Sport"},
        { 24, "Water supply and treatment"},
        { 25, "Water transport"}
        };
}





