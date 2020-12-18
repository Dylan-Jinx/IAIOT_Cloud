using IdentityServer4.Models;
using IdentityServer4.Services;
using log4net.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Config
{
    public class CustomProfileService : IProfileService
    {
        private readonly ILogger<CustomProfileService> _logger;
        public CustomProfileService
            (
                ILogger<CustomProfileService> logger
            )
        {
            _logger = logger;
        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //取决于访问用户数据的范围。
                var claims = context.Subject.Claims.ToList();

                //set issued claims to return
                context.IssuedClaims = claims.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
