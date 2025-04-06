public class Booking
{
     public int Id { get; set; }
    public Guid Reference { get; set; } = Guid.NewGuid();
    public int MemberId { get; set; }
    public Member? Member { get; set; }
    public int InventoryItemId { get; set; }
    public InventoryItem? InventoryItem { get; set; }
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;
    public bool IsCanceled { get; set; }
}