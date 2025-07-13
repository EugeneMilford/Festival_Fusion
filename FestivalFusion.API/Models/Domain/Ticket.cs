namespace FestivalFusion.API.Modals.Domain
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public string TicketType { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
