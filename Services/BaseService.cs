using Unstore.Data;

namespace Unstore.Services;

public abstract class BaseService
{
    protected readonly AppDbContext Context;
    public BaseService(AppDbContext dbContext)
    {
        Context = dbContext;
    }
}