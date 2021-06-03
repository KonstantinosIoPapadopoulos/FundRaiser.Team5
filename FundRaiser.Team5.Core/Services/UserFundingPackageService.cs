﻿using FundRaiser_Team5.Interfaces;
using FundRaiser_Team5.Model;
using FundRaiser_Team5.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundRaiser_Team5.Services.Implementation
{
    class UserFundingPackageService : IUserFundingPackageService
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<UserFundingPackageService> _logger;

        public UserFundingPackageService(IApplicationDbContext context, ILogger<UserFundingPackageService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result<OptionUserFundingPackage>> CreateUserFundingPackageAsync(OptionUserFundingPackage optionUserFundingPackage)
        {

            //UserFundingPackage userFundingPackage = optionUserFundingPackage.GetUserFundingPackage();
            //userFundingPackage.User = dbUser;
            //userFundingPackage.FundingPackage = dbFundingPackage;
            //db.UserFundingPackages.Add(userFundingPackage);
            //db.SaveChanges();
            //return new OptionUserFundingPackage(userFundingPackage);

            if (optionUserFundingPackage == null)
            {
                return new Result<OptionUserFundingPackage>(ErrorCode.BadRequest, "Null options.");
            }

            if (optionUserFundingPackage.UserId <= 0 ||
                optionUserFundingPackage.FundingPackageId <= 0 ||
                optionUserFundingPackage.Price < 0)
            {
                return new Result<OptionUserFundingPackage>(ErrorCode.BadRequest, "Not all required customer options provided.");
            }

            FundingPackage dbfundingPackage = await _context.FundingPackages.SingleOrDefaultAsync(fundingPackage => fundingPackage.FundingPackageId == optionUserFundingPackage.FundingPackageId);
            FundingPackage dbUser = await _context.Users.SingleOrDefaultAsync(user => user.UserId == optionUserFundingPackage.UserId);

            if (dbfundingPackage == null)
            {
                return new Result<OptionUserFundingPackage>(ErrorCode.NotFound, $"FundingPackage with id #{optionUserFundingPackage.FundingPackageId} not found.");
            }
            if (dbUser == null)
            {
                return new Result<OptionUserFundingPackage>(ErrorCode.NotFound, $"User with id #{optionUserFundingPackage.UserId} not found.");
            }
            UserFundingPackage userFundingPackage = optionUserFundingPackage.GetUserFundingPackage();

            await _context.UserFundingPackages.AddAsync(userFundingPackage);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new Result<OptionUserFundingPackage>(ErrorCode.InternalServerError, "Could not save UserFundingPackage.");
            }

            return new Result<OptionUserFundingPackage>
            {
                Data = new OptionUserFundingPackage( userFundingPackage)
            };
        }

        public async Task<Result<int>> DeleteUserFundingPackageAsync(int userFundingPackageId)
        {
            UserFundingPackage dbUserFundingPackage = await _context.UserFundingPackages.SingleOrDefaultAsync(UserFundingPackage => UserFundingPackage.UserFundingPackageId == UserFundingPackageId);
            if (dbUserFundingPackage == null)
            {
                return new Result<int>(ErrorCode.NotFound, $"FundingPackage with id #{userFundingPackageId} not found.");
            }

            dbUserFundingPackage.IsActive = optionUserFundingPackage.IsActive;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<int>(ErrorCode.InternalServerError, "Could not Update OptionUserFundingPackage.");
            }

            return new Result<int>
            {
                Data = 1
            };
        }

        public async Task<Result<List<OptionUserFundingPackage>>> ReadUserFundingPackageAsync()
        {
            List<UserFundingPackage> userFundingPackages = await _context.UserFundingPackages.ToListAsync();
            List<OptionUserFundingPackage> optionUserFundingPackages = new();
            userFundingPackages.ForEach(userFundingPackage => optionUserFundingPackages.Add(new OptionUserFundingPackage(userFundingPackage)));

            return new Result<List<OptionUserFundingPackage>>
            {
                Data = optionUserFundingPackages.Count > 0 ? optionUserFundingPackages : new List<OptionUserFundingPackage>()
            };
        }

        public async Task<Result<OptionUserFundingPackage>> ReadUserFundingPackageAsync(int userFundingPackageId)
        {
            if (userFundingPackageId <= 0)
            {
                return new Result<OptionUserFundingPackage>(ErrorCode.BadRequest, "Id cannot be less than or equal to zero.");
            }
            UserFundingPackage dbUserfundingPackage = await _context.UserFundingPackages.SingleOrDefaultAsync(userFundingPackage => userFundingPackage.UserFundingPackageId == userFundingPackageId);
            if (dbUserfundingPackage == null)
            {
                return new Result<OptionUserFundingPackage>(ErrorCode.NotFound, $"UserFundingPackage with id #{userFundingPackageId} not found.");
            }

            return new Result<OptionUserFundingPackage>
            {
                Data = new OptionUserFundingPackage(dbUserfundingPackage)
            };
            throw new NotImplementedException();
        }

        public async Task<Result<List<OptionUserFundingPackage>>> ReadUserFundingPackageAsync(OptionUserFundingPackage optionUserFundingPackage)
        {
            // Microsoft.EntityFrameworkCore.DbSet<UserFundingPackage> dbUserFundingPackages = _context.UserFundingPackages;
            var dbUserFundingPackages = _context.UserFundingPackages;
            if (!(optionUserFundingPackage.UserId <= 0))
                dbUserFundingPackages.Where(userFundingPackage => userFundingPackage.User.UserId.Equals(optionUserFundingPackage.UserId));
            if (!(optionUserFundingPackage.FundingPackageId <= 0))
                dbUserFundingPackages.Where(userFundingPackage => userFundingPackage.FundingPackage.FundingPackageId.Equals(optionUserFundingPackage.FundingPackageId));
            List<UserFundingPackage> userFundingPackages = await dbUserFundingPackages.ToListAsync();
            List<OptionUserFundingPackage> optionUserFundingPackages = new();
            userFundingPackages.ForEach(userFundingPackage => optionUserFundingPackages.Add(new OptionUserFundingPackage(userFundingPackage)));

            return new Result<List<OptionUserFundingPackage>>
            {
                Data = optionUserFundingPackages
            };
        }

        public Task<Result<List<OptionUserFundingPackage>>> ReadUserFundingPackagesByProjectIdAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<OptionUserFundingPackage>> UpdateUserFundingPackageAsync(int userFundingPackageId, OptionUserFundingPackage optionUserFundingPackage)
        {
            if (userFundingPackageId <= 0)
            {
                return new Result<OptionUserFundingPackage>(ErrorCode.BadRequest, "Id cannot be less than or equal to zero.");
            }
            UserFundingPackage dbUserFundingPackage = await _context.UserFundingPackages.SingleOrDefaultAsync(UserFundingPackage => UserFundingPackage.UserFundingPackageId == UserFundingPackageId);
            if (dbUserFundingPackage == null)
            {
                return new Result<OptionUserFundingPackage>(ErrorCode.NotFound, $"FundingPackage with id #{userFundingPackageId} not found.");
            }

            dbUserFundingPackage.Price = optionUserFundingPackage.Price;
            dbUserFundingPackage.UserFundingPackageStatus = optionUserFundingPackage.UserFundingPackageStatus;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<OptionUserFundingPackage>(ErrorCode.InternalServerError, "Could not save OptionUserFundingPackage.");
            }

            return new Result<OptionUserFundingPackage>
            {
                Data = new OptionUserFundingPackage(dbUserFundingPackage)
            };
        }
    }
}