public class Member
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateJoined { get; set; } = DateTime.UtcNow;
    public int? BookingCount { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}