namespace SmartValley.Application.Contracts
{
    public class NethereumOptions
    {
        public string RpcAddress { get; set; }

        public ContractOptions EtherManagerContract { get; set; }

        public ContractOptions ProjectManagerContract { get; set; }
    }
}