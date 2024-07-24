namespace DeerskinSimulation.ViewModels
{
    public class MessageConsoleViewModel
    {
        public string ImageUrl { get; set; }
        public string ImageCaption { get; set; }

        public MessageConsoleViewModel(string imageUrl = "images/ph_256.jpg", string caption = "Deerskin Simulation") 
        {
            ImageUrl = imageUrl;
            ImageCaption = caption;
        }
    }
}
