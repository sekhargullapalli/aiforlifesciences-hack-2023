using AIforLS.LUCAS;

namespace Lucas.MetadataAnalysis
{
    public class LUCAS2018PointExtended: LUCAS2018Point
    {
        public List<string> SequenceFiles { get; set; } = new List<string>();
        public static LUCAS2018PointExtended FromLucas2018Point(LUCAS2018Point parent)
        {
            LUCAS2018PointExtended pointExtended = new LUCAS2018PointExtended();
            foreach (var property in typeof(LUCAS2018Point).GetProperties())
            {
                property.SetValue(pointExtended, property.GetValue(parent));
            }
            return pointExtended;
        }
    }
}
