using DeerskinSimulation.Models;

namespace DeerskinSimulation.Commands
{
    public interface ICommand
    {
        event EventHandler? CanExecuteChanged;

        bool CanExecute(object? parameter);
        void Execute(object? parameter);
        Task<EventResultStatus> ExecuteAsync();
    }
}