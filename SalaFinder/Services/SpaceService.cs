using SalaFinder.DAO;
using SalaFinder.Interfaces;
using SalaFinder.Models;



public class SpaceService : ISpaceService
{
    private readonly ApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public SpaceService(ApplicationDbContext context, IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<List<Space>> GetAll()
    {
        return await _context.Spaces.Where(s => s.isActive).ToListAsync();
    }

    public async Task<Space> GetById(Guid id)
    {
        return await _context.Spaces.FindAsync(id);
    }

    public async Task<Space> Create(Space newSpace)
    {
        _context.Spaces.Add(newSpace);
        await _context.SaveChangesAsync();
        return newSpace;
    }

    public async Task<Space> Update(Guid id, Space space)
    {
        var existing = await _context.Spaces.FindAsync(id);

        if (existing == null) return null;

        existing.name = space.name;
        existing.capacity = space.capacity;
        existing.type = space.type;

        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> ChangeStatus(Guid id, bool isActive)
    {
        var space = await _context.Spaces.FindAsync(id);

        if (space == null)
            return false;

        space.isActive = isActive;

        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<List<Space>> Filter(string type, int capacity, string building, string resource)
    {
        return await _context.Spaces.Where(s => s.type == type && s.capacity >= capacity && s.building == building && s.isActive && s.resources.Contains(resource)).ToListAsync();
    }
}