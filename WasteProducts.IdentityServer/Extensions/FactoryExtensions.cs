﻿using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.IdentityServer.Config;

namespace WasteProducts.IdentityServer.Extensions
{
    public static class IdentityServerServiceFactoryExtensions
    {
        public static IdentityServerServiceFactory Configure(this IdentityServerServiceFactory factory)
        {
            factory.Register(new Registration<ISearchRepository, LuceneSearchRepository>());

            factory
                .UseInMemoryClients(Clients.Get())
                .UseInMemoryScopes(Scopes.Get());

            factory.Register(new Registration<WasteContext>());
            factory.Register(new Registration<UserStore>());
            factory.Register(new Registration<UserManager>());
            factory.UserService = new Registration<IUserService, IdentityUserService>();

            return factory;
        }
    }
}