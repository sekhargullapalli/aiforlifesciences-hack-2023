using AIforLS.LUCAS;

namespace AI4LS.APP.Services
{
    public class LucasData2018Service
    {
        public IEnumerable<LUCAS2018Point> GetLucasData()
        {
            return LUCASUtilities.GetLUCAS2018Data()??new List<LUCAS2018Point>();
        }
    }
}
