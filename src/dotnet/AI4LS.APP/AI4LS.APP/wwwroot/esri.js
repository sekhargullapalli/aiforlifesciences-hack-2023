function calculateAverage(lucasdata, field) {
    var total = 0;
    var count = 0;
    lucasdata.forEach(function (item, index) {
        total += item[field];
        count++;
    });
    return total / count;
}
function getLocalFeatures(lucasdata) {
    var features = [];
    lucasdata.forEach(function (item, index) {
        features.push({
            type: "feature",
            geometry: {
                type: "point",
                x: item["TH_LONG"],
                y: item["TH_LAT"],
                coordinates: [item["TH_LONG"], item["TH_LAT"]],

            },
            attributes: {
                POINTID: item["POINTID"],
                ObjectID: item["POINTID"],
                pH_H2O: item["pH_H2O"],
                pH_CaCl2: item["pH_CaCl2"],
                EC: item["EC"],
                OC: item["OC"],
                CaCO3: item["CaCO3"],
                P: item["P"],
                N: item["N"],
                K: item["K"]
            },
            properties: {
                POINTID: item["POINTID"],
                ObjectID: item["POINTID"],
                pH_H2O: item["pH_H2O"],
                pH_CaCl2: item["pH_CaCl2"],
                EC: item["EC"],
                OC: item["OC"],
                CaCO3: item["CaCO3"],
                P: item["P"],
                N: item["N"],
                K: item["K"]
            }
        });
    });
    return features;
}


export function RenderHeatMap(lucasData, renderedParam, apikey,basemapid) {

    require(
        [
            "esri/config",
            "esri/Map",
            "esri/WebMap",
            "esri/layers/FeatureLayer",
            "esri/views/MapView",
            "esri/widgets/Legend",
            "esri/core/reactiveUtils",
            "esri/smartMapping/renderers/heatmap"
        ], function (esriConfig, Map, WebMap, FeatureLayer, MapView, Legend, reactiveUtils, heatmapRendererCreator) {

            esriConfig.apiKey = apikey;

            //scalesequential interpolators: http://using-d3js.com/04_05_sequential_scales.html
            const colscale = d3.scaleSequential()
                .domain(d3.extent([0, 1]))
                .interpolator(d3.interpolateRainbow);

            console.log("COL SCALE");
            console.log(colscale);

            var avgLong = calculateAverage(lucasData, "TH_LONG");
            var avgLat = calculateAverage(lucasData, "TH_LAT");
            var localFeatures = getLocalFeatures(lucasData);
            


            var layer = new FeatureLayer({
                title: `${renderedParam} from Lucas Data (2018)`,
                objectIdField: "ObjectID",
                source: localFeatures,
                fields: [{
                    name: "ObjectID", alias: "ObjectID", type: "oid"
                }, {
                    name: "pH_H2O", alias: "pH_H2O", type: "double"
                },
                {
                    name: "pH_CaCl2", alias: "pH_CaCl2", type: "double"
                },
                {
                    name: "EC", alias: "EC", type: "double"
                },
                {
                    name: "OC", alias: "OC", type: "double"
                },
                {
                    name: "CaCO3", alias: "CaCO3", type: "double"
                }, {
                    name: "P", alias: "P", type: "double"
                }, {
                    name: "N", alias: "N", type: "double"
                }, {
                    name: "K", alias: "K", type: "double"
                }
                ],
            });

            var min = Math.min(...localFeatures.map(item => item.attributes.pH_H2O));
            var max = Math.max(...localFeatures.map(item => item.attributes.pH_H2O));
            //console.log([min, max]);

            layer.renderer = {
                type: "heatmap",
                field: renderedParam,
            };

            const map = new WebMap({
                basemap: {
                    portalItem: {
                        id: basemapid
                    }
                },
                layers: [layer]
            });


            const view = new MapView({
                map: map,
                container: "viewHeatMapDiv",
                center: [avgLong, avgLat],
                scale: 25055581,
            });

            view.ui.add(
                new Legend({
                    view,
                    layerInfos: [
                        {
                            layer: layer,

                        }
                    ],
                    id:"heatmapLegend",
                }),
                "bottom-right");

            view.when(async () => {
                const layerView = await view.whenLayerView(layer);
                const { renderer } = await heatmapRendererCreator.createRenderer({ layer, view });
                renderer.colorStops = [
                    { ratio: 0, color: "rgba(255, 255, 255, 0)" },
                    { ratio: 0.1, color: colscale(0.1) },
                    { ratio: 0.2, color: colscale(0.2) },
                    { ratio: 0.3, color: colscale(0.3) },
                    { ratio: 0.4, color: colscale(0.4) },
                    { ratio: 0.5, color: colscale(0.5) },
                    { ratio: 0.6, color: colscale(0.6) },
                    { ratio: 0.7, color: colscale(0.7) },
                    { ratio: 0.8, color: colscale(0.8) },
                    { ratio: 0.9, color: colscale(0.9) },
                    { ratio: 1, color: colscale(1) },
                ];


                renderer.referenceScale = view.scale;
                layer.renderer = renderer;

                reactiveUtils
                    .whenOnce(() => !layerView?.updating)
                    .then(() => {
                        layer.opacity = 1;
                    });
            });
        });
}


export function RenderPointMap(lucasData, apikey) {

    require(
        [
            "esri/config",
            "esri/Map",
            "esri/views/MapView",
            "esri/Graphic",
            "esri/layers/GraphicsLayer"
        ], function (esriConfig, Map, MapView, Graphic, GraphicsLayer) {

            esriConfig.apiKey = apikey;
            var avgLong = calculateAverage(lucasData, "TH_LONG");
            var avgLat = calculateAverage(lucasData, "TH_LAT");           

            const map = new Map({
                basemap: "arcgis/topographic"
            });

            const view = new MapView({
                map: map,
                center: [avgLong, avgLat],
                zoom: 4,
                container: "viewPointMapDiv"
            });

            const graphicsLayer = new GraphicsLayer();
            map.add(graphicsLayer);

            const simpleMarkerSymbol = {
                type: "simple-marker",
                color: [226, 119, 40],
                outline: {
                    color: [0, 0, 0],
                    width: 0.2
                },
                size: 6
            };
            lucasData.forEach(function (item, index) {
                const point = {
                    type: "point",
                    longitude: item.TH_LONG,
                    latitude: item.TH_LAT
                };
                const pointGraphic = new Graphic({
                    geometry: point,
                    symbol: simpleMarkerSymbol
                });
                graphicsLayer.add(pointGraphic);
            });

    });
}


