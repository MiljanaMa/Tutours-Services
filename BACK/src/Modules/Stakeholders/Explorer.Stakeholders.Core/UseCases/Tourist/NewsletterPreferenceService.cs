using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Public.Tourist;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Internal;
using Explorer.Tours.Core.Domain;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases.Tourist;

public class NewsletterPreferenceService : CrudService<NewsletterPreferenceDto, NewsletterPreference>, INewsletterPreferenceService
{
    private ICrudRepository<NewsletterPreference> _newsletterPreferenceRepository;
    private IUserService _userService;
    private IInternalEmailService _internalEmailService;
    private IInternalTourService _tourService;
    public NewsletterPreferenceService(ICrudRepository<NewsletterPreference> crudRepository, IMapper mapper,
        IUserService userService, IInternalEmailService emailService, IInternalTourService tourService) : base(crudRepository, mapper)
    {
        _newsletterPreferenceRepository = crudRepository;
        _userService = userService; 
        _internalEmailService = emailService;
        _tourService = tourService;
    }

    public override Result<NewsletterPreferenceDto> Create(NewsletterPreferenceDto np)
    {
        try
        {
            try
            {
                var exists = CrudRepository.Get(np.UserID);
                exists.Frequency = np.Frequency;
                exists.LastSent = np.LastSent;
                return MapToDto(CrudRepository.Update(exists));
            }
            catch (KeyNotFoundException) 
            {
                return MapToDto(CrudRepository.Create(MapToDomain(np)));
            }
                
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public List<NewsletterPreferenceDto> CheckCandidatesForNewsletter()
    {
        var newsletters = _newsletterPreferenceRepository.GetPaged(0, 0).Results;
        List<NewsletterPreferenceDto> validNl = new List<NewsletterPreferenceDto>();
        foreach(NewsletterPreference nl in newsletters)
        {
            if(nl.LastSent.AddDays((double)nl.Frequency) < DateTime.Now && nl.Frequency != 0)
            {
                validNl.Add(MapToDto(nl));
            }
        }
        return validNl;
    }

    public void SendNewsletter(List<NewsletterPreferenceDto> candidates)
    {
        foreach(NewsletterPreferenceDto nl in candidates)
        {
            string email = _userService.Get((int)nl.UserID).Value.Email;
            List<TourDto> PersonalizedTours = GetPersonalizedTours();
            List<TourDto> HotTours = GetHotTours();
            _internalEmailService.SendEmail(email, "Newsletter", GetMessageText(PersonalizedTours, HotTours));
            nl.LastSent = DateTime.UtcNow;
        }

        foreach (NewsletterPreferenceDto nl in candidates)
            Update(nl);
    }

    public List<TourDto> GetPersonalizedTours()
    {
        //PRASKA OVDE
        return _tourService.GetPaged(0,0).Value.Results;
    }

    public List<TourDto> GetHotTours()
    {
        //PRASKA OVDE
        return _tourService.GetPaged(0, 0).Value.Results;
    }

    public string GetMessageText(List<TourDto> personalized, List<TourDto> hot)
    {
        String message = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <title>Newsletter</title>\r\n  <style>\r\n    /* CSS stilovi */\r\n    body {\r\n      font-family: Arial, sans-serif;\r\n      background-color: #f4f4f4;\r\n      margin: 0;\r\n      padding: 0;\r\n    }\r\n    .container {\r\n      max-width: 600px;\r\n      margin: 20px auto;\r\n      background-color: #fff;\r\n      padding: 20px;\r\n      border-radius: 8px;\r\n      box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n    }\r\n    .list {\r\n      padding: 10px;\r\n      border: 1px solid #ccc;\r\n      border-radius: 5px;\r\n      margin-bottom: 20px;\r\n    }\r\n    .list h2 {\r\n      margin-top: 0;\r\n    }\r\n    .item {\r\n      margin-bottom: 5px;\r\n    }\r\n    /* Dodatni stil za linkove */\r\n    .item a {\r\n      text-decoration: none;\r\n      color: #3498db;\r\n    }\r\n  </style>\r\n</head>\r\n<body>\r\n  <div class=\"container\">\r\n    <div class=\"list\">\r\n      <h2>Recomended tours:</h2>\r\n     ";
        foreach (TourDto t in personalized)
        {
            message += "<div class=\"item\">\r\n";
            message += t.Name + " " + t.Price + " " + t.Difficulty + "\n";
            message += "</div>\r\n";
        }
        message += "</div><div class=\"list\">\r\n<h2>Hot tours</h2>";
        foreach (TourDto t in hot)
        {
            message += "<div class=\"item\">\r\n";
            message += t.Name + " " + t.Price + " " + t.Difficulty + "\n";
            message += "</div>\r\n";
        }
        message += "</div>";
        return message;
    }
}
