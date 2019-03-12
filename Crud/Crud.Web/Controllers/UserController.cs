using Crud.Business.ContractBusiness;
using Curd.Common.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Crud.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            this.userBusiness = userBusiness;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseViewModel<UserViewModel>>> Get([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ResponseViewModel<UserViewModel>()
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                });
            }

            var user = await userBusiness.Get(id);

            if (!user.Success)
            {
                user.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(user);
            }

            user.StatusCode = (int) HttpStatusCode.OK;
            return Ok(user);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ResponseViewModel<UserViewModel>>> GetAll()
        {
            var user = await userBusiness.GetAll();
            user.StatusCode = (int)HttpStatusCode.OK;
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseViewModel<UserViewModel>>> Post([FromBody] UserViewModel model)
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

        [HttpPut]
        public async Task<ActionResult<ResponseViewModel<UserViewModel>>> Put([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel<UserViewModel>() {
                    Success = false,
                    StatusCode = (int) HttpStatusCode.BadRequest,
                });
            }

            if (model.Id <= 0)
            {
                return BadRequest(new ResponseViewModel<UserViewModel>()
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                });
            }

            var user = await userBusiness.Update(model);

            if (!user.Success)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, user);
            }

            user.StatusCode = (int)HttpStatusCode.OK;
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseViewModel<UserViewModel>>> Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ResponseViewModel<UserViewModel>()
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                });
            }

            var user = await userBusiness.Delete(id);
            user.StatusCode = (int)HttpStatusCode.OK;
            return Ok(user);
        }
    }
}