using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRestaurant.Data;
using ApiRestaurant.DTO;
using ApiRestaurant.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using ApiRestaurant.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiRestaurant.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController:ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _context.Users.ToListAsync();

            List<UserListDTO> userListDTO = users.Select(user => new UserListDTO
            {
                id = user.id_user,
                fullname = user.name + " " + user.lastname,
                email = user.email,
            }).ToList();
            return Ok(userListDTO);
        }

        [HttpPost("register")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Post([FromBody] UserRegisterDTO userRegisterDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var hashPassword = HandlePassword.EncryptPassword(userRegisterDTO.password);
            userRegisterDTO.password = hashPassword;
            await _context.Users.AddAsync(
                new User {
                    name=userRegisterDTO.name,
                    lastname=userRegisterDTO.lastname,
                    email=userRegisterDTO.email,
                    password=hashPassword
                });

            await _context.SaveChangesAsync();
            var lastUser = _context.Users.ToList().Last();

            return CreatedAtAction(nameof(Post), new UserListDTO
            {
                id= lastUser.id_user,
                fullname = lastUser.name+" "+ lastUser.lastname,
                email = lastUser.email,
            });

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.id_user == id);
            if (user == null)
                return NotFound();

            return Ok(new UserListDTO
            {
                id = user.id_user,
                fullname = user.name + " " + user.lastname,
                email = user.email,
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(c => c.id_user == id);
            if (user == null)
                return NotFound();


            _context.Entry(user).CurrentValues.SetValues(userUpdateDTO);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.id_user == id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
