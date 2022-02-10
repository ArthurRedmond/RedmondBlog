using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RedmondBlog.Data;
using RedmondBlog.Enums;
using RedmondBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedmondBlog.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManageDataAsync()
        {
            await _dbContext.Database.MigrateAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (_dbContext.Roles.Any())
            {
                return;
            }

            foreach (var role in Enum.GetNames(typeof(BlogRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task SeedUsersAsync()
        {
            if (_dbContext.Users.Any())
            {
                return;
            }

            var adminUser = new BlogUser()
            {
                Email = "Arthur@ArthurRedmond.com",
                UserName = "Arthur@ArthurRedmond.com",
                FirstName = "Arthur",
                LastName = "Redmond",
                DisplayName = "Arthur",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(adminUser, "Abc@123!");

            await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());

            var modUser = new BlogUser()
            {
                Email = "carshopper26@outlook.com",
                UserName = "carshopper26@outlook.com",
                FirstName = "Jon",
                LastName = "Smith",
                DisplayName = "Moderator",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(modUser, "Abc@123!");

            await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());
        }
    }
}
