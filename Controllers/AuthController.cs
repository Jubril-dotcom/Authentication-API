using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Authentication_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();

        public IConfiguration Configuration { get; }

        public AuthController(IConfiguration configuration) 
        {
            _Configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] PasswordHash, out byte[] PasswordSalt);

            user.Username = request.Username;
            user.PasswordHash = PasswordHash;
            user.PasswordSalt = PasswordSalt;

            return Ok(user);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<String>> Login(UserDto request)
        {
            if (user.Username == request.Username)
            {
                return BadRequest("user not found.");
            }
            if(!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            String token = CreateToken(user);
            return Ok("token");
        }

        private String CreateToken(User user)
        {
            List<Claim> Claims = new List<Claim>
            {
                new Claim(ClaimType.NAME.usre.Userneme)
            };
            var Key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _Configuration.GetSection("AppSetting:Token").Value));

            var cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

            var token = new jwtsecurityToken(
                Claims:Claims,
                expires:DateTime.Now.AddDays(1),
                SingingCreadentials.creds);

            var jwt = new jwtsecurityTokenHandler().WriteToken(token)

                return jwt;
                



            return String.Empty;
        }
        




        private void CreatePasswordHash(string password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512(PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(PasswordHash);
            }
        }
    }
}
