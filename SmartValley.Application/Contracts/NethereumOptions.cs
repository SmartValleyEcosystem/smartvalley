namespace SmartValley.Application.Contracts
{
    public class NethereumOptions
    {
        public string RpcAddress { get; set; }

        public string Owner { get; set; }

        public string Password { get; set; }

        public ContractOptions TokenContract { get; set; }

        public ContractOptions MinterContract { get; set; }

        public ContractOptions EtherManagerContract { get; set; }

        public ContractOptions ProjectManagerContract { get; set; }

        public ContractOptions ProjectContract { get; set; }

        public ContractOptions VotingSprintContract { get; set; }

        public ContractOptions ScoringContract { get; set; }

        public ContractOptions VotingManagerContract { get; set; }
    }
}