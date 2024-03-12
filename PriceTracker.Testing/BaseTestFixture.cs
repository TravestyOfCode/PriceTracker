using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PriceTracker.Data;
using Respawn;
using Respawn.Graph;
using System.Data.Common;
using System.IO;

namespace PriceTracker.Testing;

public class BaseTestFixture : IAsyncLifetime
{
    private readonly IServiceScopeFactory _scopeFactory;
    private Respawner _respawner;
    private DbConnection _connection;

    public BaseTestFixture()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "Testing"
        });

        var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        builder.Configuration.AddJsonFile(Path.Combine(path, "appsettings.Testing.json"));

        builder.AddDataServices();

        // Add any mock services here, e.g.
        // builder.Services.ReplaceServiceWithSingletonMock<IHttpContextAccessor>();

        var provider = builder.Services.BuildServiceProvider();

        _scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

        var db = provider.GetRequiredService<ApplicationDBContext>();
        db.Database.EnsureDeleted();
        db.Database.Migrate();
    }

    public async Task InitializeAsync()
    {
        var respawnerOptions = new RespawnerOptions
        {
            TablesToIgnore = new Table[]
            {
                "AspNetRoleClaims",
                "AspNetRoles",
                "AspNetUserClaims",
                "AspNetUserLogins",
                "AspNetUserRoles",
                "AspNetUsers",
                "AspNetUserTokens",
            },
            DbAdapter = DbAdapter.SqlServer
        };

        var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDBContext>();
        _connection = context.Database.GetDbConnection();
        await _connection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_connection, respawnerOptions);

    }

    public TScopedService GetService<TScopedService>() => _scopeFactory.CreateScope().ServiceProvider.GetService<TScopedService>();

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        var mediator = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ISender>();
        return await mediator.Send(request);
    }

    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDBContext>();
        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public Task ResetDatabase()
    {
        return _respawner.ResetAsync(_connection);
    }

    public async Task DisposeAsync()
    {
        await _connection.CloseAsync();
    }
}

public static class ServiceCollectionServiceExtensions
{
    public static IServiceCollection ReplaceServiceWithSingletonMock<TService>(this IServiceCollection services) where TService : class
    {
        services.RemoveAll(typeof(TService));
        services.AddSingleton(_ => Mock.Of<TService>());
        return services;
    }
}

