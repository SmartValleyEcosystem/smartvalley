namespace SmartValley.Application.Contracts.Options
{
    public class NethereumOptions
    {
        public string RpcAddress { get; set; }

        public string Owner { get; set; }

        public string Password { get; set; }

        public ContractOptions TokenContract { get; set; }

        public ContractOptions MinterContract { get; set; }

        public ContractOptions EtherManagerContract { get; set; }

        public ContractOptions AdminRegistryContract { get; set; }

        public ContractOptions ScoringManagerContract { get; set; }

        public ContractOptions VotingSprintContract { get; set; }

        public ContractOptions ScoringContract { get; set; }

        public VotingManagerContractOptions VotingManagerContract { get; set; }
    }
}