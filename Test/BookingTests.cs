using Xunit;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

public class BookingTests
{
    private readonly IMapper _mapper;
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public BookingTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Booking, BookingResponse>();
        });
        _mapper = mapperConfig.CreateMapper();

        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Fresh DB each test
            .Options;
    }

    [Fact]
    public async Task BookItemAsync_ShouldCreateBooking_WhenValidRequest()
    {      

        using var context = new AppDbContext(_dbContextOptions);

        context.Members.Add(new Member { Id = 1, Name = "Sophie Davis", BookingCount = 1,DateJoined =DateTime.Now});
       context.InventoryItems.Add(new InventoryItem { Id = 1, Title = "Bali",
           Description="Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus tellus aliquam id. Maecenas non volutpat nisl. Curabitur vestibulum ante non nibh faucibus, sit amet pulvinar turpis finibus", 
        TotalCount = 5, RemainingCount = 5 ,ExpirationDate=DateTime.Now});
        context.Bookings.Add(new Booking { Id = 1, MemberId = 1, InventoryItemId = 1,BookedAt =DateTime.Now});
        context.SaveChanges();

        
        var service = new BookingService(context, _mapper);

        var member = new Member { Id = 1, Bookings = new List<Booking>() };
        var item = new InventoryItem { Id = 1, RemainingCount = 5 };

        context.Members.Add(member);
        context.InventoryItems.Add(item);
        await context.SaveChangesAsync();

        var request = new BookingRequest
        {
            MemberId = 1,
            InventoryItemId = 1
        };

        var result = await service.BookItemAsync(request);

        Assert.NotNull(result);
        Assert.Equal(1, result.BookingId);
        Assert.Single(context.Bookings);
        Assert.Equal(4, item.RemainingCount);
    }

    [Fact]
    public async Task BookItemAsync_ShouldThrow_WhenNoInventory()
    {
        using var context = new AppDbContext(_dbContextOptions);

        context.Members.Add(new Member { Id = 1, Name = "Sophie Davis", BookingCount = 1,DateJoined =DateTime.Now});
       context.InventoryItems.Add(new InventoryItem { Id = 1, Title = "Bali",
           Description="Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus tellus aliquam id. Maecenas non volutpat nisl. Curabitur vestibulum ante non nibh faucibus, sit amet pulvinar turpis finibus", 
        TotalCount = 5, RemainingCount = 5 ,ExpirationDate=DateTime.Now});
        context.Bookings.Add(new Booking { Id = 1, MemberId = 1, InventoryItemId = 1,BookedAt =DateTime.Now});
        context.SaveChanges();
        var service = new BookingService(context, _mapper);

        var member = new Member { Id = 1, Bookings = new List<Booking>() };
        var item = new InventoryItem { Id = 1, RemainingCount = 0 };

        context.Members.Add(member);
        context.InventoryItems.Add(item);
        await context.SaveChangesAsync();

        var request = new BookingRequest
        {
            MemberId = 1,
            InventoryItemId = 1
        };

        await Assert.ThrowsAsync<Exception>(() => service.BookItemAsync(request));
    }

    [Fact]
    public async Task CancelBookingAsync_ShouldRemoveBooking_WhenValidId()
    {
        using var context = new AppDbContext(_dbContextOptions);

        context.Members.Add(new Member { Id = 1, Name = "Sophie Davis", BookingCount = 1,DateJoined =DateTime.Now});
       context.InventoryItems.Add(new InventoryItem { Id = 1, Title = "Bali",
           Description="Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus tellus aliquam id. Maecenas non volutpat nisl. Curabitur vestibulum ante non nibh faucibus, sit amet pulvinar turpis finibus", 
        TotalCount = 5, RemainingCount = 5 ,ExpirationDate=DateTime.Now});
        context.Bookings.Add(new Booking { Id = 1, MemberId = 1, InventoryItemId = 1,BookedAt =DateTime.Now});
        context.SaveChanges();
        var service = new BookingService(context, _mapper);

        var booking = new Booking { Id = 1, InventoryItemId = 2, MemberId = 1 };
        var item = new InventoryItem { Id = 2, RemainingCount = 3 };

        context.Bookings.Add(booking);
        context.InventoryItems.Add(item);
        await context.SaveChangesAsync();

        var result = await service.CancelBookingAsync(1);

        Assert.True(result);
        Assert.Empty(context.Bookings);
        Assert.Equal(4, item.RemainingCount);
    }

    [Fact]
    public async Task CancelBookingAsync_ShouldReturnFalse_WhenBookingNotFound()
    {
        using var context = new AppDbContext(_dbContextOptions);

         context.Members.Add(new Member { Id = 1, Name = "Sophie Davis", BookingCount = 1,DateJoined =DateTime.Now});
       context.InventoryItems.Add(new InventoryItem { Id = 1, Title = "Bali",
           Description="Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus tellus aliquam id. Maecenas non volutpat nisl. Curabitur vestibulum ante non nibh faucibus, sit amet pulvinar turpis finibus", 
        TotalCount = 5, RemainingCount = 5 ,ExpirationDate=DateTime.Now});
        context.Bookings.Add(new Booking { Id = 1, MemberId = 1, InventoryItemId = 1,BookedAt =DateTime.Now});
        context.SaveChanges();
        var service = new BookingService(context, _mapper);

        var result = await service.CancelBookingAsync(999); // Non-existent ID

        Assert.False(result);
    }
}
