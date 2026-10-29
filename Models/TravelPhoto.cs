    namespace GeographyGame.Backend.Models;

    public class TravelPhoto
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Title { get; set; }
        public string? Details { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? EditDate { get; set; }
    }