﻿using System.Diagnostics;
using Testcontainers.PostgreSql;

namespace TCTest.DomainService.Test;

public class DbInstance : IAsyncLifetime
{
    private PostgreSqlContainer _postgres = default!;

    private string Dir
    {
        get
        {
            var dir = System.Environment.CurrentDirectory;
            return $"{dir}/../../../../../db/tctest-postgresql/init";
        }
    }

    public string DbConnectionString => _postgres.GetConnectionString();

    public Task CreateAsync()
    {
        try
        {
            _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithResourceMapping(Dir, @"/docker-entrypoint-initdb.d")
            .Build();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return _postgres.StartAsync();
    }

    public Task InitializeAsync()
    {
        return CreateAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }
}