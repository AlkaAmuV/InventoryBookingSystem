using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace InventoryBookingSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly AppDbContext _context;

    public UploadController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("members")]
    public async Task<IActionResult> UploadMembers(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            var values = line?.Split(',');
            if (values?.Length >= 2)
            {
            _context.Members.Add(new Member { Name = values[0],BookingCount = int.Parse(values[1]), DateJoined = DateTime.Parse(values[2])});
            }
        }
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("inventory")]
    public async Task<IActionResult> UploadInventory(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            var parts = line?.Split(',');
            if (parts?.Length >= 3)
            {                
                _context.InventoryItems.Add(new InventoryItem {
                    // Id = int.Parse(parts[0]),
                    Title = parts[0],
                    Description = parts[1],
                    // TotalCount = total,
                    RemainingCount = int.Parse(parts[2]),
                    ExpirationDate = DateTime.Parse(parts[3])
                });
            }
        }
        await _context.SaveChangesAsync();
        return Ok("Inventory uploaded");
    }

    [HttpGet("AllMembers")]

    public IActionResult getAllMembers()
    {
        var result = _context.Members.AsEnumerable();
        return Ok(result);
    }


[HttpGet("AllInventory")]
    public IActionResult getAllInventory()
    {
        var result = _context.InventoryItems.AsEnumerable();
        return Ok(result);
    }

    [HttpGet("AllBookings")]
    public IActionResult getAllBookings()
    {
        var result = _context.Bookings.AsEnumerable();
        return Ok(result);
    }
}


