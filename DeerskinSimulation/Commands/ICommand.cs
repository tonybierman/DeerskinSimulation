using DeerskinSimulation.Models;

namespace DeerskinSimulation.Commands
{
    public interface ICommand
    {
        event EventHandler? CanExecuteChanged;

        bool CanExecute(object? parameter);
        EventResultStatus Execute(object? parameter);
        Task<EventResultStatus> ExecuteAsync();
    }
}