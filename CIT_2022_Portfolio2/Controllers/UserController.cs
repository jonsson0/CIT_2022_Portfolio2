﻿using AutoMapper;
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
        [Route("{username}/bookmarkperson/{person}")]
        [Authorize]
        public IActionResult inputBookmarkPerson([FromRoute] string username, [FromRoute] string person)
        {
            try
            {
                if (username == _configuration.GetSection("Auth:Username").Value)
                {
                    //var user = _dataService.getUser(username).Username;
                    //var person = _dataService.getPerson("nm0000002").PersonId;
                    var bookmark = _dataService.createBookmarkPerson(username, person);

                    return Ok(bookmark);
                }
                else { return Unauthorized(); }
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("{username}/bookmarktitle/{title}")]
        [Authorize]
        public IActionResult inputBookmarkTitle([FromRoute] string username, [FromRoute] string title)
        {
            try
            {
                username = _configuration.GetSection("Auth:Username").Value;
                //title = _dataService.getTitle(title).TitleId;
                //var user = _dataService.getUser(username);
                var bookmark = _dataService.createBookmarkTitle(username, title);

                return Ok(bookmark);
            }
            catch
            {
                return Unauthorized();
            }
        }

        //Delete user
        [HttpDelete ("{username}/delete/{password}", Name = nameof(DeleteUser))]
        [Authorize]
        public IActionResult DeleteUser([FromRoute] string username, [FromRoute] string password)
        {
            try
            {
                var deleteuser = _dataService.deleteUser(username, password);
                return Ok();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPut ("{username}/{title}/{rating}", Name = nameof(createRating))]
        [Authorize]
        public IActionResult createRating([FromRoute] string username, [FromRoute] string title, [FromRoute] float rating)
        {
            try
            {
                var createrating = _dataService.createRating(username, title, rating);
                return Ok(createrating);
            }
            catch
            {
                return Unauthorized();
            }
        }

        //Mangler create user og update user password

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
                expires: DateTime.Now.AddSeconds(30),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new {user.Username, token = jwt});
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
