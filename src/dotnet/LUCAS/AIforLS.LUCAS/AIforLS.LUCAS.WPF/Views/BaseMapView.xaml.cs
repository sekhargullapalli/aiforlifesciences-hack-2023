using CoordinateSharp;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;

using System.Windows.Controls;

namespace AIforLS.LUCAS.WPF.Views
{    
    public partial class BaseMapView : UserControl
    {
        public BaseMapView()
        {
            InitializeComponent();            
            UpdateViewPoint();
        }
        public void UpdateViewPoint()
        {
            var LUCAS2018Data = LUCASUtilities.GetLUCAS2018Data();
            var lat = LUCAS2018Data!.Select(x => x.TH_LAT).Average();
            var lon = LUCAS2018Data!.Select(x => x.TH_LONG).Average();            
            MapPoint mapCenterPoint = new MapPoint(lon!.Value, lat!.Value, SpatialReferences.Wgs84);
            MainMapView.SetViewpoint(new Viewpoint(mapCenterPoint, 10000));
        }
    }
}
