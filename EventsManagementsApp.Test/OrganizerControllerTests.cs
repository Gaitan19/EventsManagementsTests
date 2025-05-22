using EventsManagementApp.Controllers;
using EventsManagementApp.DTOs;
using EventsManagementApp.Models;
using EventsManagementApp.Repositories;
using EventsManagementApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EventsManagementApp.Tests.Controllers
{
    public class OrganizerControllerTests
    {
        private readonly Mock<IOrganizerRepository> _mockRepository;
        private readonly OrganizerService _organizerService;
        private readonly OrganizerController _controller;

        public OrganizerControllerTests()
        {
            _mockRepository = new Mock<IOrganizerRepository>();
            _organizerService = new OrganizerService(_mockRepository.Object);
            _controller = new OrganizerController(_organizerService);
        }

        [Fact]
        public void GetAll_ReturnsOkResultWithOrganizers()
        {
            // Arrange  
            var organizers = new List<Organizer>
            {
                new Organizer { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@example.com", Phone = "123-456-7890" },
                new Organizer { Id = Guid.NewGuid(), Name = "Jane Smith", Email = "jane@example.com", Phone = "098-765-4321" }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(organizers);

            // Act  
            var result = _controller.GetAll();

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedOrganizers = Assert.IsAssignableFrom<IEnumerable<Organizer>>(okResult.Value);
            Assert.Equal(2, returnedOrganizers.Count());
        }

        [Fact]
        public void GetById_ExistingId_ReturnsOkResultWithOrganizer()
        {
            // Arrange  
            var organizerId = Guid.NewGuid();
            var organizer = new Organizer
            {
                Id = organizerId,
                Name = "John Doe",
                Email = "john@example.com",
                Phone = "123-456-7890"
            };
            _mockRepository.Setup(repo => repo.GetById(organizerId)).Returns(organizer);

            // Act  
            var result = _controller.GetById(organizerId);

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedOrganizer = Assert.IsType<Organizer>(okResult.Value);
            Assert.Equal(organizerId, returnedOrganizer.Id);
            Assert.Equal("John Doe", returnedOrganizer.Name);
        }

        [Fact]
        public void GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange  
            var organizerId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetById(organizerId)).Returns((Organizer)null);

            // Act  
            var result = _controller.GetById(organizerId);

            // Assert  
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Add_ValidOrganizerDto_ReturnsCreatedAtActionResult()
        {
            // Arrange  
            var organizerDto = new OrganizerDTO
            {
                Name = "New Organizer",
                Email = "new@example.com",
                Phone = "555-123-4567"
            };
            _mockRepository.Setup(repo => repo.Add(It.IsAny<Organizer>()));
            _mockRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _controller.Add(organizerDto);

            // Assert  
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetAll), createdResult.ActionName);
            _mockRepository.Verify(repo => repo.Add(It.IsAny<Organizer>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ExistingId_ReturnsNoContent()
        {
            // Arrange  
            var organizerId = Guid.NewGuid();
            var existingOrganizer = new Organizer
            {
                Id = organizerId,
                Name = "Original Name",
                Email = "original@example.com",
                Phone = "111-222-3333"
            };
            var organizerDto = new OrganizerDTO
            {
                Name = "Updated Name",
                Email = "updated@example.com",
                Phone = "444-555-6666"
            };

            _mockRepository.Setup(repo => repo.GetById(organizerId)).Returns(existingOrganizer);
            _mockRepository.Setup(repo => repo.Update(It.IsAny<Organizer>()));
            _mockRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _controller.Update(organizerId, organizerDto);

            // Assert  
            Assert.IsType<NoContentResult>(result);
            _mockRepository.Verify(repo => repo.GetById(organizerId), Times.Once);
            _mockRepository.Verify(repo => repo.Update(It.IsAny<Organizer>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_NonExistingId_CallsRepositoryButDoesNotUpdate()
        {
            // Arrange  
            var organizerId = Guid.NewGuid();
            var organizerDto = new OrganizerDTO
            {
                Name = "Updated Name",
                Email = "updated@example.com",
                Phone = "444-555-6666"
            };

            _mockRepository.Setup(repo => repo.GetById(organizerId)).Returns((Organizer)null);

            // Act  
            var result = _controller.Update(organizerId, organizerDto);

            // Assert  
            Assert.IsType<NoContentResult>(result);
            _mockRepository.Verify(repo => repo.GetById(organizerId), Times.Once);
            _mockRepository.Verify(repo => repo.Update(It.IsAny<Organizer>()), Times.Never);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Delete_ExistingId_ReturnsNoContent()
        {
            // Arrange  
            var organizerId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Delete(organizerId));
            _mockRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _controller.Delete(organizerId);

            // Assert  
            Assert.IsType<NoContentResult>(result);
            _mockRepository.Verify(repo => repo.Delete(organizerId), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }
    }
}