namespace DeerskinSimulation.Models
{
    public class EventRecord
    {
        public string Message { get; set; }
        public string Color { get; set; }

        public EventRecord(string message, string color = "black")
        {
            Message = message;
            Color = color;
        }

        public static EventRecord Empty { get; } = new EventRecord(string.Empty);

        public static bool IsNullOrEmpty(EventRecord record)
        {
            return record == null || string.IsNullOrEmpty(record.Message);
        }
    }

}
