using BL.ForAPI.DTO;
using BL.Models.Interfaces;
using BL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WEBServer.Bodies;

namespace WEBServer.APIControllers.Implementations
{
    public class AuthController : ControllerBase
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private IUser user;
        public AuthController(IUser user)
        {
            this.user = user;
        }

        [HttpPost("api/auth")]
        public async Task<ActionResult> Authentification([FromBody] AuthorizationData authData)
        {
            try
            {
                UserData? userData = await user.GetUser(new UserData(1, authData.Login, authData.Password));
                if (userData != null)
                {
                    var tokenOptions = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: new List<Claim>() { new Claim(JwtRegisteredClaimNames.Sub, userData.ID.ToString()) },
                        expires: DateTime.Now.AddMinutes(AuthOptions.LIFETIME),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    return Ok(new { Token = tokenString, UserID = userData.ID });
                }
                else
                    return StatusCode(401);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }

        }

        [HttpPost("api/users")]
        public async Task<ActionResult> Registration([FromBody] AuthorizationData authData)
        {
            try
            {
                await user.Registration(new UserData(1, authData.Login, authData.Password));
                return Ok();
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }

        }
    }
}
