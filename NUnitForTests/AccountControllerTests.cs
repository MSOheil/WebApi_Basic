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
using WebApplication1.Models;

namespace NUnitForTests
{
    [TestFixture]
    class AccountControllerTests
    {
        private Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<RoleManager<IdentityRole>> _roleManager;
        private Mock<SignInManager<ApplicationUser>> _signInManager;
        private Mock<DBContext> _context;
        private Mock<IConfigurationSection> _jwtSettings;
        private AccountController accountController;

        [SetUp]
        public void SetUp()
        {
            _userManager = new Mock<UserManager<ApplicationUser>>();
            _roleManager = new Mock<RoleManager<IdentityRole>>();
            _signInManager = new Mock<SignInManager<ApplicationUser>>();
            _context = new Mock<DBContext>();
        }

        [Test]
        public async Task RegisterAsync_WithReadDto_Return201statusCode()
        {
            //Arrage
            accountController = new AccountController(_userManager.Object, _roleManager.Object, _signInManager.Object, _context.Object, _jwtSettings.Object);
            //Act
            RegisterDto user = new RegisterDto()
            {
                Email = "ali@gmail.com",
                ConfirmPassword = "soheyl@M123",
                Password = "soheyl@M123",
                UserName = "ofking609"
            };
             var succes=await accountController.RegisterAsync(user);
            //Assert
            Assert.IsInstanceOf<OkObjectResult>(succes); 
        }





    }
}
