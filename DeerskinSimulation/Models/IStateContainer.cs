
namespace DeerskinSimulation.Models
{
    public interface IStateContainer
    {
        string? AvatarUrl { get; set; }
        bool? Debug { get; set; }

        event Action? OnChange;
    }
}