using EventsManagementApp.Controllers;
using EventsManagementApp.Models;
using EventsManagementApp.Repositories;
using EventsManagementApp.Service;
using EventsManagementsApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EventsManagementApp.Tests.Controllers
{
    public class EventControllerTests
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly EventService _eventService;
        private readonly EventController _eventController;

        public EventControllerTests()
        {
            // Arrange - Setup mock repository  
            _mockEventRepository = new Mock<IEventRepository>();

            // Create EventService with mocked repository  
            _eventService = new EventService(_mockEventRepository.Object);

            // Create EventController with EventService  
            _eventController = new EventController(_eventService);
        }

        [Fact]
        public void Get_ReturnsAllEvents()
        {
            // Arrange  
            var expectedEvents = new List<Event>
            {
                new Event { Id = Guid.NewGuid(), Name = "Event 1", Description = "Description 1", Location = "Location 1", Date = DateTime.Now, MaxCapacity = 100, OrganizerId = Guid.NewGuid() },
                new Event { Id = Guid.NewGuid(), Name = "Event 2", Description = "Description 2", Location = "Location 2", Date = DateTime.Now, MaxCapacity = 200, OrganizerId = Guid.NewGuid() }
            };

            _mockEventRepository.Setup(repo => repo.GetAll()).Returns(expectedEvents);

            // Act  
            var result = _eventController.Get();

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockEventRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void Get_WithValidId_ReturnsEvent()
        {
            // Arrange  
            var eventId = Guid.NewGuid();
            var expectedEvent = new Event
            {
                Id = eventId,
                Name = "Test Event",
                Description = "Test Description",
                Location = "Test Location",
                Date = DateTime.Now,
                MaxCapacity = 150,
                OrganizerId = Guid.NewGuid()
            };

            _mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(expectedEvent);

            // Act  
            var result = _eventController.Get(eventId);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(expectedEvent, result.Value);
            _mockEventRepository.Verify(repo => repo.GetById(eventId), Times.Once);
        }

        [Fact]
        public void Get_WithInvalidId_ReturnsNull()
        {
            // Arrange  
            var eventId = Guid.NewGuid();
            _mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns((Event)null);

            // Act  
            var result = _eventController.Get(eventId);

            // Assert  
            Assert.Null(result.Value);
            _mockEventRepository.Verify(repo => repo.GetById(eventId), Times.Once);
        }

        [Fact]
        public void Post_WithValidEventDTO_ReturnsOkResult()
        {
            // Arrange  
            var eventDto = new EventDTO
            {
                Name = "New Event",
                Description = "New Description",
                Location = "New Location",
                Date = DateTime.Now.AddDays(30),
                MaxCapacity = 300,
                OrganizerId = Guid.NewGuid()
            };

            _mockEventRepository.Setup(repo => repo.Add(It.IsAny<Event>()));
            _mockEventRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _eventController.Post(eventDto);

            // Assert  
            Assert.IsType<OkResult>(result);
            _mockEventRepository.Verify(repo => repo.Add(It.Is<Event>(e =>
                e.Name == eventDto.Name &&
                e.Description == eventDto.Description &&
                e.Location == eventDto.Location &&
                e.MaxCapacity == eventDto.MaxCapacity &&
                e.OrganizerId == eventDto.OrganizerId)), Times.Once);
            _mockEventRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Put_WithValidIdAndEventDTO_ReturnsOkResult()
        {
            // Arrange  
            var eventId = Guid.NewGuid();
            var existingEvent = new Event
            {
                Id = eventId,
                Name = "Old Event",
                Description = "Old Description",
                Location = "Old Location",
                Date = DateTime.Now,
                MaxCapacity = 100,
                OrganizerId = Guid.NewGuid()
            };

            var eventDto = new EventDTO
            {
                Name = "Updated Event",
                Description = "Updated Description",
                Location = "Updated Location",
                Date = DateTime.Now.AddDays(60),
                MaxCapacity = 250,
                OrganizerId = Guid.NewGuid()
            };

            _mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(existingEvent);
            _mockEventRepository.Setup(repo => repo.Update(It.IsAny<Event>()));
            _mockEventRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _eventController.Put(eventId, eventDto);

            // Assert  
            Assert.IsType<OkResult>(result);
            _mockEventRepository.Verify(repo => repo.GetById(eventId), Times.Once);
            _mockEventRepository.Verify(repo => repo.Update(existingEvent), Times.Once);
            _mockEventRepository.Verify(repo => repo.SaveChanges(), Times.Once);

            // Verify the event was updated with new values  
            Assert.Equal(eventDto.Name, existingEvent.Name);
            Assert.Equal(eventDto.Description, existingEvent.Description);
            Assert.Equal(eventDto.Location, existingEvent.Location);
            Assert.Equal(eventDto.MaxCapacity, existingEvent.MaxCapacity);
            Assert.Equal(eventDto.OrganizerId, existingEvent.OrganizerId);
        }

        [Fact]
        public void Put_WithInvalidId_DoesNotUpdateAndReturnsOk()
        {
            // Arrange  
            var eventId = Guid.NewGuid();
            var eventDto = new EventDTO
            {
                Name = "Updated Event",
                Description = "Updated Description",
                Location = "Updated Location",
                Date = DateTime.Now.AddDays(60),
                MaxCapacity = 250,
                OrganizerId = Guid.NewGuid()
            };

            _mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns((Event)null);

            // Act  
            var result = _eventController.Put(eventId, eventDto);

            // Assert  
            Assert.IsType<OkResult>(result);
            _mockEventRepository.Verify(repo => repo.GetById(eventId), Times.Once);
            _mockEventRepository.Verify(repo => repo.Update(It.IsAny<Event>()), Times.Never);
            _mockEventRepository.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        public void Delete_WithValidId_ReturnsOkResult()
        {
            // Arrange  
            var eventId = Guid.NewGuid();
            _mockEventRepository.Setup(repo => repo.Delete(eventId));
            _mockEventRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _eventController.Delete(eventId);

            // Assert  
            Assert.IsType<OkResult>(result);
            _mockEventRepository.Verify(repo => repo.Delete(eventId), Times.Once);
            _mockEventRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Delete_WithInvalidId_StillReturnsOkResult()
        {
            // Arrange  
            var eventId = Guid.NewGuid();
            _mockEventRepository.Setup(repo => repo.Delete(eventId));
            _mockEventRepository.Setup(repo => repo.SaveChanges());

            // Act  
            var result = _eventController.Delete(eventId);

            // Assert  
            Assert.IsType<OkResult>(result);
            _mockEventRepository.Verify(repo => repo.Delete(eventId), Times.Once);
            _mockEventRepository.Verify(repo => repo.SaveChanges(), Times.Once);
        }
    }
}