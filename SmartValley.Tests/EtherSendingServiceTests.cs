using System.Threading.Tasks;
using SmartValley.WebApi.EtherSending;
using Xunit;

namespace SmartValley.Tests
{
	public class EtherSendingServiceTests
	{
		private readonly EtherSendingService _sut;
		private readonly string _abi = "[\n    {\n      \"constant\": true,\n      \"inputs\": [\n        {\n          \"name\": \"\",\n          \"type\": \"address\"\n        }\n      ],\n      \"name\": \"receiversMap\",\n      \"outputs\": [\n        {\n          \"name\": \"\",\n          \"type\": \"bool\"\n        }\n      ],\n      \"payable\": false,\n      \"type\": \"function\"\n    },\n    {\n      \"constant\": false,\n      \"inputs\": [\n        {\n          \"name\": \"_receiver\",\n          \"type\": \"address\"\n        }\n      ],\n      \"name\": \"giftEth\",\n      \"outputs\": [\n        {\n          \"name\": \"\",\n          \"type\": \"uint256\"\n        }\n      ],\n      \"payable\": false,\n      \"type\": \"function\"\n    },\n    {\n      \"constant\": true,\n      \"inputs\": [],\n      \"name\": \"owner\",\n      \"outputs\": [\n        {\n          \"name\": \"\",\n          \"type\": \"address\"\n        }\n      ],\n      \"payable\": false,\n      \"type\": \"function\"\n    },\n    {\n      \"constant\": true,\n      \"inputs\": [],\n      \"name\": \"weiAmountToGift\",\n      \"outputs\": [\n        {\n          \"name\": \"\",\n          \"type\": \"uint256\"\n        }\n      ],\n      \"payable\": false,\n      \"type\": \"function\"\n    },\n    {\n      \"constant\": false,\n      \"inputs\": [\n        {\n          \"name\": \"_owner\",\n          \"type\": \"address\"\n        }\n      ],\n      \"name\": \"changeOwner\",\n      \"outputs\": [],\n      \"payable\": false,\n      \"type\": \"function\"\n    },\n    {\n      \"constant\": false,\n      \"inputs\": [],\n      \"name\": \"confirmOwner\",\n      \"outputs\": [],\n      \"payable\": false,\n      \"type\": \"function\"\n    },\n    {\n      \"constant\": true,\n      \"inputs\": [\n        {\n          \"name\": \"\",\n          \"type\": \"uint256\"\n        }\n      ],\n      \"name\": \"receivers\",\n      \"outputs\": [\n        {\n          \"name\": \"\",\n          \"type\": \"address\"\n        }\n      ],\n      \"payable\": false,\n      \"type\": \"function\"\n    },\n    {\n      \"constant\": true,\n      \"inputs\": [],\n      \"name\": \"newOwner\",\n      \"outputs\": [\n        {\n          \"name\": \"\",\n          \"type\": \"address\"\n        }\n      ],\n      \"payable\": false,\n      \"type\": \"function\"\n    },\n    {\n      \"constant\": false,\n      \"inputs\": [\n        {\n          \"name\": \"_weiAmountToGift\",\n          \"type\": \"uint256\"\n        }\n      ],\n      \"name\": \"setAmountToGift\",\n      \"outputs\": [],\n      \"payable\": false,\n      \"type\": \"function\"\n    },\n    {\n      \"inputs\": [],\n      \"payable\": true,\n      \"type\": \"constructor\"\n    },\n    {\n      \"payable\": true,\n      \"type\": \"fallback\"\n    }\n  ]";

		public EtherSendingServiceTests()
		{
			_sut = new EtherSendingService(
				"0x87523e068c08d1f5fd314fcb7b1541ca3ecaa1be",
				"0x9c14a0e2ae9a2bdce66c8d1450c64f4f8f091774",
				_abi,
				"password",
				"http://localhost:8545");
		}

		[Fact]
		public async Task GetBalanceReturnsCorrectBalance()
		{
			const string address = "0xfd7412905d69254048ac0f07c75a8af07c577262";

			var balanceBefore = await _sut.GetBalanceAsync(address);
			Assert.Equal(100m, balanceBefore);

			//await _sut.GiftEthAsync(address);
			
			//var balanceAfter = await _sut.GetBalanceAsync(address);

			var received = await _sut.WasGiftEtherSentAsync(address);
			
			Assert.Equal(true, received);
			//Assert.Equal(101m, balanceAfter);
		}
	}
}