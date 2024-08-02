using DeerskinSimulation.Commands;
using DeerskinSimulation.Models;

namespace DeerskinSimulation.ViewModels
{
    public interface ISimulationViewModel
    {
        // Declare necessary properties and methods for the interface
        RoleHunter Hunter { get; }
        RoleTrader Trader { get; }
        RoleExporter Exporter { get; }
        int SelectedPackhorses { get; set; }
        int GameDay { get; }
        IReadOnlyList<EventResult> Messages { get; }
        UserInitiatedActivitySequence CurrentUserActivity { get; set; }
        Task UpdateUserActivityDay();
        void AddMessage(EventResult message);
        void ClearMessages();
        Story Featured { get; }
        void SetFeatured(EventRecord? result);
        event Func<Task> StateChanged;
        ForwardingSequenceCoordinator EnsureSellCmd { get; }
        HuntingSequentCoordinator EnsureHuntCmd { get; }
        TransportingSequenceCoordinator EnsureTransportCmd { get; }
        ExportSequenceCoordinator EnsureExportCmd { get; }
    }
}
