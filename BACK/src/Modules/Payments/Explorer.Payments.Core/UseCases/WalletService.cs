using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases
{
    public class WalletService: CrudService<WalletDto, Wallet>, IWalletService
    {
        protected readonly IWalletRepository _walletRepository;
        protected readonly IInternalNotificationService _notificationService;
        protected readonly IInternalEmailService _emailService;
        protected readonly IInternalUserService _userService;

        public WalletService(IWalletRepository repository, IMapper mapper, IInternalNotificationService notificationService, IInternalEmailService emailService, IInternalUserService userService) : base(repository, mapper)
        {
            _walletRepository = repository;
            _notificationService = notificationService;
            _emailService = emailService;
            _userService = userService;
        }
        public WalletDto GetByUser(int userId)
        {
            var cart = _walletRepository.GetByUser(userId);
            return MapToDto(cart);
        }
        public Result<WalletDto> AddCoins(WalletDto updatedWalletDto)
        {
            try
        {
            var existingWallet = _walletRepository.Get(updatedWalletDto.Id);

            if (existingWallet == null) return Result.Fail("Wallet not found.");

                existingWallet.AdventureCoins = updatedWalletDto.AdventureCoins;
                _walletRepository.Update(existingWallet);
                var url = "/wallet/byUser";
                _notificationService.Generate(updatedWalletDto.UserId,Stakeholders.API.Dtos.Enums.NotificationType.COINS_GIFTED,url, DateTime.UtcNow, "");
                UserDto user = _userService.Get(updatedWalletDto.UserId).Value;
                _emailService.SendEmail(user.Email, "Congratulations on Your Gifted Adventure Coins!", GetMessageText());
            return Result.Ok(new WalletDto
            {
                UserId = existingWallet.UserId,
                AdventureCoins = existingWallet.AdventureCoins
            });
        }
        catch (Exception ex)
        {
            return Result.Fail($"An error occurred while updating the wallet: {ex.Message}");
        }
        }

        public string GetMessageText()
        {
            return "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Congratulations on Your Adventure Coins!</title>\r\n    <style>\r\n        body {\r\n            font-family: 'Arial', sans-serif;\r\n            background-color: #f4f4f4;\r\n            color: #333;\r\n            margin: 0;\r\n            padding: 0;\r\n        }\r\n\r\n        .container {\r\n            max-width: 600px;\r\n            margin: 20px auto;\r\n            background-color: #fff;\r\n            padding: 20px;\r\n            border-radius: 8px;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n        }\r\n\r\n        h1 {\r\n            color: #3498db; /* Blue color for the title */\r\n            text-align: center;\r\n        }\r\n\r\n        p {\r\n            font-size: 16px;\r\n            line-height: 1.6;\r\n            text-align: justify;\r\n        }\r\n\r\n        .button {\r\n            display: inline-block;\r\n            padding: 10px 20px;\r\n            font-size: 16px;\r\n            text-align: center;\r\n            text-decoration: none;\r\n            background-color: #4CAF50;\r\n            color: #fff;\r\n            border-radius: 5px;\r\n            transition: background-color 0.3s;\r\n        }\r\n\r\n        .button:hover {\r\n            background-color: #45a049;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <h1>Congratulations!</h1>\r\n        <p>You have just been gifted Adventure Coins! Get ready for an exciting journey filled with thrilling experiences and epic quests.</p>\r\n        <p>Use your Adventure Coins wisely and make the most of every adventure that comes your way.</p>\r\n        <p>Happy exploring!</p>\r\n    </div>\r\n</body>\r\n</html>\r\n";
        }
    }
}
