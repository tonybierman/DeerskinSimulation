namespace DeerskinSimulation.Models
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class Trip
    {
        private readonly string _jsonFilePath;
        private readonly HttpClient _httpClient;

        public Trip() { }

        public List<TripDay> Days { get; private set; } = new List<TripDay>();

        /// <summary>
        /// Load a trip file e.g. "data/bethabara_to_charleston_trip.json"
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="jsonFilePath"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Trip(HttpClient httpClient, string jsonFilePath)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _jsonFilePath = jsonFilePath ?? throw new ArgumentNullException(nameof(jsonFilePath));
        }

        public async Task InitAsync()
        {
            try
            {
                var json = await _httpClient.GetStringAsync(_jsonFilePath);
                Days = JsonSerializer.Deserialize<List<TripDay>>(json) ?? new List<TripDay>();
            }
            catch (Exception ex)
            {
                // TODO: Error handling
                throw;
            }
        }

        public TripDay GetDay(int dayNumber)
        {
            return Days.Find(day => day.Day == dayNumber);
        }
    }
}
