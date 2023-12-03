using AIforLS.LUCAS;

namespace Lucas.MetadataAnalysis;

public class MetaModel
{
    public string Run { get; set; }
    public string BioSample { get; set; }
    public string Collection_Date { get; set; }
    public string Depth { get; set; }
    public int elev { get; set; }
    public string Experiment { get; set; }
    public string geo_loc_name_country { get; set; }
    public string geo_loc_name { get; set; }
    public string lat_lon { get; set; }
    public string LibraryName { get; set; }
    public string LibraryLayout { get; set; }
    public int version { get; set; }
    public string SampleName { get; set; }
    public string SRAStudy { get; set; }


    public List<string> Files { get; set; }= new List<string>();
    public LUCAS2018Point LUCAS2018Point { get; set; } = null;
}
