
using Microsoft.EntityFrameworkCore;
using SalaFinder.DAO;
using SalaFinder.Interfaces;
using SalaFinder.Models;

public class NoShowService : INoShowService
{
    private readonly ApplicationDbContext _context;

    public NoShowService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserBlocked(string userId)
    {
        var record = await _context.NoShows.FirstOrDefaultAsync(n => n.userId == userId);

        if (record == null)
            return false;

        return record.blockedUntil > DateTime.UtcNow;
    }

    public async Task<NoShow> RegisterNoShow(string userId)
    {
        var record = await _context.NoShows.FirstOrDefaultAsync(n => n.userId == userId);

        if (record == null)
        {
            record = new NoShow
            {
                userId = userId,
                count = 1
            };

            _context.NoShows.Add(record);
        }
        else
        {
            record.count++;

            if (record.count >= 2)
            {
                record.blockedUntil = DateTime.UtcNow.AddDays(7);
            }
        }

        await _context.SaveChangesAsync();

        return record;
    }
    public async Task ResetNoShows(string userId)
    {
        var record = await _context.NoShows.FirstOrDefaultAsync(n => n.userId == userId);

        if (record != null)
        {
            record.count = 0;
            record.blockedUntil = DateTime.MinValue;

            await _context.SaveChangesAsync();
        }
    }
}