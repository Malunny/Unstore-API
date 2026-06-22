using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.Category;

public class ProductCategoryService : BaseService
{
    public ProductCategoryService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IServiceResult<List<ProductCategoryReadDto>>> GetAllAsync()
    {
        var categories = await Context.ProductCategories.AsNoTracking().ToListAsync();
        var dtos = Mapper.Map<List<ProductCategoryReadDto>>(categories);
        return new DataServiceResult<List<ProductCategoryReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<ProductCategoryReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<ProductCategoryReadDto>(OperationStatus.InvalidInput, false);

        var category = await Context.ProductCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            return new DataServiceResult<ProductCategoryReadDto>(OperationStatus.NotFound, false);

        var dto = Mapper.Map<ProductCategoryReadDto>(category);
        return new DataServiceResult<ProductCategoryReadDto>(true, dto);
    }

    public async Task<IServiceResult<ProductCategoryReadDto>> CreateAsync(ProductCategoryCreateDto createDto)
    {
        if (createDto == null)
            return new DataServiceResult<ProductCategoryReadDto>(OperationStatus.InvalidInput, false);

        var keyExists = await Context.ProductCategories.AnyAsync(x => x.Key == createDto.Key);
        if (keyExists)
            return new DataServiceResult<ProductCategoryReadDto>(OperationStatus.ValidationError, false);

        var category = Mapper.Map<ProductCategory>(createDto);

        await Context.ProductCategories.AddAsync(category);
        await Context.SaveChangesAsync();

        var dto = Mapper.Map<ProductCategoryReadDto>(category);
        return new DataServiceResult<ProductCategoryReadDto>(OperationStatus.Created, true, dto);
    }

    public async Task<IServiceResult<ProductCategoryReadDto>> UpdateAsync(ProductCategoryUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<ProductCategoryReadDto>(OperationStatus.InvalidInput, false);

        var category = await Context.ProductCategories.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

        if (category == null)
            return new DataServiceResult<ProductCategoryReadDto>(OperationStatus.NotFound, false);

        if (updateDto.Key != null && updateDto.Key != category.Key)
        {
            var keyExists = await Context.ProductCategories.AnyAsync(x => x.Key == updateDto.Key && x.Id != updateDto.Id);
            if (keyExists)
                return new DataServiceResult<ProductCategoryReadDto>(OperationStatus.ValidationError, false);
        }

        Mapper.Map(updateDto, category);
        Context.ProductCategories.Update(category);
        await Context.SaveChangesAsync();

        var dto = Mapper.Map<ProductCategoryReadDto>(category);
        return new DataServiceResult<ProductCategoryReadDto>(OperationStatus.Updated, true, dto);
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var category = await Context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        Context.ProductCategories.Remove(category);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
    }
}
