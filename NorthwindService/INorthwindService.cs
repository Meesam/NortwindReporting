using NorthiwindModels.DTO;

namespace NorthwindService
{
    public interface INorthwindService
    {
        Task<IReadOnlyList<CategoryDto>> GetAllCategoies();
    }
}
