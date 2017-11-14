namespace SmartValley.WebApi.Contract
{
    public class NethereumOptions
    {
        public string RpcAddress { get; set; }

        public ContractOptions Contract { get; set; }
    }
}