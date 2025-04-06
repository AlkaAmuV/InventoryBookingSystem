
using Microsoft.AspNetCore.Mvc;


namespace InventoryBookingSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("book")]
    public async Task<IActionResult> Book([FromBody] BookingRequest request)
    {
        try
        {
            var result = await _bookingService.BookItemAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("cancel/{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _bookingService.CancelBookingAsync(id);
        return result ? Ok("Booking cancelled") : NotFound();
    }
}