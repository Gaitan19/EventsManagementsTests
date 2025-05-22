using System;
using System.Collections.Generic;
using System.Linq;
using EventsManagementApp.Controllers;
using EventsManagementApp.Models;
using EventsManagementApp.Repositories;
using EventsManagementApp.Service;
using EventsManagementsApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EventsManagementApp.Tests.Controllers
{
    public class ParticipantControllerTests
    {
        private readonly Mock<IParticipantRepository> _mockRepository;
        private readonly ParticipantService _participantService;
        private readonly ParticipantController _controller;

        public ParticipantControllerTests()
        {
            _mockRepository = new Mock<IParticipantRepository>();
            _participantService = new ParticipantService(_mockRepository.Object);
            _controller = new ParticipantController(_participantService);
        }

        [Fact]
        public void Get_ReturnsAllParticipants()
        {
            // Arrange  
            var participants = new List<Participant>
            {
                new Participant { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@example.com", Phone = "123-456-7890" },
                new Participant { Id = Guid.NewGuid(), Name = "Jane Smith", Email = "jane@example.com", Phone = "098-765-4321" }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(participants);

            // Act  
            var result = _controller.Get();

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void Get_WithValidId_ReturnsParticipant()
        {
            // Arrange  
            var participantId = Guid.NewGuid();
            var participant = new Participant
            {
                Id = participantId,
                Name = "John Doe",
                Email = "john@example.com",
                Phone = "123-456-7890"
            };
            _mockRepository.Setup(repo => repo.GetById(participantId)).Returns(participant);

            // Act  
            var result = _controller.Get(participantId);

            // Assert  
            Assert.NotNull(result);
            Assert.IsType<ActionResult<Participant>>(result);
            _mockRepository.Verify(repo => repo.GetById(participantId), Times.Once);
        }

        [Fact]
        public void Get_WithInvalidId_ReturnsNull()
        {
            // Arrange  
            var participantId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetById(participantId)).Returns((Participant)null);

            // Act  
            var result = _controller.Get(participantId);

            // Assert  
            Assert.NotNull(result);
            _mockRepository.Verify(repo => repo.GetById(participantId), Times.Once);
        }

        [Fact]
        public void Post_WithValidParticipant_ReturnsOk()
        {
            // Arrange  
            var participantDto = new ParticipantDTO
            {
                Name = "New Participant",
                Email = "new@example.com",
                Phone = "555-0123"
            };

            _mockRepository.Setup(repo => repo.Add(It.IsAny<Participant>()));
            _mockRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _controller.Post(participantDto);

            // Assert  
            Assert.IsType<OkResult>(result);
            _mockRepository.Verify(repo => repo.Add(It.IsAny<Participant>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Put_WithValidIdAndParticipant_ReturnsOk()
        {
            // Arrange  
            var participantId = Guid.NewGuid();
            var existingParticipant = new Participant
            {
                Id = participantId,
                Name = "Old Name",
                Email = "old@example.com",
                Phone = "111-222-3333"
            };
            var participantDto = new ParticipantDTO
            {
                Name = "Updated Name",
                Email = "updated@example.com",
                Phone = "444-555-6666"
            };

            _mockRepository.Setup(repo => repo.GetById(participantId)).Returns(existingParticipant);
            _mockRepository.Setup(repo => repo.Update(It.IsAny<Participant>()));
            _mockRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _controller.Put(participantId, participantDto);

            // Assert  
            Assert.IsType<OkResult>(result);
            _mockRepository.Verify(repo => repo.GetById(participantId), Times.Once);
            _mockRepository.Verify(repo => repo.Update(It.IsAny<Participant>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Put_WithInvalidId_DoesNotUpdate()
        {
            // Arrange  
            var participantId = Guid.NewGuid();
            var participantDto = new ParticipantDTO
            {
                Name = "Updated Name",
                Email = "updated@example.com",
                Phone = "444-555-6666"
            };

            _mockRepository.Setup(repo => repo.GetById(participantId)).Returns((Participant)null);

            // Act  
            var result = _controller.Put(participantId, participantDto);

            // Assert  
            Assert.IsType<OkResult>(result);
            _mockRepository.Verify(repo => repo.GetById(participantId), Times.Once);
            _mockRepository.Verify(repo => repo.Update(It.IsAny<Participant>()), Times.Never);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Delete_WithValidId_ReturnsOk()
        {
            // Arrange  
            var participantId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Delete(participantId));
            _mockRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _controller.Delete(participantId);

            // Assert  
            Assert.IsType<OkResult>(result);
            _mockRepository.Verify(repo => repo.Delete(participantId), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }
    }
}