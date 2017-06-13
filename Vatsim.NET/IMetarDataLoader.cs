using System.Threading.Tasks;

namespace Vatsim.NET
{
    internal interface IMetarDataLoader
    {
        Task<string> LoadData(string dataUrl, string icaoCode);
    }
}