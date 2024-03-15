using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public.Blog;
using Explorer.Blog.API.Public.Commenting;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.Enums;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using FluentResults;

namespace Explorer.Blog.Core.UseCases.Blog;

public class BlogService : BaseService<BlogDto, Domain.Blog>, IBlogService
{
    private readonly IBlogStatusService _blogStatusService;
    private readonly IMapper _mapper;
    private readonly IProfileService _profileService;
    private readonly IBlogRepository _repository;
    private readonly IUserService _userService;
    private readonly IBlogCommentService _blogCommentService;

    public BlogService(IBlogStatusService blogStatusService, IBlogRepository crudRepository, IMapper mapper,
        IUserService userService, IProfileService profileService,  IBlogCommentService blogCommentService) : base(mapper)
    {
        _blogStatusService = blogStatusService;
        _repository = crudRepository;
        _mapper = mapper;
        _userService = userService;
        _profileService = profileService;
        _blogCommentService = blogCommentService;
    }

    public Result<BlogDto> Create(BlogDto blog)
    {
        try
        {
            var result = _repository.Create(MapToDomain(blog));
            var newDto = MapToDto(result);
            var users = _userService.GetPaged(0, 0);
            foreach (var user in users.Value.Results)
                if (user.Id == newDto.CreatorId && user.Role == 0)
                    throw new ArgumentException("Administrator cannot create blog.");
            return MapToDto(result);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public Result Delete(int id)
    {
        try
        {
            _repository.Delete(id);
            return Result.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public Result<BlogDto> Get(int id)
    {
        try
        {
            var result = _repository.Get(id);
            var newDto = MapToDto(result);
            //TODO Promeni kada neko doda get({id})
            if (newDto.CreatorId != 0)
            {
                var users = _userService.GetPaged(0, 0);
                foreach (var user in users.Value.Results) LoadUserInformation(newDto, user);

                LoadPersonInformation(newDto);

                foreach (var ratingDto in newDto.BlogRatings)
                {
                    var user = _userService.GetPaged(0, 0).Value.Results.Find(u => u.Id == ratingDto.UserId);
                    if (user != null) ratingDto.Username = user.Username;
                }
            }

            return newDto;
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public Result<PagedResult<BlogDto>> GetPaged(int page, int pageSize)
    {
        var result = _repository.GetPaged(page, pageSize).Results;
        List<BlogDto> dtos = new();
        foreach (var item in result) dtos.Add(MapToDto(item));

        //TODO Promeni kada neko doda get({id})
        var users = _userService.GetPaged(0, 0);
        foreach (var user in users.Value.Results)
        foreach (var dto in dtos)
            LoadUserInformation(dto, user);

        foreach (var dto in dtos) LoadPersonInformation(dto);

        foreach (var dto in dtos)
        foreach (var ratingDto in dto.BlogRatings)
        {
            var user = _userService.GetPaged(0, 0).Value.Results.Find(u => u.Id == ratingDto.UserId);
            if (user != null) ratingDto.Username = user.Username;
        }

        PagedResult<BlogDto> res = new(dtos, dtos.Count);
        return res;
    }

    public PagedResult<BlogDto> GetWithStatuses(int page, int pageSize)
    {
        var result = _repository.GetWithStatuses(page, pageSize).Results;
        List<BlogDto> dtos = new();
        foreach (var item in result) dtos.Add(MapToDto(item));
        PagedResult<BlogDto> res = new(dtos, dtos.Count);
        return res;
    }


    public Result<BlogDto> Update(BlogDto blog)
    {
        try
        {
            var result = _repository.Update(MapToDomain(blog));
            return MapToDto(result);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public void UpdateStatuses(BlogDto blogDto, string status)
    {
        if (_blogStatusService.GetPaged(0, 0).Value.Results.Find((s => s.Name == status && s.BlogId == blogDto.Id)) == null)
        {
            _blogStatusService.Generate(blogDto.Id, status);
        }
    }

    public Result<BlogDto> AddRating(BlogRatingDto blogRatingDto, long userId)
    {
        var blog = _repository.GetBlog(Convert.ToInt32(blogRatingDto.BlogId));
        if (blog.SystemStatus == Domain.Enum.BlogSystemStatus.CLOSED)
        {
            return Result.Fail("Blog is closed");
        }
        var rating = new BlogRating(blogRatingDto.BlogId, userId, blogRatingDto.CreationTime,
            Enum.Parse<Rating>(blogRatingDto.Rating));
        blog.AddRating(rating);
        DetermineStatus(blog);
        return Update(MapToDto(blog));
    }

    public void LoadPersonInformation(BlogDto dto)
    {
        if (dto.CreatorId != 0)
        {
            var person = _profileService.GetProfile(dto.CreatorId).Value;
            dto.CreatorName = person.Name;
            dto.CreatorSurname = person.Surname;
        }
    }

    public void LoadUserInformation(BlogDto dto, UserDto user)
    {
        if (user.Id == dto.CreatorId && dto.CreatorId != 0) dto.CreatorRole = user.Role;
    }

    public void DetermineStatus(Domain.Blog blog)
    {
        string status;
        var upvotes = blog.BlogRatings.Count(b => b.Rating == Rating.UPVOTE);
        var downvotes = blog.BlogRatings.Count(b => b.Rating == Rating.DOWNVOTE);
        var commentNumber = _blogCommentService.GetPaged(0, 0, blog.Id).Value.Results.Count();

        double score = (upvotes - downvotes) * (commentNumber + 1);

        if (score > 35 || commentNumber >= 2)
        {
            status = "POPULAR";
        }
        else if (score > 2 || commentNumber >= 1)
        {
            status = "ACTIVE";
        }
        else if(score < 0)
        {
            blog.CloseBlog();
            status = "CLOSED";
        }
        else
        {
            status = "NEW";
        }

        UpdateStatuses(MapToDto(blog),status);
    }
}