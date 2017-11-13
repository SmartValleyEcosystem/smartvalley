using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Data.SQL.Tests
{
    [TestFixture]
    public class TestEnvironment
    {
        private readonly AppDBContext _db;
        private List<Project> _projects;
        private List<Country> _countries;
        private List<Person> _persons;
        private List<Application> _applications;
        private List<CryptoCurrency> _cryptoCurrencies;
        private List<PersonApplication> _personsApplications;

        public TestEnvironment()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=smartValleyDB;Integrated Security=True;MultipleActiveResultSets=True");
            _db = new AppDBContext(optionsBuilder.Options);

            TestData();
        }

        private void TestData()
        {
            _cryptoCurrencies = new List<CryptoCurrency>
                                {
                                    new CryptoCurrency { Name = "Bitcoin", Code = "BTC" },
                                    new CryptoCurrency { Name = "Ethereum", Code = "eth" }
                                };

            _countries = new List<Country>
                         {
                             new Country{Name = "United States of America", Code = "US"},
                             new Country{Name="Russian Federation", Code="RU"}
                         };

            _persons = new List<Person>
                       {
                           new Person { FirstName = "Max", SecondName = "Payne", PersonType = PersonType.PR, FacebookLink = "FB1", LinkedInLink = "LinkedIN1" },
                           new Person { FirstName = "Ivan", SecondName = "Ivanov", PersonType = PersonType.CFO, FacebookLink = "FB2", LinkedInLink = "LinkedIN2" },
                           new Person { FirstName = "Vasya", SecondName = "Pupkin", PersonType = PersonType.CMO, FacebookLink = "FB2", LinkedInLink = "LinkedIN2" }
                       };

            _projects = new List<Project>
                        {
                            new Project { Name="Project1", SolutionDesc = "Sol1", ProblemDesc = "Prob1", ProjectArea = "ProjA1", ProjectStatus = "ProjStat1", WhitePaperLink = "www1"},
                            new Project { Name="Project2", SolutionDesc = "Sol2", ProblemDesc = "Prob2", ProjectArea = "ProjA2", ProjectStatus = "ProjStat2", WhitePaperLink = "www2"}
                        };

            _applications = new List<Application>
                            {
                                new Application { Project = _projects[0], Country = _countries[0], CryptoCurrency = _cryptoCurrencies[0], FinancialModelLink = "Fin1", HardCap = 50000.0m, InvestmentsAreAttracted = true, MVPLink = "mvp1", SoftCap = 50000.0m },
                                new Application { Project = _projects[1], Country = _countries[1], CryptoCurrency = _cryptoCurrencies[1], FinancialModelLink = "Fin2", HardCap = 20000.0m, InvestmentsAreAttracted = false, MVPLink = "mvp2", SoftCap = 30000.0m }
                            };

            _personsApplications = new List<PersonApplication>
                                   {
                                       new PersonApplication {Application = _applications[0], Person = _persons[0]},
                                       new PersonApplication {Application = _applications[1], Person = _persons[0]},
                                       new PersonApplication {Application = _applications[1], Person = _persons[1]}
                                   };
        }

        [SetUp]
        public void LoadData()
        {
            _db.AddRange(_cryptoCurrencies);
            _db.AddRange(_countries);
            _db.AddRange(_persons);
            _db.AddRange(_projects);
            _db.AddRange(_applications);
            _db.AddRange(_personsApplications);

            _db.SaveChanges();
        }

        [TearDown]
        public void ClearData()
        {
            _db.RemoveRange(_personsApplications);
            _db.RemoveRange(_applications);
            _db.RemoveRange(_cryptoCurrencies);
            _db.RemoveRange(_countries);
            _db.RemoveRange(_persons);
            _db.RemoveRange(_projects);

            _db.SaveChanges();
        }

        [Test]
        public void CheckAdd()
        {
            var newApplication = new Application
            {
                Country = new Country { Code = "RU", Name = "Russian Federation" },
                Project = new Project { Name = "Project1", ProblemDesc = "Prob1", ProjectArea = "PA1", ProjectStatus = "Cont", SolutionDesc = "SolDe1", WhitePaperLink = "WPLink1" },
                CryptoCurrency = _cryptoCurrencies[0]
            };

            _db.Applications.Add(newApplication);
            _db.SaveChanges();

            Assert.IsTrue(newApplication.Id != Guid.Empty);

            _db.Remove(newApplication);
            _db.SaveChanges();
        }
    }
}
