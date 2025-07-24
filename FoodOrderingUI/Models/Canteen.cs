namespace FoodOrderingUI.Models
{
    public class Canteen
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string Location { get; set; } = string.Empty;
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public bool IsActive { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<MenuItem> MenuItems { get; set; } = new();

        public string FormattedOpeningHours => $"{OpeningTime:hh\\:mm} - {ClosingTime:hh\\:mm}";
        public bool IsCurrentlyOpen
        {
            get
            {
                var now = DateTime.Now.TimeOfDay;
                return now >= OpeningTime && now <= ClosingTime;
            }
        }
    }
}