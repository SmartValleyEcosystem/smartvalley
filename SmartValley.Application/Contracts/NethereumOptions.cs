namespace SmartValley.Application.Contracts
{
    public class NethereumOptions
    {
        public string RpcAddress { get; set; }

        public string Owner { get; set; }

        public string Password { get; set; }

        public ContractOptions EtherManagerContract { get; set; }

        public ContractOptions ProjectManagerContract { get; set; }
    }
}