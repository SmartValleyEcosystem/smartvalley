using System.Threading.Tasks;
using FluentAssertions;
using SmartValley.Application.Contracts;
using Xunit;

namespace SmartValley.Application.Tests
{
    public class ProjectManagerContractClientTests
    {
        private readonly ProjectManagerContractClient _sut;

        public ProjectManagerContractClientTests()
        {
            var nethereumOptions = new NethereumOptions
                                   {
                                       //RpcAddress = "http://localhost:8545",
                                       ProjectManagerContract = new ContractOptions
                                                                {
                                                                    Abi = "[{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"projects\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"_owner\",\"type\":\"address\"}],\"name\":\"changeOwner\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[],\"name\":\"confirmOwner\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"newOwner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"_author\",\"type\":\"address\"},{\"name\":\"_applicationHash\",\"type\":\"string\"},{\"name\":\"_name\",\"type\":\"string\"}],\"name\":\"addProject\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]",
                                                                    Address = "0x048a84f69AF53B2b6Fec8064C50868bB8926a133",
                                                                    Owner = "0x058E504DD34e4f07d1063BC03427CC236Ff1a33A",
                                                                    Password = "11235813"
                                                                }
                                   };

            _sut = new ProjectManagerContractClient(nethereumOptions);
        }

        [Fact]
        public async Task AddProject_ReturnsTheDeployedContractAddress()
        {
            string author = "0xa8878BC7e2CeEDAbA076f6ADCffBF40DFFcaC713";
            string hash = "somehash";

            var address = await _sut.AddProjectAsync(author, hash, "some name");
            address.Should().NotBeEmpty();
        }
    }
}