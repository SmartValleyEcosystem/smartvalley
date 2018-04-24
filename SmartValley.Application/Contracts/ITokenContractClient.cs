using System.Threading.Tasks;

namespace SmartValley.Application.Contracts
{
    public interface ITokenContractClient
    {
        Task<int> GetDecimalsAsync();
    }
}