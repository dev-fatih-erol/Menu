namespace Menu.Api.Models
{
    public class VenueDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string VenueType { get; set; }
    }
}