using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiBasic.Controllers;
using WebApiBasic.Data;
using WebApiBasic.Dtos;
using WebApiBasic.Dtos.Account;
using WebApplication1.Models;

namespace UnitTest
{
    [TestFixture]
    class AccountControllerTests
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<ApplicationUser> _signInManager;
        private DBContext _context;
        private IConfigurationSection _jwtSettings;
        private AccountController accountController;

        [SetUp]
        public void SetUp()
        {
            _userManager = new UserManager<ApplicationUser>();
            _roleManager = new Mock<RoleManager<IdentityRole>();
            _signInManager = new Mock<SignInManager<ApplicationUser>();
            _context = new DBContext();
        }
        [TearDown]
        public void Dispose()
        {
            _context.Setup(a => a.Dispose());
        }
        [Test]
        public async Task RegisterAsync_WithReadDto_Return201statusCode()
        {
            //Arrage
            accountController = new AccountController(_userManager.Object, _roleManager.Object, _signInManager.Object, _context.Object, _jwtSettings.Object);
            var user = new RegisterDto()
            {
                Email = "ali@gmail.com",
                ConfirmPassword = "soheyl@M123",
                Password = "soheyl@M123",
                UserName = "ofking609"
            };
            //Act
            var succes =await accountController.RegisterAsync(user);
            //Assert
            Assert.IsInstanceOf<OkObjectResult>(succes); 
        }


        [Test]
        public async Task LoginAsync_WithLoginDto_Return201statusCode()
        {
            //Arrage
            accountController = new AccountController(_userManager.Object, _roleManager.Object, _signInManager.Object, _context.Object, _jwtSettings.Object);
            //Act
            var user = new LoginDto()
            {
                Email = "ali@gmail.com",
                Password = "soheyl@M123",
            };
            var succes = await accountController.LoginAsync(user);
            //Assert
            Assert.That("200",Is.EqualTo(succes));
        }


    }
}
