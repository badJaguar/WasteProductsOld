﻿using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Repositories.UserManagement;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.MailService;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Logic.Services.MailService;
using WasteProducts.Logic.Services.UserService;

namespace WasteProducts.Logic.Tests.UserManagementTests
{
    public class UserServiceIntegrationTests
    {
        public const string NAME_OR_CONNECTION_STRING = "name=ConStrByServer";

        public static IUserRepository UserRepo;

        public static IUserRoleRepository RoleRepo;

        public static IMailService MailService;

        public static IUserService UserService;

        public static IUserRoleService RoleService;

        [OneTimeSetUp]
        public void TextFixtureSetUp()
        {
            UserRepo = new UserRepository(NAME_OR_CONNECTION_STRING);
            RoleRepo = new UserRoleRepository(NAME_OR_CONNECTION_STRING);
            MailService = new MailService(null, null, null);
            UserService = new UserService(MailService, UserRepo);
            RoleService = new UserRoleService(RoleRepo);
        }

        // тестируем регистрирование юзеров и делаем начальное заполнение таблицы юзерами
        [Test]
        public void UserIntegrTest_00AddingUserAndRole()
        {
            var recreator = new UserRepository(NAME_OR_CONNECTION_STRING);
            recreator.RecreateTestDatabase();

            User user1 = UserService.RegisterAsync("test49someemail@gmail.com", "Sergei", "qwerty1", "qwerty1").GetAwaiter().GetResult();
            User user2 = UserService.RegisterAsync("test50someemail@gmail.com", "Anton", "qwerty2", "qwerty2").GetAwaiter().GetResult();
            User user3 = UserService.RegisterAsync("test51someemail@gmail.com", "Alexander", "qwerty3", "qwerty3").GetAwaiter().GetResult();

            Assert.AreEqual("test49someemail@gmail.com", user1.Email);
            Assert.AreEqual("Anton", user2.UserName);
            Assert.IsNotNull(user1.Id);
        }

        // проверяем запрос юзера по правильным емейлу и паролю
        [Test]
        public void UserIntegrTest_01CorrectLoggingInByEmail()
        {
            User user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.Email, "test49someemail@gmail.com");
        }

        // проверяем запрос юзера по неверным емейлу и паролю
        [Test]
        public void UserIntegrTest_02IncorrectQueryingByEmail()
        {
            User user = UserService.LogInAsync("incorrectEmail", "incorrectPassword").GetAwaiter().GetResult();
            Assert.IsNull(user);
        }

        // тестируем создание роли, а так же проверяем, действительно ли роль создается в базе данных
        [Test]
        public void UserIntegrTest_03FindingRoleByCorrectRoleName()
        {
            UserRole roleToCreate = new UserRole() { Name = "Simple user" };
            RoleService.CreateAsync(roleToCreate).GetAwaiter().GetResult();

            UserRole role = RoleService.FindByNameAsync("Simple user").GetAwaiter().GetResult();
            Assert.AreEqual(role.Name, "Simple user");
        }

        // проверяем запрос роли по несуществующему названию
        [Test]
        public void UserIntegrTest_04FindingRoleByIncorrectRoleName()
        {
            UserRole role = RoleService.FindByNameAsync("Not existing role name").GetAwaiter().GetResult();
            Assert.IsNull(role);
        }

        // тестим, правильно ли работает функционал добавления роли и добавления юзера в роль
        [Test]
        public void UserIntegrTest_05AddingToTheUserDBNewRole()
        {
            User user1 = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            User user2 = UserService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = UserService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            UserService.AddToRoleAsync(user1, "Simple user").GetAwaiter().GetResult();
            UserService.AddToRoleAsync(user2, "Simple user").GetAwaiter().GetResult();
            UserService.AddToRoleAsync(user3, "Simple user").GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user1.Roles.FirstOrDefault());

            user1 = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user1.Roles.FirstOrDefault());
        }

        // тестируем, как работает метот GetRolesAsynс IUserService
        [Test]
        public void UserIntegrTest_06GettingRolesOfTheUser()
        {
            User user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            user.Roles = UserService.GetRolesAsync(user).GetAwaiter().GetResult();

            Assert.AreEqual("Simple user", user.Roles.FirstOrDefault());
        }

        // тестируем изъятие из роли
        [Test]
        public void UserIntegrTest_07RemovingUserFromRole()
        {
            User user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.Roles.FirstOrDefault(), "Simple user");

            UserService.RemoveFromRoleAsync(user, "Simple user").GetAwaiter().GetResult();
            Assert.IsNull(user.Roles.FirstOrDefault());

            user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.IsNull(user.Roles.FirstOrDefault());
        }

        // Тестируем добавление утверждения (Claim) в юзера
        [Test]
        public void UserIntegrTest_08AddingClaimToUser()
        {
            User user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            var claim = new Claim("SomeType", "SomeValue");

            UserService.AddClaimAsync(user, claim).GetAwaiter().GetResult();
            var userClaim = user.Claims.FirstOrDefault();
            Assert.AreEqual(userClaim.Type, claim.Type);
            Assert.AreEqual(userClaim.Value, claim.Value);

            user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            userClaim = user.Claims.FirstOrDefault();
            Assert.AreEqual(userClaim.Type, claim.Type);
            Assert.AreEqual(userClaim.Value, claim.Value);
        }

        // тестируем удаление утверждения из юзера
        [Test]
        public void UserIntegrTest_09DeletingClaimFromUser()
        {
            User user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.Claims.Count, 1);

            UserService.RemoveClaimAsync(user, user.Claims.FirstOrDefault()).GetAwaiter().GetResult();
            Assert.AreEqual(user.Claims.Count, 0);

            user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.Claims.Count, 0);
        }

        // тестируем добавление логина в юзера
        [Test]
        public void UserIntegrTest_10AddingLoginToUser()
        {
            User user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            UserService.AddLoginAsync(user, login).GetAwaiter().GetResult();
            var userLogin = user.Logins.FirstOrDefault();
            Assert.AreEqual(login, userLogin);

            user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            userLogin = user.Logins.FirstOrDefault();
            Assert.AreEqual(login, userLogin);
        }

        // тестируем удаление логина из юзера
        [Test]
        public void UserIntegrTest_11DeletingLoginFromUser()
        {
            User user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            var login = new UserLogin { LoginProvider = "SomeLoginProvider", ProviderKey = "SomeProviderKey" };

            Assert.AreEqual(user.Logins.Count, 1);
            UserService.RemoveLoginAsync(user, login).GetAwaiter().GetResult();
            Assert.AreEqual(user.Logins.Count, 0);

            user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.Logins.Count, 0);
        }

        // тестируем апдейт юзера
        [Test]
        public void UserIntegrTest_12UserUpdating()
        {
            User user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();

            string userPhoneNumber = user.PhoneNumber;
            Assert.AreEqual(userPhoneNumber, null);

            user.PhoneNumber = "+375172020327";
            UserService.UpdateAsync(user).GetAwaiter().GetResult();

            user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(user.PhoneNumber, "+375172020327");
        }

        // тестируем изменение пароля пользователя
        [Test]
        public void UserIntegrTest_13ResettingUserPassword()
        {
            User user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            user.PhoneNumber = "3334455";
            UserService.ResetPasswordAsync(user, "qwerty1", "New password", "New password").GetAwaiter().GetResult();

            user = UserService.LogInAsync("test49someemail@gmail.com", "New password").GetAwaiter().GetResult();
            Assert.AreNotEqual("3334455", user.PhoneNumber);
            UserService.ResetPasswordAsync(user, "New password", "qwerty1", "qwerty1").GetAwaiter().GetResult();
        }

        // тестируем добавление друзей
        [Test]
        public void UserIntegrTest_14AddingNewFriendsToUser()
        {
            User user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);

            User user2 = UserService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = UserService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            UserService.AddFriendAsync(user, user2).GetAwaiter().GetResult();
            UserService.AddFriendAsync(user, user3).GetAwaiter().GetResult();

            Assert.AreEqual(2, user.Friends.Count);

            user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(2, user.Friends.Count);
        }

        // тестируем удаление друзей
        [Test]
        public void UserIntegrTest_15DeletingFriendsFromUser()
        {
            User user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(2, user.Friends.Count);

            User user2 = UserService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = UserService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            UserService.DeleteFriendAsync(user, user2).GetAwaiter().GetResult();
            UserService.DeleteFriendAsync(user, user3).GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);

            user = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            Assert.AreEqual(0, user.Friends.Count);
        }

        // TODO доделать этот тест после того, как появится толковая реализация продуктов
        // тестируем добавление продуктов
        //[Test]
        public void UserIntegrTest_16AddingNewProductsToUser()
        {
            
        }

        // TODO доделать этот тест после того, как появится толковая реализация продуктов
        // тестируем удаление продуктов
        //[Test]
        public void UserIntegrTest_17DeletingProductsFromUser()
        {

        }

        // тестируем поиск роли по айди и имени
        [Test]
        public void UserIntegrTest_18FindRoleByIdAndName()
        {
            UserRole foundByName = RoleService.FindByNameAsync("Simple user").GetAwaiter().GetResult();
            Assert.AreEqual("Simple user", foundByName.Name);

            UserRole foundById = RoleService.FindByIdAsync(foundByName.Id).GetAwaiter().GetResult();
            Assert.AreEqual(foundByName.Name, foundById.Name);
            Assert.AreEqual(foundByName.Id, foundById.Id);
        }

        // тестируем получение всех пользователей определенной роли
        [Test]
        public void UserIntegrTest_19GettingRoleUsers()
        {
            User user1 = UserService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user2 = UserService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();
            UserRole role = RoleService.FindByNameAsync("Simple user").GetAwaiter().GetResult();

            IEnumerable<User> users = RoleService.GetRoleUsers(role).GetAwaiter().GetResult();
            User user1FromGetRoles = users.FirstOrDefault(u => u.Email == user1.Email);
            User user2FromGetRoles = users.FirstOrDefault(u => u.Email == user2.Email);
            Assert.AreEqual(user1.Id, user1FromGetRoles.Id);
            Assert.AreEqual(user2.Id, user2FromGetRoles.Id);
        }

        // тестируем изменение названия роли
        [Test]
        public void UserIntegrTest_20UpdatingRoleName()
        {
            User user1 = UserService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            Assert.AreEqual("Simple user", user1.Roles.FirstOrDefault());

            UserRole role = RoleService.FindByNameAsync("Simple user").GetAwaiter().GetResult();

            RoleService.UpdateRoleNameAsync(role, "Not so simple user").GetAwaiter().GetResult();
            User user2 = UserService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();
            Assert.AreEqual("Not so simple user", user2.Roles.FirstOrDefault());
        }

        // тестируем удаление роли
        [Test]
        public void UserIntegrTest_21DeletingRole()
        {
            User user1 = UserService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            Assert.AreEqual("Not so simple user", user1.Roles.FirstOrDefault());

            UserRole role = RoleService.FindByNameAsync("Not so simple user").GetAwaiter().GetResult();
            RoleService.DeleteAsync(role).GetAwaiter().GetResult();

            User user2 = UserService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();
            Assert.IsNull(user2.Roles.FirstOrDefault());
        }

        // тестируем удаление юзеров, а заодно и чистим базу до изначального состояния
        //[Test]
        public void UserIntegrTest_22DeletingUsers()
        {
            User user1 = UserService.LogInAsync("test49someemail@gmail.com", "qwerty1").GetAwaiter().GetResult();
            User user2 = UserService.LogInAsync("test50someemail@gmail.com", "qwerty2").GetAwaiter().GetResult();
            User user3 = UserService.LogInAsync("test51someemail@gmail.com", "qwerty3").GetAwaiter().GetResult();

            user1.PasswordHash = "NotWalidPassword";
            user1.UserName = "SomeNewUnsavedUserName";

            UserService.DeleteUserAsunc(user1).GetAwaiter().GetResult();
            UserService.DeleteUserAsunc(user2).GetAwaiter().GetResult();
            UserService.DeleteUserAsunc(user3).GetAwaiter().GetResult();
        }
    }
}