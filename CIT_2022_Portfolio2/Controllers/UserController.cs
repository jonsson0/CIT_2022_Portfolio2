using AutoMapper;
using CIT_2022_Portfolio2.Models;
using DataLayer;
using DataLayer.DataTransferObjects;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIT_2022_Portfolio2.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(IDataService dataService, LinkGenerator generator, IMapper mapper, IConfiguration configuration)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet ("{username}", Name = nameof(getUser))]
        public IActionResult getUser()
        {
            try
            {
                //HardCoded User
                var username = _configuration.GetSection("Auth:Username").Value;
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

        [HttpPut("{username}/bookmarkperson", Name = nameof(inputBookmarkPerson))]
        public IActionResult inputBookmarkPerson ()
        {
            try
            {
                var username = _configuration.GetSection("Auth:Username").Value;
                var user = _dataService.getUser(username).Username;
                var person = _dataService.getPerson("nm0000002").PersonId;
                var bookmark = _dataService.createBookmarkPerson(username, person);

                return Ok(bookmark);
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPut("{username}/bookmarktitle", Name = nameof(inputBookmarkTitle))]
        public IActionResult inputBookmarkTitle ()
        {
            try
            {
                var username = _configuration.GetSection("Auth:Username").Value;
                var title = _dataService.getTitle("tt0052520").TitleId;
                var bookmark = _dataService.createBookmarkTitle(username, title);

                return Ok(bookmark);
            }
            catch
            {
                return Unauthorized();
            }
        }

        //Delete user
        [HttpDelete]

        private UserModel createUserModel(UserPageDTO userpagedto)
        {
            var model = _mapper.Map<UserModel>(userpagedto);
            model.url = _generator.GetUriByName(HttpContext, nameof(getUser), new {userpagedto.Username});
            return model;
        }


    }
}
