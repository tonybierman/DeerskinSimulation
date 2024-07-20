using DeerskinSimulation.Models;
using System.ComponentModel.DataAnnotations;

namespace DeerskinSimulation.ViewModels
{
    public class HuntOptionsViewModel
    {
        [Range(1, Constants.MaxPackhorses, ErrorMessage = "Packhorses must be between 1 and {1}.")]
        public int SelectedPackhorses { get; set; } = 1;
    }
}
