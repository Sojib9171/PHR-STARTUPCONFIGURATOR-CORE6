using HBS.ImplementationAutomationConsole.API.Security;
using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.Extensions;
using HBS.ImplementationAutomationConsole.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text;

namespace HBS.ImplementationAutomationConsole.API.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginServices _iloginServices;
        private static IMemoryCache _memoryCache;
        private readonly ILogger<LoginController> _logger;
        public LoginController(
            ILoginServices iloginServices, IMemoryCache memoryCache, ILogger<LoginController> logger)
        {
            _iloginServices = iloginServices;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("api/initiate")]
        [HttpPost]
        public IActionResult GetRSAPublicKeyPEMEncoded()
        {
            if (_memoryCache.Get("rsaPublic") == null || _memoryCache.Get("rsaPrivate") == null)
            {
                try
                {
                    var AsymetricEnc = Cryptography.Initiate();
                    _memoryCache.Set("rsaPrivate", AsymetricEnc.RSAPrivateKeyXML, TimeSpan.FromMinutes(30));
                    _memoryCache.Set("rsaPublic", AsymetricEnc.RSAPublicKeyPEM, TimeSpan.FromMinutes(30));
                    _memoryCache.Set("rsaPublicXml", AsymetricEnc.RSAPublicKeyXML, TimeSpan.FromMinutes(30));

                    return Ok(AsymetricEnc.RSAPublicKeyPEM);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return Ok(_memoryCache.Get("rsaPublic"));
            }
        }

        [AllowAnonymous]
        [Route("api/test")]
        [HttpGet]
        public async Task <IActionResult> Test()
        {
            var test = "Approved";
            var x = test.CheckForValidStringResponse(true, true, true, 5);
            return Ok();
        }

        [AllowAnonymous]
        [Route("api/Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var LoginData = Cryptography.Decrypt(_memoryCache, loginDto);

                if (LoginData.UserID == "")
                {
                    return Unauthorized("Timed Out!");
                }

                if(LoginData.UserID.CheckForValidStringResponse(true,true,true,40))
                {
                    throw new Exception("Please Specify A Valid User Id");
                }

                var user = await _iloginServices.GetUserbyId(LoginData.UserID);
                bool passwordMatch = await _iloginServices.CheckPassword(LoginData.UserID, LoginData.Password);

                if (user != null && passwordMatch && user.USER_ISACTIVE)
                {

                    _memoryCache.Set("rsaPrivate_" + loginDto.UserID.Substring(0, 8), _memoryCache.Get("rsaPrivate"), TimeSpan.FromMinutes(30));
                    _memoryCache.Set("rsaPublic_" + loginDto.UserID.Substring(0, 8), _memoryCache.Get("rsaPublic"), TimeSpan.FromMinutes(30));
                    _memoryCache.Set("rsaPublicXml_" + loginDto.UserID.Substring(0, 8), _memoryCache.Get("rsaPublicXml"), TimeSpan.FromMinutes(30));
                    _memoryCache.Remove("rsaPrivate");
                    _memoryCache.Remove("rsaPublic");
                    _memoryCache.Remove("rsaPublicXml");

                    var hasUserLoggedInBefore = await _iloginServices.HasUserLoggedInBefore(user.USER_ID);
                    return Ok(new
                    {
                        UserId = user.USER_ID,
                        Name = user.NAME,
                        Username = user.USERNAME,
                        UserTypeCode = user.USER_TYPE_CODE,
                        HasUserLoggedInBefore = hasUserLoggedInBefore
                    });
                }

                if (user == null)
                {
                    return Unauthorized("User not found!");
                }

                if (!passwordMatch)
                {
                    return Unauthorized("Incorrect Password!");
                }

                if (!user.USER_ISACTIVE)
                {
                    return Unauthorized("User is Inactive!");
                }
                return Unauthorized("User");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [Route("api/updateUserLoggedInBeforeStatus")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateUserLoggedInBeforeStatus(string userId)
        {
            try
            {
                await _iloginServices.UpdateUserLoggedInBeforeStatus(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("api/LogOut")]
        [HttpPost]
        public IActionResult LogOut()
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

            _memoryCache.Remove("rsaPrivate_" + credentials[0]);
            _memoryCache.Remove("rsaPublic_" + credentials[0]);
            _memoryCache.Remove("rsaPublicXml_" + credentials[0]);

            return Ok();
        }
    }
}