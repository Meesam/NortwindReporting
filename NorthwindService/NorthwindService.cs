using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NorthiwindModels.DTO;
using NortwindReporting.Models;

namespace NorthwindService
{
    public class NorthwindService : INorthwindService
    {
        private readonly NorthwindContext _northwindContext;
        private readonly ICacheService _cacheService;

        public NorthwindService(NorthwindContext northwindContext, ICacheService cacheService)
        {
            _northwindContext = northwindContext;
            _cacheService = cacheService;
        }
        public async Task<IReadOnlyList<CategoryDto>> GetAllCategoies()
        {
            if (_cacheService.GetCachedValue("categories", out List<CategoryDto> categories))
                return categories;

            var result = await _northwindContext.Categories
               .Select(c => new CategoryDto
               {
                  CategoryId = c.CategoryId,
                  CategoryName =  c.CategoryName
               }).ToListAsync();

            _cacheService.SetCache("categories", result);
          
            return result;
        }
    }
}
