using System.Reflection.Metadata.Ecma335;

namespace DeerskinSimulation.Models
{
    public class EventRecord
    {
        private string _image;
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }

        public EventRecord(string name, int day, string message, string color = "black", string image = "images/ph_256.jpg") 
            : this($"{name} day {day}: {message}", color, image) { }

        public EventRecord(string? message, string color = "black", string image = "images/ph_256.jpg")
        {
            Message = message;
            Color = color;
            Image = image;
        }

        public static bool IsNullOrEmpty(EventRecord record)
        {
            return record == null || string.IsNullOrEmpty(record.Message);
        }
    }

}
