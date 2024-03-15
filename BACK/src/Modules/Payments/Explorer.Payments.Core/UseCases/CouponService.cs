using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Internal;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases
{
    public class CouponService : CrudService<CouponDto, Coupon>, ICouponService
    {
        protected readonly ICouponRepository _couponRepository;
        protected readonly IInternalTourService _internalTourService;

        public CouponService(ICouponRepository repository, IMapper mapper, IInternalTourService internalTourService) : base(repository, mapper)
        {
            _couponRepository = repository;
            _internalTourService = internalTourService;
        }

        private static string GenerateRandomAlphanumericString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
        public Result<CouponDto> Create(CouponDto coupon)
        {
            coupon.Code = GenerateRandomAlphanumericString();
            return base.Create(coupon);
        }

        public Result<PagedResult<CouponDto>> GetCouponForTourAndTourist(int page, int pageSize, int tourId, int touristId)
        {
            Result<TourDto> tourResult = _internalTourService.Get(tourId);
            TourDto tourDto = tourResult.Value;
            var result = _couponRepository.GetCouponForTourAndTourist(page, pageSize, tourId, touristId);
            int authorId = tourDto.UserId;
            var resultAuthor = _couponRepository.GetCouponForAuthorAndTourist(page, pageSize, authorId, touristId);

            PagedResult<Coupon> couponsFromTour = result.Value;
            PagedResult<Coupon> couponsFromAuthor = resultAuthor.Value;

            List<Coupon> allCouponsList = new List<Coupon>(couponsFromTour.Results);
            allCouponsList.AddRange(couponsFromAuthor.Results);

            PagedResult<Coupon> allCoupons = new PagedResult<Coupon>(allCouponsList, allCouponsList.Count);

            return MapToDto(allCoupons);
        }

        public Result<PagedResult<CouponDto>> GetCouponsForAuthor(int page, int pageSize, int authorId)
        {
            var result = _couponRepository.GetCouponsForAuthor(page, pageSize, authorId);
            return MapToDto(result);
        }

        public CouponDto GetCouponByTourId(int tourId)
        {
            var result = _couponRepository.GetCouponByTourId(tourId); 
            return MapToDto(result);
        }

    }
}
