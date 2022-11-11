using System;
using AutoMapper;
using CIT_2022_Portfolio2.Models;
using DataLayer;
using DataLayer.DataTransferObjects;
using DataLayer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CIT_2022_Portfolio2.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public PersonsController(IDataService dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }


        [HttpGet(Name = nameof(getPersons))]
        public IActionResult getPersons()
        {
            var persons =
                _dataService.getPerson()
                    .Select(x => createPersonModel(x)).ToList();
            return Ok(persons);
        }

        [HttpGet("{personId}", Name = nameof(getPerson))]
        public IActionResult getPerson(string personId)
        {
            var Person = _dataService.getPerson(personId);

            if (Person != null)
            {
                return Ok(Person);

            }
            return NotFound();
            var model = createPersonModel(Person);


        }

        private PersonModel createPersonModel(Person person)
        {
            var model = _mapper.Map<PersonModel>(person);
            model.url = _generator.GetUriByName(HttpContext, nameof(getPerson), new { person.PersonId });
            return model;
        }
    }
}

