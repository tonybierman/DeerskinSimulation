using System.ComponentModel.DataAnnotations;

namespace DeerskinSimulation.ViewModels
{
    public class SellOptionsViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "The number of skins must be at least 1.")]
        public int NumberOfSkins { get; set; } = 1;
    }
}
