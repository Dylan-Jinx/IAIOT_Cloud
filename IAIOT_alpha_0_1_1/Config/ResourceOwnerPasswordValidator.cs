using IAIOT_alpha_0_1_1.Models;
using IdentityModel;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Config
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IAIOTCloudContext _context;
        public ResourceOwnerPasswordValidator(IAIOTCloudContext context)
        {
            _context = context;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //账号密码验证
            bool flag =  _context.TSysUsers
                .Where(a => a.Telephone == context.UserName && a.Password == context.Password)
                .Any();
            if (flag)
            {
                context.Result = new GrantValidationResult(
                    subject: context.UserName,
                    authenticationMethod: "custom",
                    claims: GetUserClaims(context)
                    );
            }
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
            return Task.CompletedTask;
        }

        private IEnumerable<Claim> GetUserClaims(ResourceOwnerPasswordValidationContext context)
        {
            var userId = _context.TSysUsers
                .Where(a => a.Telephone == context.UserName)
                .FirstOrDefault()
                .UserId;
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Id,Guid.NewGuid().ToString().Replace("-","")),
                new Claim(JwtClaimTypes.ClientId,context.Request.ClientId),
                new Claim(JwtClaimTypes.PhoneNumber, context.UserName),
                new Claim("userId",userId.ToString()),
            };
        }
    }
}
