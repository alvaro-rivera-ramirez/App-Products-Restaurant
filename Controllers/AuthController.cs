using ApiRestaurant.Data;
using ApiRestaurant.DTO;
using ApiRestaurant.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiRestaurant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HandleToken _handleToken;
        public AuthController(AppDbContext context, HandleToken handleToken)
        {
            _context = context;
            _handleToken = handleToken;
        }
        //public AuthController(AppDbContext context)
        //{
        //    _context = context;
        //}

        [HttpPost("login")]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.email == loginDTO.email);
            if (user == null)
            {
                return Unauthorized("Usuario y/o Password Incorrectos");
            }

            bool isPasswordValid = HandlePassword.VerifyPassword(loginDTO.password, user.password);
            if (!isPasswordValid)
            {
                return Unauthorized("Usuario y/o Password Incorrectos");
            }

            string token = _handleToken.generateToken(new UserListDTO
            {
                fullname = user.name + " " + user.lastname,
                id = user.id_user
            });



            return Created(string.Empty,new
            {
                token
            });
        }

    }
}
