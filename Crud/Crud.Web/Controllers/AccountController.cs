using Crud.Business.ContractBusiness;
using Curd.Common.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
        private readonly IConfiguration config;

        public AccountController(IUserBusiness userBusiness, IConfiguration config)
        {
            this.userBusiness = userBusiness;
            this.config = config;

        }


        #region Metodos Privados

        private string GenerarToken(List<Claim> claims)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                        issuer: config["Jwt:Issuer"],
                        audience: config["Jwt:Issuer"],
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds,
                        claims: claims
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        #endregion

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel<LoginViewModel>()
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                });
            }

            var user = await userBusiness.Login(model);

            if (!user.Success)
            {
                user.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(user);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Object.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Object.Email),
                new Claim("idusuario", user.Object.Id.ToString()),
                new Claim("nombre", user.Object.Email),
                new Claim("email", user.Object.Email),
            };

            user.Object.Token = GenerarToken(claims);
            user.StatusCode = (int)HttpStatusCode.OK;

            return Ok(user);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseViewModel<UserViewModel>>> Registry([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel<UserViewModel>()
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                });
            }

            var user = await userBusiness.Add(model);

            if (!user.Success)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, user);
            }

            user.StatusCode = (int)HttpStatusCode.OK;
            return Ok(user);
        }
    }
}