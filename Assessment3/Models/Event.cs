namespace Assessment3.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalSeats { get; set; }
        public int SeatsTaken { get; set; }
        public DateTime Date { get; set; }
        public Location Location { get; set; } = new();
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
