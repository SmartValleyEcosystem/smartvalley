using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SmartValley.Data.SQL.Repositories;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Tests.Tests
{
    [TestFixture]
    public class ApplicationTestInit
    {
        /// <summary>
        /// Тестовая среда
        /// </summary>
        private readonly TestEnvironment _env;

        private List<Project> _projects;
        private List<TeamMember> _persons;
        private List<Application> _applications;
        public ApplicationTestInit()
        {
            _env = new TestEnvironment();
            TestData();
        }
        
        /// <summary>
        /// Инициализация данных
        /// </summary>
        private void TestData()
        {
            _projects = new List<Project>
                        {
                            new Project { Name="Project1", SolutionDesc = "Sol1", ProblemDesc = "Prob1", ProjectArea = "ProjA1", Country = "Russia", ProjectAddress = "PA1", AuthorAddress = "PA1"},
                            new Project { Name="Project2", SolutionDesc = "Sol2", ProblemDesc = "Prob2", ProjectArea = "ProjA2", Country = "USA", ProjectAddress = "PA2", AuthorAddress = "PA1"}
                        };

            _applications = new List<Application>
                            {
                                new Application { ProjectStatus = "ProjStat1", WhitePaperLink = "www1", CryptoCurrency = "Bitcoin", FinancialModelLink = "Fin1", HardCap = 50000.0m, InvestmentsAreAttracted = true, MVPLink = "mvp1", SoftCap = 50000.0m },
                                new Application { ProjectStatus = "ProjStat2",  WhitePaperLink = "www2", CryptoCurrency = "Ethereum", FinancialModelLink = "Fin2", HardCap = 20000.0m, InvestmentsAreAttracted = false, MVPLink = "mvp2", SoftCap = 30000.0m }
                            };
            _persons = new List<TeamMember>
                       {
                           new TeamMember { ApplicationId = _applications[0].Id, FullName = "Max Payne", PersonType = MemberType.PR, FacebookLink = "FB1", LinkedInLink = "LinkedIN1" },
                           new TeamMember { ApplicationId = _applications[1].Id, FullName = "Ivan Ivanov", PersonType = MemberType.CFO, FacebookLink = "FB2", LinkedInLink = "LinkedIN2" },
                           new TeamMember { ApplicationId = _applications[1].Id, FullName = "Vasya Pupkin", PersonType = MemberType.CMO, FacebookLink = "FB2", LinkedInLink = "LinkedIN2" }
                       };
        }

        [SetUp]
        public async Task LoadData()
        {
            await _env.LoadData(_persons);
            await _env.LoadData(_projects);
            await _env.LoadData(_applications);
        }

        [TearDown]
        public void ClearData()
        {
            _env.DeleteData(_applications);
            _env.DeleteData(_persons);
            _env.DeleteData(_projects);
        }

        [Test]
        public async Task CheckAdd()
        {
            var repository = new ApplicationRepository(_env.Context, _env.Context);
            var newApplication = new Application {ProjectId = _projects[0].Id, ProjectStatus = "ProjStat3", WhitePaperLink = "www3", CryptoCurrency = "Bitcoin", FinancialModelLink = "Fin3", HardCap = 50000.0m, InvestmentsAreAttracted = true, MVPLink = "mvp3", SoftCap = 50000.0m};

            await repository.AddAsync(newApplication);

            Assert.IsTrue(newApplication.Id != default(long));

            await repository.RemoveAsync(newApplication);
        }
    }
}
