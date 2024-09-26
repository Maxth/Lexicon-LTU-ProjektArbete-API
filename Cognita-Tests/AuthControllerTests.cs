﻿using Microsoft.AspNetCore.Mvc.Testing;
using Cognita.API.Services;
using Cognita_API.Controllers;
using Cognita_Service;
using Cognita_API;
using System.Security.Policy;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using IntegrationTests;
using Cognita_API.Infrastructure.Data;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Net;
using Cognita_Infrastructure.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Cognita_Infrastructure.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using Cognita_Shared.Entities;
using Cognita_Shared.Enums;

namespace Cognita_Tests
{
    public class AuthControllerTests
    : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private readonly CognitaDbContext _context;
        private readonly CustomWebApplicationFactory _applicationFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private bool _testUserSeeded;

        const string baseHttpAddress = "https://localhost:7147/api/";

        public AuthControllerTests(CustomWebApplicationFactory applicationFactory)
        {
            //applicationFactory.ClientOptions.BaseAddress = new Uri("https://localhost:5000/api/");
            _httpClient = applicationFactory.CreateClient();
            _context = applicationFactory.Context;
            _applicationFactory = applicationFactory;
            _userManager = applicationFactory.UserManager;
            _testUserSeeded = false;
        }

        [Fact]
        public async Task LoginTest()
        {
            // Arrage
            await SeedTestUser();

            // Act
            var obj = new UserForAuthenticationDto
            {
                UserName = "Kalle",
                Password = "password123",
            };
            var response = await _httpClient.PostAsJsonAsync(baseHttpAddress + "authentication/login", obj);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterUserTest()
        {
            // Arrange
            var newUser = new UserForRegistrationDto() {
                Name = "Daniel M",
                Email = "daniel.m@hemsida.se",
                Password = "test123",
                CourseId = 1
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync(baseHttpAddress + "authentication", newUser);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task RefreshTokenTest()
        {
            // Arrange
            await SeedTestUser();

            // Act
            var obj = new UserForAuthenticationDto
            {
                UserName = "Kalle",
                Password = "password123",
            };
            // Tokens
            var response = await _httpClient.PostAsJsonAsync(baseHttpAddress + "authentication/login", obj);

            // Save tokens
            //var responseAsString = response.Content.ToString();

            // Act

            // Fetch with tokens
            //var response = await _httpClient.GetAsync("api/courses");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        private async Task SeedTestUser() {
            if (_testUserSeeded) return;
            var user = new ApplicationUser
            {
                UserName = "Kalle",
                Email = "kalle@example.com",
                User = new User
                {
                    Name = "Kalle",
                    Role = UserRole.Student,
                    CourseId = 1
                }
            };

            await _userManager.CreateAsync(user, "password123");
        }
    }
}