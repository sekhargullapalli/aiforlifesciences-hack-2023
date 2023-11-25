namespace AIforLS.LUCAS;

public class Lucas2018PointWithWeatherData:LUCAS2018Point
{
    public double? D0_CloudCover_Afternoon { get; set; }
    public double? D0_Humidity_Afternoon { get; set; }
    public double? D0_Pressure_Afternoon { get; set; }
    public double? D0_Percipitation_Total { get; set; }

    public double? D0_Temperature_Min { get; set; }
    public double? D0_Temperature_Max { get; set; }
    public double? D0_Temperature_Afternoon { get; set; }
    public double? D0_Temperature_Night { get; set; }
    public double? D0_Temperature_Evening { get; set; }
    public double? D0_Temperature_Morning { get; set; }
    public double? D0_MaxWind_Speed { get; set; }
    public double? D0_MaxWind_Direction { get; set; }


}
