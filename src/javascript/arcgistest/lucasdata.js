import data from "./lucasdata.json" assert { type: 'json' };   
//import lucasgeodata from "./lucasgeodata2018.json" assert { type: 'json' };      

//var lucasdata = data.filter(item => item["NUTS_0"] === "SE");
var lucasdata = data;
console.log(lucasdata.length);
console.log(data.length);

export function calculateAverage(field) {
    var total = 0;
    var count = 0;
    lucasdata.forEach(function(item, index) {
        total += item[field];
        count++;
    });
    return total / count;
}

export function getLocalFeatures(){
    var features = [];
    lucasdata.forEach(function(item, index) {
        features.push({
            type: "feature",
            geometry:{
                type: "point",
                x: item["TH_LONG"],
                y: item["TH_LAT"],
                coordinates: [item["TH_LONG"], item["TH_LAT"]],

            },
            attributes:{
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
            properties:{
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

export {lucasdata}