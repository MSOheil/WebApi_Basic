//using AutoMapper;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using WebApiBasic.Controllers;
//using WebApiBasic.Dtos;
//using WebApiBasic.Models;
//using WebApiBasic.Repositories;

//namespace UnitTest
//{
//    [TestFixture]
//    public class PatientControllerTests
//    {

//        private IMapper _mapper;
//        private IPatientService _patientRepository;
//        private readonly ILogger<PatientController> _logger;
//        public PatientControllerTests(IPatientService patientRepository, IMapper mapper, ILogger<PatientController> logger)
//        {
//            _patientRepository = patientRepository;
//            _mapper = mapper;
//            _logger = logger;
//        }

//        [Test]
//        public void GetAllPatientAsync_WithUnexistingItem_ReturnsNotFoun()
//        {
//            //Arrage
//            var controller = new PatientController(_patientRepository, _mapper, _logger);
//            //Act
//            var result = controller.GetAllPatientAsync();
//            //Assert
//        }


//    }
//}
