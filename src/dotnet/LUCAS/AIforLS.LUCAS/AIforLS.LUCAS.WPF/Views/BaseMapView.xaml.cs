using CoordinateSharp;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Windows.Devices.Geolocation;

namespace AIforLS.LUCAS.WPF.Views
{
    /// <summary>
    /// Interaction logic for BaseMapView.xaml
    /// </summary>
    public partial class BaseMapView : UserControl
    {
        public BaseMapView()
        {
            InitializeComponent();
            //MapPoint mapCenterPoint = new MapPoint(-118.805, 34.027, SpatialReferences.Wgs84);
            //MainMapView.SetViewpoint(new Viewpoint(mapCenterPoint, 100000));
            UpdateViewPoint();
        }
        public void UpdateViewPoint()
        {
            var LUCAS2018Data = LUCASUtilities.GetLUCAS2018Data();
            var lat = LUCAS2018Data!.Select(x => x.TH_LAT).Average();
            var lon = LUCAS2018Data!.Select(x => x.TH_LONG).Average();

            Coordinate coordinate = new Coordinate(lat!.Value,lon!.Value);
            coordinate.Set_Datum(Earth_Ellipsoid_Spec.WGS84_1984);

            MapPoint mapCenterPoint = new MapPoint(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), SpatialReferences.Wgs84);
            MainMapView.SetViewpoint(new Viewpoint(mapCenterPoint, 100000));
        }
    }
}
