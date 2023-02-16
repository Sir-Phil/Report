using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Articles.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtIssueOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtIssueOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string CreateToken(string Username)
        {
            var claim = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, Username),
               new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
               new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(_jwtOptions.IssuedAt).ToUnixTimeSeconds().ToString().ToString()
                    , ClaimValueTypes.Integer64)
           };

            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claim,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}
