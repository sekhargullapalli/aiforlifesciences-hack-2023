using CoordinateSharp;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AIforLS.LUCAS.WPF.ViewModels;

public class BaseMapViewModel : INotifyPropertyChanged
{
    public BaseMapViewModel()
    {
        SetupMap();
        CreateGraphics();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private Map? _map;
    public Map? Map
    {
        get { return _map; }
        set
        {
            _map = value;
            OnPropertyChanged();
        }
    }
    private GraphicsOverlayCollection? _graphicsOverlays;
    public GraphicsOverlayCollection? GraphicsOverlays
    {
        get { return _graphicsOverlays; }
        set
        {
            _graphicsOverlays = value;
            OnPropertyChanged();
        }
    }
  
    private void SetupMap()
    {     
        Map = new Map(BasemapStyle.ArcGISTopographic);
    }
    private void CreateGraphics()
    {
        // Create a new graphics overlay to contain a variety of graphics.
        var malibuGraphicsOverlay = new GraphicsOverlay();
        // Add the overlay to a graphics overlay collection.
        GraphicsOverlayCollection overlays = new GraphicsOverlayCollection
            {
                malibuGraphicsOverlay
            };
        // Set the view model's "GraphicsOverlays" property (will be consumed by the map view).
        this.GraphicsOverlays = overlays;

        //// Create a point geometry.
        //var dumeBeachPoint = new MapPoint(-118.8066, 34.0006, SpatialReferences.Wgs84);
        //// Create a symbol to define how the point is displayed.
        //var pointSymbol = new SimpleMarkerSymbol
        //{
        //    Style = SimpleMarkerSymbolStyle.Circle,
        //    Color = System.Drawing.Color.Orange,
        //    Size = 10.0
        //};
        //// Add an outline to the symbol.
        //pointSymbol.Outline = new SimpleLineSymbol
        //{
        //    Style = SimpleLineSymbolStyle.Solid,
        //    Color = System.Drawing.Color.Blue,
        //    Width = 2.0
        //};
        //var pointGraphic = new Graphic(dumeBeachPoint, pointSymbol);
        //// Add the point graphic to graphics overlay.
        //malibuGraphicsOverlay.Graphics.Add(pointGraphic);

        var pointSymbol = new SimpleMarkerSymbol
        {
            Style = SimpleMarkerSymbolStyle.Circle,
            Color = System.Drawing.Color.Orange,
            Size = 10.0
        };
        pointSymbol.Outline = new SimpleLineSymbol
        {
            Style = SimpleLineSymbolStyle.Solid,
            Color = System.Drawing.Color.Blue,
            Width = 2.0
        };

        var LUCAS2018Data = LUCASUtilities.GetLUCAS2018Data();
        foreach(var pt in LUCAS2018Data!)
        {
            Coordinate coordinate = new Coordinate(pt!.TH_LAT!.Value, pt!.TH_LONG!.Value);
            coordinate.Set_Datum(Earth_Ellipsoid_Spec.WGS84_1984);

            //var dumeBeachPoint = new MapPoint(pt!.TH_LAT!.Value, pt!.TH_LONG!.Value, SpatialReferences.WebMercator);
            var dumeBeachPoint = new MapPoint(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), SpatialReferences.Wgs84);
            
            var pointGraphic = new Graphic(dumeBeachPoint, pointSymbol);
            malibuGraphicsOverlay.Graphics.Add(pointGraphic);

        }

    }
}

