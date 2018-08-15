﻿using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.Search;
using WasteProducts.Logic.Services;
using System.Linq;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Tests.Search_Tests
{
    [TestFixture]
    public class SearchService_Test_Passed
    {
        [SetUp]
        public void Setup()
        {
            users = new List<User>
            {
                new User { Id = 1, Login = "user1", Email = "user1@mail.net" },
                new User { Id = 2, Login = "user2", Email = "user2@mail.net" },
                new User { Id = 3, Login = "user3", Email = "user3@mail.net" },
                new User { Id = 4, Login = "user4", Email = "user4@mail.net" },
                new User { Id = 5, Login = "user5", Email = "user5@mail.net" }
            };

            mockRepo = new Mock<ISearchRepository>();
            sut = new LuceneSearchService(mockRepo.Object);
        }

        private IEnumerable<User> users;
        private Mock<ISearchRepository> mockRepo;
        private ISearchService sut;

        #region IEnumerable<TEntity> Search<TEntity>(SearchQuery query) where TEntity : class
        [Test]
        public void Search_GetAll_ReturnVerify()
        {
            mockRepo.Setup(x => x.GetAll<User>(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>())).Verifiable();

            var query = new SearchQuery();

            var result = sut.Search<User>(query);

            mockRepo.Verify(v => v.GetAll<User>(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Search_CheckContainsKey_ReturnTrue()
        {
            ////нужный метод репозитория
            //mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            //var query = new SearchQuery();

            //var result = sut.Search<User>(query);

            //Assert.AreEqual(true, result.Result.ContainsKey(typeof(User)));
        }

        [Test]
        public void Search_EmptyQuery_ReturnAllObjectsInRepository()
        {
            ////нужный метод репозитория
            //mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            //var query = new SearchQuery();

            //var result = sut.Search<User>(query);

            //Assert.AreEqual(1, result.Result.Count);
        }

        [Test]
        public void Search_login_by_id_Return_count_0()
        {
            ////нужный метод репозитория
            //mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            //var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            //var result = sut.Search<User>(query);

            //List<object> list = result.Result[typeof(User)].ToList();

            //Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void Search_login_by_login_Return_count_1()
        {
            //mockRepo.Setup(x => x.GetAll<User>(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>())).Returns(users);

            //var query = new SearchQuery();// { Query = "user1", SearchableFields = new string[] { "Login" } };

            //var result = sut.Search<User>(query);
            //List<object> list = result.Result[typeof(User)].ToList();

            //Assert.AreEqual(5, list.Count);
        }        

        [Test]
        public void SearchId_ByLogin_ReturnUser1()
        {
            //mockRepo.Setup(x => x.GetAll<User>(It.IsAny<int>())).Returns(users);

            //var query = new SearchQuery() { Query = "user1", SearchableFields = new string[] { "id" } };

            //var result = sut.Search<User>(query);
            //List<object> list = result.Result[typeof(User)].ToList();
            //var user = (User)list[0];

            //Assert.AreEqual(1, user.Id);
        }
        #endregion
    }
}
