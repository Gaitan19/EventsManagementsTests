using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using EventsManagementApp.Controllers;
using EventsManagementApp.Service;
using EventsManagementApp.Repositories;
using EventsManagementApp.Models;
using EventsManagementsApp.DTOs;

namespace EventsManagementApp.Tests.Controllers
{
    public class RegistrationControllerTests
    {
        private readonly Mock<IRegistrationRepository> _mockRegistrationRepository;
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<IParticipantRepository> _mockParticipantRepository;
        private readonly RegistrationService _registrationService;
        private readonly EventService _eventService;
        private readonly ParticipantService _participantService;
        private readonly RegistrationController _controller;

        public RegistrationControllerTests()
        {
            // Mock repositories  
            _mockRegistrationRepository = new Mock<IRegistrationRepository>();
            _mockEventRepository = new Mock<IEventRepository>();
            _mockParticipantRepository = new Mock<IParticipantRepository>();

            // Instantiate services with mocked repositories  
            _registrationService = new RegistrationService(_mockRegistrationRepository.Object, _mockEventRepository.Object);
            _eventService = new EventService(_mockEventRepository.Object);
            _participantService = new ParticipantService(_mockParticipantRepository.Object);

            // Inject services into controller  
            _controller = new RegistrationController(_registrationService, _eventService, _participantService);
        }

        [Fact]
        public void Get_ReturnsAllRegistrations_WhenSuccessful()
        {
            // Arrange  
            var registrations = new List<Registration>
            {
                new Registration { Id = Guid.NewGuid(), EventId = Guid.NewGuid(), ParticipantId = Guid.NewGuid(), RegistrationDate = DateTime.Now },
                new Registration { Id = Guid.NewGuid(), EventId = Guid.NewGuid(), ParticipantId = Guid.NewGuid(), RegistrationDate = DateTime.Now }
            };
            _mockRegistrationRepository.Setup(repo => repo.GetAll()).Returns(registrations);

            // Act  
            var result = _controller.Get();

            // Assert  
            Assert.Equal(registrations, result);
            _mockRegistrationRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetById_ReturnsOkWithRegistration_WhenRegistrationExists()
        {
            // Arrange  
            var registrationId = Guid.NewGuid();
            var registration = new Registration
            {
                Id = registrationId,
                EventId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                RegistrationDate = DateTime.Now
            };
            _mockRegistrationRepository.Setup(repo => repo.GetById(registrationId)).Returns(registration);

            // Act  
            var result = _controller.Get(registrationId);

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(registration, okResult.Value);
            _mockRegistrationRepository.Verify(repo => repo.GetById(registrationId), Times.Once);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenRegistrationDoesNotExist()
        {
            // Arrange  
            var registrationId = Guid.NewGuid();
            _mockRegistrationRepository.Setup(repo => repo.GetById(registrationId)).Returns((Registration)null);

            // Act  
            var result = _controller.Get(registrationId);

            // Assert  
            Assert.IsType<NotFoundResult>(result.Result);
            _mockRegistrationRepository.Verify(repo => repo.GetById(registrationId), Times.Once);
        }

        [Fact]
        public void Post_ReturnsOk_WhenRegistrationIsSuccessful()
        {
            // Arrange  
            var eventId = Guid.NewGuid();
            var participantId = Guid.NewGuid();
            var registrationDto = new RegistrationDTO
            {
                EventId = eventId,
                ParticipantId = participantId,
                RegistrationDate = DateTime.Now
            };

            var eventEntity = new Event
            {
                Id = eventId,
                MaxCapacity = 100,
                Registrations = new List<Registration>() // Empty list, under capacity  
            };

            var participantEntity = new Participant { Id = participantId };

            _mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(eventEntity);
            _mockParticipantRepository.Setup(repo => repo.GetById(participantId)).Returns(participantEntity);

            // Act  
            var result = _controller.Post(registrationDto);

            // Assert  
            Assert.IsType<OkResult>(result);
            _mockEventRepository.Verify(repo => repo.GetById(eventId), Times.Exactly(2)); // Cambiado de Times.Once a Times.Exactly(2)
            _mockParticipantRepository.Verify(repo => repo.GetById(participantId), Times.Once);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenEventNotFound()
        {
            // Arrange  
            var registrationDto = new RegistrationDTO
            {
                EventId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                RegistrationDate = DateTime.Now
            };

            _mockEventRepository.Setup(repo => repo.GetById(registrationDto.EventId)).Returns((Event)null);

            // Act  
            var result = _controller.Post(registrationDto);

            // Assert  
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Event not found", badRequestResult.Value);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenParticipantNotFound()
        {
            // Arrange  
            var eventId = Guid.NewGuid();
            var registrationDto = new RegistrationDTO
            {
                EventId = eventId,
                ParticipantId = Guid.NewGuid(),
                RegistrationDate = DateTime.Now
            };

            var eventEntity = new Event { Id = eventId };
            _mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(eventEntity);
            _mockParticipantRepository.Setup(repo => repo.GetById(registrationDto.ParticipantId)).Returns((Participant)null);

            // Act  
            var result = _controller.Post(registrationDto);

            // Assert  
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Participant not found", badRequestResult.Value);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenEventIsFull()
        {
            // Arrange  
            var eventId = Guid.NewGuid();
            var participantId = Guid.NewGuid();
            var registrationDto = new RegistrationDTO
            {
                EventId = eventId,
                ParticipantId = participantId,
                RegistrationDate = DateTime.Now
            };

            var eventEntity = new Event
            {
                Id = eventId,
                MaxCapacity = 1,
                Registrations = new List<Registration> { new Registration() } // At capacity  
            };

            var participantEntity = new Participant { Id = participantId };

            _mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(eventEntity);
            _mockParticipantRepository.Setup(repo => repo.GetById(participantId)).Returns(participantEntity);

            // Act  
            var result = _controller.Post(registrationDto);

            // Assert  
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Event is full, registration not allowed", badRequestResult.Value);
        }

        [Fact]
        public void Put_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange  
            var registrationId = Guid.NewGuid();
            var registrationDto = new RegistrationDTO
            {
                EventId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                RegistrationDate = DateTime.Now
            };

            var existingRegistration = new Registration { Id = registrationId };
            _mockRegistrationRepository.Setup(repo => repo.GetById(registrationId)).Returns(existingRegistration);

            // Act  
            var result = _controller.Put(registrationId, registrationDto);

            // Assert  
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenDeleteIsSuccessful()
        {
            // Arrange  
            var registrationId = Guid.NewGuid();
            var existingRegistration = new Registration { Id = registrationId };
            _mockRegistrationRepository.Setup(repo => repo.GetById(registrationId)).Returns(existingRegistration);

            // Act  
            var result = _controller.Delete(registrationId);

            // Assert  
            Assert.IsType<OkResult>(result);
        }
    }
}