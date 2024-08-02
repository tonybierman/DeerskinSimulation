using Xunit;
using Moq;
using System.Net.Http;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using DeerskinSimulation.Commands;

namespace DeerskinSimulation.Tests
{
    public class CommandFactoryTests
    {
        private readonly CommandFactory _factory;
        private readonly Mock<HttpClient> _httpClientMock;

        public CommandFactoryTests()
        {
            _factory = new CommandFactory();
            _httpClientMock = new Mock<HttpClient>();
        }

        [Fact]
        public void CreateGreatWagonRoadCommand_ShouldReturnCorrectCommand()
        {
            // Arrange
            var mockViewModel = new Mock<SimulationViewModel>(null, null, _httpClientMock.Object, null);
            var mockTrip = new Mock<Trip>(_httpClientMock.Object, "data/bethabara_to_charleston_trip.json");
            int numberOfSkins = 10;

            // Act
            var command = _factory.CreateGreatWagonRoadCommand(mockViewModel.Object, mockTrip.Object, numberOfSkins);

            // Assert
            Assert.NotNull(command);
            Assert.IsType<GreatWagonRoadCommand>(command);
        }

        [Fact]
        public void CreateDeliverToExporterCommand_ShouldReturnCorrectCommand()
        {
            // Arrange
            var mockViewModel = new Mock<SimulationViewModel>(null, null, _httpClientMock.Object, null);
            int numberOfSkins = 10;

            // Act
            var command = _factory.CreateDeliverToExporterCommand(mockViewModel.Object, numberOfSkins);

            // Assert
            Assert.NotNull(command);
            Assert.IsType<DeliverToExporterCommand>(command);
        }

        [Fact]
        public void CreateRandomTransportEventCheckCommand_ShouldReturnCorrectCommand()
        {
            // Arrange
            var mockViewModel = new Mock<SimulationViewModel>(null, null, _httpClientMock.Object, null);

            // Act
            var command = _factory.CreateRandomTransportEventCheckCommand(mockViewModel.Object);

            // Assert
            Assert.NotNull(command);
            Assert.IsType<RandomTransportEventCheckCommand>(command);
        }

        [Fact]
        public void CreateWildernessRoadCommand_ShouldReturnCorrectCommand()
        {
            // Arrange
            var mockViewModel = new Mock<SimulationViewModel>(null, null, _httpClientMock.Object, null);

            // Act
            var command = _factory.CreateWildernessRoadCommand(mockViewModel.Object);

            // Assert
            Assert.NotNull(command);
            Assert.IsType<WildernessRoadCommand>(command);
        }

        [Fact]
        public void CreateForwardToTraderCommand_ShouldReturnCorrectCommand()
        {
            // Arrange
            var mockViewModel = new Mock<SimulationViewModel>(null, null, _httpClientMock.Object, null);
            int numberOfSkins = 10;

            // Act
            var command = _factory.CreateForwardToTraderCommand(mockViewModel.Object, numberOfSkins);

            // Assert
            Assert.NotNull(command);
            Assert.IsType<ForwardToTraderCommand>(command);
        }

        [Fact]
        public void CreateRandomForwardingEventCheckCommand_ShouldReturnCorrectCommand()
        {
            // Arrange
            var mockViewModel = new Mock<SimulationViewModel>(null, null, _httpClientMock.Object, null);

            // Act
            var command = _factory.CreateRandomForwardingEventCheckCommand(mockViewModel.Object);

            // Assert
            Assert.NotNull(command);
            Assert.IsType<RandomForwardingEventCheckCommand>(command);
        }

        [Fact]
        public void CreateExportCommand_ShouldReturnCorrectCommand()
        {
            // Arrange
            var mockViewModel = new Mock<SimulationViewModel>(null, null, _httpClientMock.Object, null);
            int numberOfSkins = 10;

            // Act
            var command = _factory.CreateExportCommand(mockViewModel.Object, numberOfSkins);

            // Assert
            Assert.NotNull(command);
            Assert.IsType<ExportCommand>(command);
        }

        [Fact]
        public void CreateRandomExportEventCheckCommand_ShouldReturnCorrectCommand()
        {
            // Arrange
            var mockViewModel = new Mock<SimulationViewModel>(null, null, _httpClientMock.Object, null);

            // Act
            var command = _factory.CreateRandomExportEventCheckCommand(mockViewModel.Object);

            // Assert
            Assert.NotNull(command);
            Assert.IsType<RandomExportEventCheckCommand>(command);
        }

        [Fact]
        public void CreateHuntCommand_ShouldReturnCorrectCommand()
        {
            // Arrange
            var mockViewModel = new Mock<SimulationViewModel>(null, null, _httpClientMock.Object, null);

            // Act
            var command = _factory.CreateHuntCommand(mockViewModel.Object);

            // Assert
            Assert.NotNull(command);
            Assert.IsType<HuntCommand>(command);
        }

        [Fact]
        public void CreateRandomHuntingEventCheckCommand_ShouldReturnCorrectCommand()
        {
            // Arrange
            var mockViewModel = new Mock<SimulationViewModel>(null, null, _httpClientMock.Object, null);

            // Act
            var command = _factory.CreateRandomHuntingEventCheckCommand(mockViewModel.Object);

            // Assert
            Assert.NotNull(command);
            Assert.IsType<RandomHuntingEventCheckCommand>(command);
        }

        [Fact]
        public void CreateDeliverToCampCommand_ShouldReturnCorrectCommand()
        {
            // Arrange
            var mockViewModel = new Mock<SimulationViewModel>(null, null, _httpClientMock.Object, null);

            // Act
            var command = _factory.CreateDeliverToCampCommand(mockViewModel.Object);

            // Assert
            Assert.NotNull(command);
            Assert.IsType<DeliverToCampCommand>(command);
        }
    }
}
