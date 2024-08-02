using System.Reflection.Metadata.Ecma335;

namespace DeerskinSimulation.Models
{
    public class EventRecord
    {
        private string _image;
        public string? Title { get; set; }
        public string? Message { get; init; }
        public string Color { get; init; }
        public string Image { get; init; }

        public EventRecord(string name, int gameDay, string message, string color = "black", string image = "images/ph_256.jpg") 
            : this($"{DateUtils.GameDate(gameDay).ToString("d")}: {message}", color, image) { }

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
