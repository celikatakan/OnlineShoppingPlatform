using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineShoppingPlatform.WebApi.Jwt
{
    public static class JwtHelper  // Static class for generating JSON Web Tokens (JWT)
    {
        public static string GenerateJwtToken(JwtDto jwtInfo) // Method to generate a JWT token using provided information
        {
            // Create a symmetric security key from the secret key string
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.SecretKey));
            // Create signing credentials using the security key and HMAC SHA256 algorithm
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]  // Define claims to be included in the JWT
            {
                new Claim(JwtClaimNames.Id, jwtInfo.Id.ToString()),
                new Claim(JwtClaimNames.FirstName, jwtInfo.FirstName),
                new Claim(JwtClaimNames.LastName, jwtInfo.LastName),
                new Claim(JwtClaimNames.Email, jwtInfo.Email),
                new Claim(JwtClaimNames.PhoneNumber, jwtInfo.PhoneNumber),
                new Claim(JwtClaimNames.UserType, jwtInfo.UserType.ToString()),
                new Claim(ClaimTypes.Role, jwtInfo.UserType.ToString())
            };

            var expireTime = DateTime.Now.AddMinutes(jwtInfo.ExpireMinutes);   // Set the token expiration time   

            // Create a JWT security token descriptor with issuer, audience, claims, and expiration
            var tokenDescriptor = new JwtSecurityToken(jwtInfo.Issuer, jwtInfo.Audience, claims, null, expireTime, credentials);

            // Write the token to a string format
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return token;
        }
    }
}
