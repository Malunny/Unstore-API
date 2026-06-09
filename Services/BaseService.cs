using AutoMapper;
using Unstore.Data;

namespace Unstore.Services;

public abstract class BaseService
{
    public readonly AppDbContext Context;
    public readonly IMapper Mapper;
    public BaseService(AppDbContext dbContext, IMapper mapper)
    {
        Context = dbContext;
        Mapper = mapper;
    }
}