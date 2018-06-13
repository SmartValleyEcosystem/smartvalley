using SmartValley.Ethereum.Contracts;

namespace SmartValley.Ethereum
{
    public class NethereumOptions
    {
        public string RpcAddress { get; set; }

        public string Owner { get; set; }

        public string Password { get; set; }

        public int? TransactionConfirmationsCount { get; set; }

        public ContractOptions EtherManagerContract { get; set; }

        public ContractOptions AdminRegistryContract { get; set; }

        public ContractOptions ScoringsRegistryContract { get; set; }

        public ContractOptions ScoringManagerContract { get; set; }

        public ContractOptions AllotmentEventsManagerContract { get; set; }

        public ContractOptions PrivateScoringManagerContract { get; set; }

        public ContractOptions ScoringContract { get; set; }

        public ContractOptions ExpertsRegistryContract { get; set; }

        public ContractOptions ScoringOffersManagerContract { get; set; }

        public ContractOptions ScoringParametersProviderContract { get; set; }
    }
}