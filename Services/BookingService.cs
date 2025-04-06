using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    private const int MAX_BOOKINGS = 2;

    public BookingService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BookingResponse> BookItemAsync(BookingRequest request)
    {
         var member = await _context.Members.Include(m => m.Bookings)
            .FirstOrDefaultAsync(m => m.Id == request.MemberId);
        var item = await _context.InventoryItems.FindAsync(request.InventoryItemId);

        if (member == null || item == null)
            throw new Exception("Invalid member or inventory item");

        if (member.BookingCount >= MAX_BOOKINGS)
            throw new Exception("Member has reached maximum bookings");

        if (item.RemainingCount <= 0)
            throw new Exception("No remaining inventory");

        var booking = new Booking
        {
            MemberId = request.MemberId,
            InventoryItemId = item.Id,
            BookedAt = DateTime.UtcNow
        };

        _context.Bookings.Add(booking);
        item.RemainingCount--;
        await _context.SaveChangesAsync();

        return _mapper.Map<BookingResponse>(booking);
    }

    public async Task<bool> CancelBookingAsync(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) return false;

        var item = await _context.InventoryItems.FindAsync(booking.InventoryItemId);
        _context.Bookings.Remove(booking);
        if (item != null) item.RemainingCount++;

        await _context.SaveChangesAsync();
        return true;
    }
}