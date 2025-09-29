using System.Security.Claims;
using DNDProject.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DNDProject.Api.Data;

public static class IdentitySeed
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var db      = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await db.Customers.AnyAsync(c => c.Id == 1))
        {
            db.Customers.Add(new Customer { Id = 1, Name = "Demo-kunde" });
            await db.SaveChangesAsync();
        }

        // Roller
        string[] roles = { "Employee", "Customer" };
        foreach (var r in roles)
        {
            if (!await roleMgr.RoleExistsAsync(r))
                await roleMgr.CreateAsync(new IdentityRole(r));
        }

        // Employee/admin
        var adminEmail = "admin@demo.local";
        var admin = await userMgr.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            await userMgr.CreateAsync(admin, "Pass123!");
            await userMgr.AddToRoleAsync(admin, "Employee");
        }

        // Customer (koblet til CustomerId = 1)
        var custEmail = "customer@demo.local";
        var customerUser = await userMgr.FindByEmailAsync(custEmail);
        if (customerUser is null)
        {
            customerUser = new ApplicationUser
            {
                UserName = custEmail,
                Email = custEmail,
                EmailConfirmed = true,
                CustomerId = 1
            };
            await userMgr.CreateAsync(customerUser, "Pass123!");
            await userMgr.AddToRoleAsync(customerUser, "Customer");

            // Claim så vi kan filtrere “mine data” senere
            await userMgr.AddClaimAsync(customerUser, new Claim("customerId", "1"));
        }
    }
}
