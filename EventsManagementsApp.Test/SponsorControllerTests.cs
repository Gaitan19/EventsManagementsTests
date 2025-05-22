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
    public class SponsorControllerTests
    {
        private readonly Mock<ISponsorRepository> _mockRepository;
        private readonly SponsorService _sponsorService;
        private readonly SponsorController _controller;

        public SponsorControllerTests()
        {
            _mockRepository = new Mock<ISponsorRepository>();
            _sponsorService = new SponsorService(_mockRepository.Object);
            _controller = new SponsorController(_sponsorService);
        }

        [Fact]
        public void GetAll_ReturnsOkResultWithSponsors()
        {
            // Arrange  
            var sponsors = new List<Sponsor>
            {
                new Sponsor { Id = Guid.NewGuid(), Name = "Sponsor 1", Description = "Description 1", EventId = Guid.NewGuid() },
                new Sponsor { Id = Guid.NewGuid(), Name = "Sponsor 2", Description = "Description 2", EventId = Guid.NewGuid() }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(sponsors);

            // Act  
            var result = _controller.GetAll();

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedSponsors = Assert.IsAssignableFrom<IEnumerable<Sponsor>>(okResult.Value);
            Assert.Equal(2, returnedSponsors.Count());
        }

        [Fact]
        public void GetById_ExistingId_ReturnsOkResultWithSponsor()
        {
            // Arrange  
            var sponsorId = Guid.NewGuid();
            var sponsor = new Sponsor
            {
                Id = sponsorId,
                Name = "Test Sponsor",
                Description = "Test Description",
                EventId = Guid.NewGuid()
            };
            _mockRepository.Setup(repo => repo.GetById(sponsorId)).Returns(sponsor);

            // Act  
            var result = _controller.GetById(sponsorId);

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedSponsor = Assert.IsType<Sponsor>(okResult.Value);
            Assert.Equal(sponsorId, returnedSponsor.Id);
            Assert.Equal("Test Sponsor", returnedSponsor.Name);
        }

        [Fact]
        public void GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange  
            var sponsorId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetById(sponsorId)).Returns((Sponsor)null);

            // Act  
            var result = _controller.GetById(sponsorId);

            // Assert  
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Add_ValidSponsorDto_ReturnsCreatedAtActionResult()
        {
            // Arrange  
            var sponsorDto = new SponsorDTO
            {
                Name = "New Sponsor",
                Description = "New Description",
                EventId = Guid.NewGuid()
            };
            _mockRepository.Setup(repo => repo.Add(It.IsAny<Sponsor>()));
            _mockRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _controller.Add(sponsorDto);

            // Assert  
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetAll), createdResult.ActionName);
            _mockRepository.Verify(repo => repo.Add(It.IsAny<Sponsor>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ExistingId_ReturnsNoContent()
        {
            // Arrange  
            var sponsorId = Guid.NewGuid();
            var existingSponsor = new Sponsor
            {
                Id = sponsorId,
                Name = "Original Name",
                Description = "Original Description",
                EventId = Guid.NewGuid()
            };
            var sponsorDto = new SponsorDTO
            {
                Name = "Updated Name",
                Description = "Updated Description",
                EventId = Guid.NewGuid()
            };

            _mockRepository.Setup(repo => repo.GetById(sponsorId)).Returns(existingSponsor);
            _mockRepository.Setup(repo => repo.Update(It.IsAny<Sponsor>()));
            _mockRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _controller.Update(sponsorId, sponsorDto);

            // Assert  
            Assert.IsType<NoContentResult>(result);
            _mockRepository.Verify(repo => repo.GetById(sponsorId), Times.Once);
            _mockRepository.Verify(repo => repo.Update(It.IsAny<Sponsor>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_NonExistingId_CallsRepositoryButDoesNotUpdate()
        {
            // Arrange  
            var sponsorId = Guid.NewGuid();
            var sponsorDto = new SponsorDTO
            {
                Name = "Updated Name",
                Description = "Updated Description",
                EventId = Guid.NewGuid()
            };

            _mockRepository.Setup(repo => repo.GetById(sponsorId)).Returns((Sponsor)null);

            // Act  
            var result = _controller.Update(sponsorId, sponsorDto);

            // Assert  
            Assert.IsType<NoContentResult>(result);
            _mockRepository.Verify(repo => repo.GetById(sponsorId), Times.Once);
            _mockRepository.Verify(repo => repo.Update(It.IsAny<Sponsor>()), Times.Never);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Delete_ExistingId_ReturnsNoContent()
        {
            // Arrange  
            var sponsorId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Delete(sponsorId));
            _mockRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _controller.Delete(sponsorId);

            // Assert  
            Assert.IsType<NoContentResult>(result);
            _mockRepository.Verify(repo => repo.Delete(sponsorId), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }
    }
}