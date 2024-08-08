using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SCR.API.Models.Domain;
using SCR.API.Models;
using SCR.API.Models.DTO;
using SCR.API.Data;
using SCR.API.Controllers;
using Microsoft.EntityFrameworkCore;
using SCR.API.Repositories;


namespace SCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private  UserManager<IdentityUser> userManager;
        private SCRDbContext dbContext;
        private readonly ITokenRepository tokenrep;
        
        public AuthController(UserManager<IdentityUser> userManager,  SCRDbContext dbContext, ITokenRepository tokenrep)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.tokenrep = tokenrep;
        }
        //post api/auth/ register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AddStudentDTO requestDTO)

        {
            var identityUser = new IdentityUser
            {
                UserName = requestDTO.StdUserName,
                Email = requestDTO.StdUserName,

            };
            var identityResult = await userManager.CreateAsync(identityUser, requestDTO.Password);
            if (identityResult.Succeeded)
            {
                var student = new Student
                {
                    StdName = requestDTO.StdName,
                    EsuInst=requestDTO.EsuInst,
                    StdUserName=requestDTO.StdUserName,
                    Password=requestDTO.Password
                };
                dbContext.Students.Add(student);
                await dbContext.SaveChangesAsync();
                if (requestDTO.Roles != null && requestDTO.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, requestDTO.Roles);
                    if (identityResult.Succeeded)
                    {                       
                        return Ok("succecfull registered, please login");
                    }
                }
            }
            return BadRequest("something went wrong");

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO lrd)
        {
            
            var user = await userManager.FindByEmailAsync(lrd.Username);
            if (user != null)
            {
                var checkPassword = await userManager.CheckPasswordAsync(user, lrd.Password);
                if (checkPassword)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    Student std = dbContext.Students.FirstOrDefault(u => u.StdUserName ==lrd.Username );
                    int userid = std.StdId;
                    string names= std.StdName;
                    string eduinst = std.EsuInst;

                    if (roles != null)
                    {
                        

                        var jwttoken = tokenrep.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwttoken,
                            Roles = roles.ToList(),
                           StdId = userid,
                           name = names,
                           EduInst = eduinst,
                            
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or password incorrect");
        }

    }
}
