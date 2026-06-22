using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.Product;

public class ProductService : BaseService
{
    public ProductService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IServiceResult<List<ProductReadDto>>> GetAllAsync()
    {
        var products = await Context.Products.AsNoTracking().ToListAsync();
        var dtos = Mapper.Map<List<ProductReadDto>>(products);
        return new DataServiceResult<List<ProductReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ProductReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<ProductReadDto>(OperationStatus.InvalidInput, false);

        var product = await Context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            return new DataServiceResult<ProductReadDto>(OperationStatus.NotFound, false);

        var dto = Mapper.Map<ProductReadDto>(product);
        return new DataServiceResult<ProductReadDto>(true, dto);
    }

    public async Task<IServiceResult<ProductReadDto>> CreateAsync(ProductCreateDto createDto)
    {
        if (createDto == null)
            return new DataServiceResult<ProductReadDto>(OperationStatus.InvalidInput, false);

        var product = Mapper.Map<Models.Product>(createDto);
        product.Active = true;

        await Context.Products.AddAsync(product);
        await Context.SaveChangesAsync();

        var dto = Mapper.Map<ProductReadDto>(product);
        return new DataServiceResult<ProductReadDto>(OperationStatus.Created, true, dto);
    }

    public async Task<IServiceResult<ProductReadDto>> UpdateAsync(ProductUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<ProductReadDto>(OperationStatus.InvalidInput, false);

        var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

        if (product == null)
            return new DataServiceResult<ProductReadDto>(OperationStatus.NotFound, false);

        Mapper.Map(updateDto, product);
        Context.Products.Update(product);
        await Context.SaveChangesAsync();

        var dto = Mapper.Map<ProductReadDto>(product);
        return new DataServiceResult<ProductReadDto>(OperationStatus.Updated, true, dto);
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        Context.Products.Remove(product);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
    }
}
