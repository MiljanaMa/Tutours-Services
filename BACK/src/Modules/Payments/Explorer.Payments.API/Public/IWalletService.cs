using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Public
{
    public interface IWalletService
    {
        Result<WalletDto> Create(WalletDto wallet);
        Result<PagedResult<WalletDto>> GetPaged(int page, int pageSize);
        Result<WalletDto> AddCoins(WalletDto wallet);
        WalletDto GetByUser(int userId);
        Result Delete(int id);
    }
}
