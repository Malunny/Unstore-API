using System.Linq;
using Unstore.DTOs.Mapping;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.Avaliation;

public class ProductAvaliationService : BaseService
{
    public ProductAvaliationService(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IServiceResult<List<ProductAvaliationReadDto>>> GetAllAsync()
    {
        var avaliations = await Context.ProductAvaliations.AsNoTracking().ToListAsync();
        var dtos = avaliations.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<ProductAvaliationReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ProductAvaliationReadDto>> GetByIdAsync(int userId, int productId)
    {
        if (userId <= 0 || productId <= 0)
            return new DataServiceResult<ProductAvaliationReadDto>(OperationStatus.InvalidInput, false);

        var avaliation = await Context.ProductAvaliations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

        if (avaliation == null)
            return new DataServiceResult<ProductAvaliationReadDto>(OperationStatus.NotFound, false);

        var dto = avaliation.MapToDto();
        return new DataServiceResult<ProductAvaliationReadDto>(true, dto);
    }

    public async Task<IServiceResult<List<ProductAvaliationReadDto>>> GetByProductIdAsync(int productId)
    {
        if (productId <= 0)
            return new DataServiceResult<List<ProductAvaliationReadDto>>(OperationStatus.InvalidInput, false);

        var avaliations = await Context.ProductAvaliations
            .AsNoTracking()
            .Where(x => x.ProductId == productId)
            .ToListAsync();

        var dtos = avaliations.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<ProductAvaliationReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ProductAvaliationReadDto>> CreateAsync(ProductAvaliationCreateDto createDto)
    {
        if (createDto == null || createDto.UserId <= 0 || createDto.ProductId <= 0)
            return new DataServiceResult<ProductAvaliationReadDto>(OperationStatus.InvalidInput, false);

        var userExists = await Context.Users.AnyAsync(x => x.Id == createDto.UserId);
        if (!userExists)
            return new DataServiceResult<ProductAvaliationReadDto>(OperationStatus.NotFound, false);

        var productExists = await Context.Products.AnyAsync(x => x.Id == createDto.ProductId);
        if (!productExists)
            return new DataServiceResult<ProductAvaliationReadDto>(OperationStatus.NotFound, false);

        var avaliation = createDto.MapToModel();

        await Context.ProductAvaliations.AddAsync(avaliation);
        await Context.SaveChangesAsync();

        var dto = avaliation.MapToDto();
        return new DataServiceResult<ProductAvaliationReadDto>(OperationStatus.Created, true, dto);
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int userId, int productId)
    {
        if (userId <= 0 || productId <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var avaliation = await Context.ProductAvaliations
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

        if (avaliation == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        Context.ProductAvaliations.Remove(avaliation);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
    }
}
