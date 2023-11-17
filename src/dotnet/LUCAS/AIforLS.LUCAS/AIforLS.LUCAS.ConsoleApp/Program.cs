using AIforLS.LUCAS;

var lucasPoints = LUCASUtilities.GetLUCAS2018Data();
var lucaspointsSwedish = lucasPoints!.Where(p => p.NUTS_0 == "SE");
Console.WriteLine(lucasPoints!.Count());

