using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Unstore.Data;
using Unstore.DTO;
using Unstore.Extensions;
using Unstore.Models;

namespace Unstore.Services
{
    public class ProductService
    {
        private readonly IMapper mapper;
        private readonly AppDbContext context;

        public ProductService(IMapper map, AppDbContext context)
        {
            mapper = map;
            this.context = context;
        }
        public async Task<ServiceResult<ProductCreateDto>> CreateAsync(ModelStateDictionary modelstate, ProductCreateDto productCreateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ProductCreateDto>.Failure(modelstate.GetErrors());

            var productMapped = mapper.Map<ProductCreateDto,Product>(productCreateDto);

            await context.Products.AddAsync(productMapped);
            await context.SaveChangesAsync();

            return ServiceResult<ProductCreateDto>.Success(productCreateDto);
        }

        public async Task<ServiceResult<IEnumerable<ProductCreateDto>>> CreateRangeAsync(ModelStateDictionary modelstate, IEnumerable<ProductCreateDto> productCreateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<ProductCreateDto>>.Failure(modelstate.GetErrors());

            var productMapped = mapper.Map<IEnumerable<ProductCreateDto>,IEnumerable<Product>>(productCreateDtos);

            await context.Products.AddRangeAsync(productMapped);
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<ProductCreateDto>>.Success(productCreateDtos);
        }

        public async Task<ServiceResult<ProductUpdateDto>> UpdateAsync(ModelStateDictionary modelstate, ProductUpdateDto productUpdateDto)
        {
            if (!modelstate.IsValid)
                return ServiceResult<ProductUpdateDto>.Failure(modelstate.GetErrors());
            
            var productFromDb = await context.Products.FirstOrDefaultAsync(x => x.Id == productUpdateDto.Id);

            if (productFromDb is null)
                return ServiceResult<ProductUpdateDto>.Failure(OperationStatus.NotFound);

            var mapped = mapper.Map(productUpdateDto, productFromDb);
            var returnedProduct = mapper.Map(mapped, new ProductUpdateDto());
            await context.SaveChangesAsync();

            return ServiceResult<ProductUpdateDto>.Success(returnedProduct, OperationStatus.Updated);
        }
        public async Task<ServiceResult<IEnumerable<ProductUpdateDto>>> UpdateRangeAsync
            (ModelStateDictionary modelstate, IEnumerable<ProductUpdateDto> productUpdateDtos)
        {
            if (!modelstate.IsValid)
                return ServiceResult<IEnumerable<ProductUpdateDto>>.Failure(modelstate.GetErrors());
            
            var productsList = productUpdateDtos.ToList();
            List<int> productIds = new(productsList.Select(c => c.Id));
            var productsFromDb = await context.Products.Where(x => productIds.Contains(x.Id)).ToListAsync();

            if (productsList.Count > productsFromDb.Count)
                return ServiceResult<IEnumerable<ProductUpdateDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more products wasn't found"));

            List<Product> mappedProductsList = new();

            for (int i = 0; i < productsFromDb.Count; i++)
                 mappedProductsList.Add(mapper.Map(productsList[i], productsFromDb[i]));

            var returnedProducts = mapper.Map(mappedProductsList, new List<ProductUpdateDto>());
            await context.SaveChangesAsync();

            return ServiceResult<IEnumerable<ProductUpdateDto>>.Success(returnedProducts, OperationStatus.Updated);
        }

        public async Task<ServiceResult<ProductReadDto>> GetByIdAsync(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(prod => prod.Id == id);

            if (product is null)
                return ServiceResult<ProductReadDto>.Failure(OperationStatus.NotFound);

            return ServiceResult<ProductReadDto>
                .Success(mapper.Map<Product,ProductReadDto>(product), OperationStatus.Ok);
        }

        public async Task<ServiceResult<IEnumerable<ProductReadDto>>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            var productsFromDb = await context.Products.Where(prod => idsList.Contains(prod.Id)).ToListAsync();

            if (idsList.Count > productsFromDb.Count)
                return ServiceResult<IEnumerable<ProductReadDto>>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "One or more products wasn't found"));

            var productsDtos = mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDto>>(productsFromDb);
            return ServiceResult<IEnumerable<ProductReadDto>>.Success(productsDtos);
        }

        public async Task<ServiceResult<IEnumerable<ProductReadDto>>> GetRangeAsync(int skip, int take, IMemoryCache cache)
        {
            if (skip < 0 || take < 0)
                return ServiceResult<IEnumerable<ProductReadDto>>.Failure(OperationStatus.InvalidInput);
            
            var cacheItems = cache.GetOrCreate("productsRangeCache",
                async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return await context.Products.Skip(skip).Take(take).ToListAsync();
                });
            
            var productsFromDb = await cacheItems;

            if (productsFromDb == null)
                return ServiceResult<IEnumerable<ProductReadDto>>.Success(new List<ProductReadDto>());

            var productsDtos = mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDto>>(productsFromDb);
            return ServiceResult<IEnumerable<ProductReadDto>>.Success(productsDtos);
        }

        public async Task<ServiceResult<object?>> RemoveAsync(int id)
        {
            Product? product = await context.Products.FirstOrDefaultAsync(prod => prod.Id == id);
            if (product is null)
                return ServiceResult<object?>.Failure(OperationStatus.NotFound);
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
        public async Task<ServiceResult<object?>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            var idsList = ids.ToList();
            IEnumerable<Product> products = await context.Products.Where(prod => idsList.Contains(prod.Id)).ToListAsync();
            if (!products.Any())
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't products with these ids."));
            if (products.Count() < idsList.Count)
                return ServiceResult<object?>.Failure
                (new ResultStatusMessage(OperationStatus.NotFound, "There aren't products with one or more of these ids."));
            context.Products.RemoveRange(products);
            await context.SaveChangesAsync();
            return ServiceResult<object?>.Success(null);
        }
    }
}
