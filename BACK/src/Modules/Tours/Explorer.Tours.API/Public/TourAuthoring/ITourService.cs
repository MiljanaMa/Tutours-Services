using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.TourAuthoring;

public interface ITourService
{
        Result<PagedResult<TourDto>> GetPaged(int page, int pageSize);
        Result<TourDto> Get(int id);
        Result<TourDto> Create(TourDto equipment);
        Result<TourDto> Update(TourDto equipment);
        Result Delete(int id);
        TourDto GetById(int id);
        Result<PagedResult<TourDto>> GetByAuthor(int page, int pageSize, int id);
        public Result<PagedResult<TourDto>> GetPublishedPaged(int page, int pageSize);
        public Result<PagedResult<TourDto>> GetArchivedAndPublishedPaged(int page, int pageSize);
        Result<TourDto> CreateCustom(TourDto equipment);
        public  Result<PagedResult<TourDto>> GetCustomByUserPaged(int userId, int page, int pageSize);
        Result<TourDto> CreateCampaign(TourDto tour);
        Result<PagedResult<TourDto>> GetCampaignByUserPaged(int userId, int page, int pageSize);
        public  Result<PagedResult<TourDto>> GetPublishedByAuthor(int authorId, int page, int pageSize);

}


