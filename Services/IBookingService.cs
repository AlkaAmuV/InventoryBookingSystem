public interface IBookingService
{
    Task<BookingResponse> BookItemAsync(BookingRequest request);
    Task<bool> CancelBookingAsync(int bookingId);
}