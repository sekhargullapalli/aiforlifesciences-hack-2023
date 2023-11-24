using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;

using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        var lucasGraphicsOverlay = new GraphicsOverlay();
        // Add the overlay to a graphics overlay collection.
        GraphicsOverlayCollection overlays = new GraphicsOverlayCollection
            {
                lucasGraphicsOverlay
            };
        // Set the view model's "GraphicsOverlays" property (will be consumed by the map view).
        this.GraphicsOverlays = overlays;      

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
            var mpt = new MapPoint(pt.TH_LONG!.Value,pt.TH_LAT!.Value, SpatialReferences.Wgs84);            
            var pointGraphic = new Graphic(mpt, pointSymbol);
            lucasGraphicsOverlay.Graphics.Add(pointGraphic);
        }
    }
}

