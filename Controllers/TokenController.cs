using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using exam70486.Jwt;

namespace exam70486.Controllers
{
    [Route("token")]
    [AllowAnonymous]
    public class TokenController : Controller
    {
        public readonly TokenProperties _tokenProperties;

        public TokenController(TokenProperties tokenProperties)
        {
            _tokenProperties = tokenProperties;
        }

        [HttpPost]
        public IActionResult Create([FromBody]Login login)
        {
            if (login.Username == "adm" && login.Password == "adm")
            {
                var token = new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(_tokenProperties.Key))
                .AddSubject("james bond")
                .AddIssuer(_tokenProperties.Issuer)
                .AddAudience(_tokenProperties.Audience)
                .AddClaim("MembershipId", "123")
                .AddClaim("AdministratorId", "111")
                .AddExpiry(1)
                .Build();

                return Ok(token.Value);
            }

            if (login.Username == "luis" && login.Password == "lanfredi")
            {
                var token = new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(_tokenProperties.Key))
                .AddSubject("james bond")
                .AddIssuer(_tokenProperties.Issuer)
                .AddAudience(_tokenProperties.Audience)
                .AddClaim("MembershipId", "123")
                .AddExpiry(1)
                .Build();

                //return Ok(token);
                return Ok(token.Value);
            }

            return Unauthorized();
        }
    }
}