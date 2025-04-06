public class InventoryItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? TotalCount { get; set; }
    public int RemainingCount { get; set; }
    public DateTime ExpirationDate { get; set; } = DateTime.UtcNow;
    public ICollection<Booking>? Bookings { get; set; }
}