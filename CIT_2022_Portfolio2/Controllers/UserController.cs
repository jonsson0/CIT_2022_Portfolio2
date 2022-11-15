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
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public UserController(IDataService dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet ("{username}", Name = nameof(getUser))]
        public IActionResult getUser(string username)
        {
            var user = _dataService.getUser(username);

            if (user != null)
            {
                var model = createUserModel(user);
                return Ok(model);
            }
            else { return NotFound(); }
            
        }

        [HttpPut("{username}/bookmarkperson", Name = nameof(inputBookmarkPerson))]
        public IActionResult inputBookmarkPerson (string username, string personname)
        {
            var bookmark = _dataService.createBookmarkPerson(username, personname);
           
            return Ok(bookmark);
        }

        [HttpPut("{username}/bookmarktitle", Name = nameof(inputBookmarkTitle))]
        public IActionResult inputBookmarkTitle (string username, string titlename)
        {
            var bookmark = _dataService.createBookmarkTitle(username, titlename);

            return Ok(bookmark);
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
