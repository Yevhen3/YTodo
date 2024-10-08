﻿using System.Diagnostics.CodeAnalysis;

using FluentValidation;

using Mediator;

using Microsoft.EntityFrameworkCore;

using YTodo.Application.Abstractions.TokeStorage;
using YTodo.Application.Abstractions.UserStorage;
using YTodo.Application.Behaviors;
using YTodo.Application.Options;
using YTodo.Application.Services.PasswordHasher;
using YTodo.Application.Services.Token;
using YTodo.Persistence;
using YTodo.Persistence.Implementations;

namespace YTodo.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Singleton);
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies(), ServiceLifetime.Singleton);

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IUserStorage, UserStorage>();
        services.AddSingleton<ITokenStorage, TokenStorage>();
        services.AddSingleton<ITokenService, TokenService>();
        
        services
            .AddOptions<TokenOptions>()
            .BindConfiguration("Jwt")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<YTodoDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}