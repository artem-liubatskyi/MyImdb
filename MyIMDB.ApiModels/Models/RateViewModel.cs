namespace MyIMDB.ApiModels.Models
{
    public class RateViewModel
    {
        public long MovieId { get; set; }
        public long UserId { get; set; }
        public int Value { get; set; }
    }
}
