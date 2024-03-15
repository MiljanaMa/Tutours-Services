﻿using Explorer.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Explorer.BuildingBlocks.Tests;

public abstract class BaseTestFactory<TDbContext> : WebApplicationFactory<Program> where TDbContext : DbContext
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            using var scope = BuildServiceProvider(services).CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<TDbContext>();
            var logger = scopedServices.GetRequiredService<ILogger<BaseTestFactory<TDbContext>>>();

            InitializeDatabase(db, Path.Combine(".", "..", "..", "..", "TestData"), logger);
        });
    }

    private static void InitializeDatabase(DbContext context, string scriptFolder, ILogger logger)
    {
        try
        {
            context.Database.EnsureCreated();
            var databaseCreator = context.Database.GetService<IRelationalDatabaseCreator>();
            databaseCreator.CreateTables();
        }
        catch (Exception)
        {
            // CreateTables throws an exception if the schema already exists. This is a workaround for multiple dbcontexts.
        }

        try
        {
            var scriptFiles = Directory.GetFiles(scriptFolder);
            Array.Sort(scriptFiles);
            var script = string.Join('\n', scriptFiles.Select(File.ReadAllText));
            context.Database.ExecuteSqlRaw(script);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the database with test data. Error: {Message}", ex.Message);
        }
    }

    private ServiceProvider BuildServiceProvider(IServiceCollection services)
    {
        return ReplaceNeededDbContexts(services).BuildServiceProvider();
    }

    protected abstract IServiceCollection ReplaceNeededDbContexts(IServiceCollection services);

    protected static Action<DbContextOptionsBuilder> SetupTestContext()
    {
        var server = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";
        var database = Environment.GetEnvironmentVariable("DATABASE_SCHEMA") ?? "explorer-v1-test";
        var user = Environment.GetEnvironmentVariable("DATABASE_USERNAME") ?? "postgres";
        var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "super";
        var integratedSecurity = Environment.GetEnvironmentVariable("DATABASE_INTEGRATED_SECURITY") ?? "false";
        var pooling = Environment.GetEnvironmentVariable("DATABASE_POOLING") ?? "true";

        var connectionString =
            $"Server={server};Port={port};Database={database};User ID={user};Password={password};Integrated Security={integratedSecurity};Pooling={pooling};Include Error Detail=True";

        return opt => opt.UseNpgsql(connectionString);
    }
}