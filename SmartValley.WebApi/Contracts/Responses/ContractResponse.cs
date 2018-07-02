using SmartValley.Ethereum.Contracts;

namespace SmartValley.WebApi.Contracts.Responses
{
    public class ContractResponse
    {
        public string Address { get; set; }

        public string Abi { get; set; }

        public static ContractResponse FromOptions(ContractOptions contractOptions)
        {
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi
                   };
        }
    }
}