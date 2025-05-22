using EventsManagementApp.Controllers;
using EventsManagementApp.DTOs;
using EventsManagementApp.Models;
using EventsManagementApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace EventsManagementApp.Tests.Controllers
{
    public class SponsorControllerTests
    {
        private readonly Mock<SponsorService> _mockService;
        private readonly SponsorController _controller;

        public SponsorControllerTests()
        {
            _mockService = new Mock<SponsorService>();
            _controller = new SponsorController(_mockService.Object);
        }

        [Fact]
        public void GetAll_ReturnsOkWithSponsors()
        {
            // Arrange  
            var sponsors = new List<Sponsor>
            {
                new Sponsor { Id = Guid.NewGuid(), Name = "Sponsor 1" },
                new Sponsor { Id = Guid.NewGuid(), Name = "Sponsor 2" }
            };
            _mockService.Setup(s => s.GetAll()).Returns(sponsors);

            // Act  
            var result = _controller.GetAll();

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(sponsors, okResult.Value);
            _mockService.Verify(s => s.GetAll(), Times.Once);
        }

        [Fact]
        public void GetById_ExistingId_ReturnsOkWithSponsor()
        {
            // Arrange  
            var sponsorId = Guid.NewGuid();
            var sponsor = new Sponsor { Id = sponsorId, Name = "Test Sponsor" };
            _mockService.Setup(s => s.GetById(sponsorId)).Returns(sponsor);

            // Act  
            var result = _controller.GetById(sponsorId);

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(sponsor, okResult.Value);
            _mockService.Verify(s => s.GetById(sponsorId), Times.Once);
        }

        [Fact]
        public void GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange  
            var sponsorId = Guid.NewGuid();
            _mockService.Setup(s => s.GetById(sponsorId)).Returns((Sponsor)null);

            // Act  
            var result = _controller.GetById(sponsorId);

            // Assert  
            Assert.IsType<NotFoundResult>(result.Result);
            _mockService.Verify(s => s.GetById(sponsorId), Times.Once);
        }

        [Fact]
        public void Add_ValidSponsor_ReturnsCreatedAtAction()
        {
            // Arrange  
            var sponsorDto = new SponsorDTO { Name = "New Sponsor" };

            // Act  
            var result = _controller.Add(sponsorDto);

            // Assert  
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetAll), createdResult.ActionName);
            _mockService.Verify(s => s.Add(sponsorDto), Times.Once);
        }

        [Fact]
        public void Update_ValidSponsor_ReturnsNoContent()
        {
            // Arrange  
            var sponsorId = Guid.NewGuid();
            var sponsorDto = new SponsorDTO { Name = "Updated Sponsor" };

            // Act  
            var result = _controller.Update(sponsorId, sponsorDto);

            // Assert  
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.Update(sponsorId, sponsorDto), Times.Once);
        }

        [Fact]
        public void Delete_ValidId_ReturnsNoContent()
        {
            // Arrange  
            var sponsorId = Guid.NewGuid();

            // Act  
            var result = _controller.Delete(sponsorId);

            // Assert  
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.Delete(sponsorId), Times.Once);
        }
    }
}