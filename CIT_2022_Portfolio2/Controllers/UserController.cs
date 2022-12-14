using AutoMapper;
using CIT_2022_Portfolio2.Models;
using DataLayer;
using DataLayer.DataTransferObjects;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIT_2022_Portfolio2.Controllers
{
    [Route("api/users")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly Hashing _hashing;

        public UserController(Hashing hashing, IDataService dataService, LinkGenerator generator, IMapper mapper, IConfiguration configuration)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
            _configuration = configuration;
            _hashing = hashing;
        }

        [HttpGet(Name = nameof(getUsers))]
        //[Authorize]
        public IActionResult getUsers()
        {
            var users = _dataService.getUsers().Select(createUserModel);
            return Ok(users);
        }

        [HttpGet("{username}", Name = nameof(getUser))]
        //[Authorize]
        public IActionResult getUser([FromRoute] string username)
        {
            try
            {
                //HardCoded User
                //var username = _configuration.GetSection("Auth:Username").Value;
                var user = _dataService.getUser(username);

                if (user != null)
                {
                    var model = createUserModel(user);
                    return Ok(model);
                }
                else { return NotFound(); }
            }
            catch
            {
                return Unauthorized();
            }

        }

        [HttpPut]
        [Route("{username}/bookmarkperson/{personID}")]
        [Authorize]
        public IActionResult inputBookmarkPerson([FromRoute] string username, [FromRoute] string personID)
        {
            try
            {
                var user = _dataService.getUser(username);
                var person = _dataService.getPerson(personID);

                if (user != null && person != null)
                {
                    var bookmark = _dataService.createBookmarkPerson(username, personID);

                    return Ok(createUserModel(user));
                }
                else { return BadRequest(); }
                
            { return Unauthorized(); }
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("{username}/bookmarktitle/{titleID}")]
        [Authorize]
        public IActionResult inputBookmarkTitle([FromRoute] string username, [FromRoute] string titleID)
        {
            try
            {
                //username = _configuration.GetSection("Auth:Username").Value;
                //title = _dataService.getTitle(title).TitleId;
                //var user = _dataService.getUser(username);
                var user = _dataService.getUser(username);
                var title = _dataService.getTitle(titleID);
                if (user != null && title != null)
                {
                    var bookmark = _dataService.createBookmarkTitle(username, titleID);

                    return Ok(createUserModel(user));
                }
                else { return BadRequest(); }
            }
            catch
            {
                return Unauthorized();
            }
        }

        //Delete user
        [HttpDelete ("{username}/delete", Name = nameof(DeleteUser))]
        [Authorize]
        public IActionResult DeleteUser([FromRoute] string username)
        {
            try
            {
                if (_dataService.getUser(username) != null)
                {
                    var deleteuser = _dataService.deleteUser(username);
                    return Ok(getUsers());
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPut ("{username}/updatepassword/{newpassword}", Name = nameof(UpdatePassword))]
        [Authorize]
        public IActionResult UpdatePassword([FromRoute] string username, [FromRoute] string newpassword)
        {
            try
            {
                var user = _dataService.getUser(username);
                //Kan ikke få lavet verification på password fordi det er hashet, så kan ikke sammenligne input med aktuel password
                //if (_hashing.verify(user.Username, user.Password, user.Salt))
                if(user != null)
                {
                    _dataService.updateUserPassword(username, _hashing.Hash(newpassword).hash);
                    return Ok(createUserModel(user));
                }
                else
                {
                    return BadRequest(user.Password);
                }
            }
            catch
            {
                return  BadRequest();
            }
        }

        [HttpPut ("{username}/{title}/{rating}", Name = nameof(createRating))]
        [Authorize]
        public IActionResult createRating([FromRoute] string username, [FromRoute] string title, [FromRoute] float rating)
        {
            try
            {
                var user = _dataService.getUser(username);
                var createrating = _dataService.createRating(username, title, rating);
                return Ok(createUserModel(user));
            }
            catch
            {
                return BadRequest();
            }
        }


        private UserModel createUserModel(UserPageDTO userpagedto)
        {
            var model = _mapper.Map<UserModel>(userpagedto);
            model.url = _generator.GetUriByName(HttpContext, nameof(getUser), new { userpagedto.Username });
            return model;
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginModel model)
        {
            var user = _dataService.getUser(model.username);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!_hashing.verify(model.password, user.Password, user.Salt))
            {
                return BadRequest();
            }

            var claims = new List<Claim>
            {
                //Payload - What has to be sent
                new Claim(ClaimTypes.Name, user.Username)
            };

            //Key to tokens - Is secure on server side - Better not to hardcode the key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Auth:secret").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                //Dont know if we want/need expiration for this type of project, but it makes testing annoying :)
                expires: DateTime.Now.AddSeconds(36000),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { user.Username, token = jwt });
        }

        //api/users?action=register
        [HttpPost("register")]
        public IActionResult Register(UserCreateModel model)
        {
            if (_dataService.getUser(model.Username) != null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                return BadRequest();
            }
            var hashResult = _hashing.Hash(model.Password);
            _dataService.createUser(model.Username, hashResult.hash, hashResult.salt);

            return Ok();
        }
    }
}
