namespace Assessment3.Models
{
    public class Location
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int? Number { get; set; }
        public string Street { get; set; } = string.Empty;
        public string Suburb { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
