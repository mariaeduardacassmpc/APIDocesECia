using ApiDoces.Infra.JWT;
using ApiDoces.Services;
using ApiDoces.Services.Report;
using Application.Interfaces;
using Application.Services;
using Application.Services.Report;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace ApiDoces.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ProductService>();
        services.AddScoped<CustomerService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<ExpenseService>();
        services.AddScoped<UserService>();
        services.AddScoped<DashboardService>();
        services.AddScoped<SaleService>();
        services.AddScoped<ExcelReportService>();
        services.AddScoped<CustomerReportService>();
        services.AddScoped<ProductReportService>();
        services.AddScoped<SaleReportService>();
        services.AddScoped<AuthService>();

        services.AddHttpClient<IEmailService, EmailService>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IImageStorage, FileSystemImageStorage>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        return services;
    }
}