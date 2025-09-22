using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace HBS.ImplementationAutomationConsole.API.Security
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ILoginServices _iloginServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static IMemoryCache _memoryCache;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                          ILoggerFactory logger,
                                          UrlEncoder encoder,
                                          ISystemClock clock,
                                          IHttpContextAccessor httpContextAccessor,
                                          IMemoryCache memoryCache,
                                          ILoginServices iloginServices) : base(options, logger, encoder, clock)
        {
            _iloginServices = iloginServices;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = _httpContextAccessor.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return AuthenticateResult.NoResult();
            }

            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            Users user = new Users();
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

                LoginDto loginDto = new LoginDto()
                {
                    UserID = credentials[0],
                    Password = credentials[1],
                };

                var userCode = credentials[2];
                var userName = credentials[3];
                var name = credentials[4];

                var AuthData = Cryptography.DecryptAfterAuth(_memoryCache, loginDto);

                user = await _iloginServices.GetUserbyId(AuthData.UserID);
                bool userTypeCodeMatched = false;
                bool userNameMatched = false;

                try
                {
                    userTypeCodeMatched = (AuthData.UserID == user.USER_ID && AuthData.Password == user.PASSWORD && userCode != user.USER_TYPE_CODE) ? false : true;
                }
                catch (Exception ex)
                {
                    return AuthenticateResult.Fail("Session Timed Out!");
                }

                try
                {
                    userNameMatched = (AuthData.UserID == user.USER_ID && AuthData.Password == user.PASSWORD && name != user.NAME) ? false : true;
                }
                catch (Exception ex)
                {
                    return AuthenticateResult.Fail("Session Timed Out!");
                }

                //Increasing logged in time out
                string userKey = loginDto.UserID.Substring(0, 8);
                List<string> cachekeys = new List<string>();
                cachekeys.Add("rsaPrivate_" + userKey);
                cachekeys.Add("rsaPublic_" + userKey);
                cachekeys.Add("rsaPublicXml_" + userKey);

                foreach (var key in cachekeys)
                {
                    _memoryCache.Set(key, _memoryCache.Get(key), TimeSpan.FromMinutes(60));
                }
                cachekeys.RemoveAll(x => x.Contains(userKey));

                if (AuthData.UserID == "")
                {
                    return AuthenticateResult.Fail("Session Timed Out!");
                }
                if (user == null)
                {
                    return AuthenticateResult.Fail("User does not exist!");
                }
                if (!await _iloginServices.CheckPassword(user.USER_ID, AuthData.Password))
                {
                    return AuthenticateResult.Fail("Incorrect Password!");
                }
                if (!user.USER_ISACTIVE)
                {
                    return AuthenticateResult.Fail("User is Inactive!");
                }
                if (!userTypeCodeMatched)
                {
                    return AuthenticateResult.Fail("User Role Does Not Match");
                }
                if (!userNameMatched)
                {
                    return AuthenticateResult.Fail("Name Not Match");
                }
                if (userName != user.USERNAME)
                {
                    return AuthenticateResult.Fail("User Name Does Not Match");
                }
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.USER_ID.ToString()),
            };

            string userRole = DetermineUserRole(user.USER_TYPE_CODE);
            claims.Add(new Claim(ClaimTypes.Role, userRole));

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private string DetermineUserRole(string userTypeCode)
        {
            switch (userTypeCode)
            {
                case "1":
                    return "User";
                case "2":
                    return "Admin";
                case "3":
                    return "User Admin";
                default:
                    return "Not Valid User";
            }
        }
    }
}