using System;
using AutoMapper;
using CIT_2022_Portfolio2.Models;
using DataLayer;
using DataLayer.DataTransferObjects;
using DataLayer.Models;
using DataLayer.Models.ObjectsFromFunctions;
using Microsoft.AspNetCore.Mvc;

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
                _dataService.getPersons()
                    .Select(x => createPersonModel(x)).ToList();
            return Ok(persons);
        }

        [HttpGet("{personId}", Name = nameof(getPerson))]
        public IActionResult getPerson(string personId)
        {
            var Person = _dataService.getPerson(personId);

            if (Person != null)
            {
                var model = createPersonModel(Person);

                return Ok(model);
            }
            return NotFound();
        }

        [HttpGet("{personName}", Name = nameof(getPersonName))]
        public IActionResult getPersonName(string personName)
        {
            var Person = _dataService.getPersonName(personName);

            if (Person != null)
            {
                var model = createPersonModel(Person);

                return Ok(model);
            }
            return NotFound();
        }

        [HttpGet("{personName}/CoActorPerson", Name = nameof(getCoActorPerson))]
        public IActionResult getCoActorPerson(string name)
        {
            var CoActorPersons = _dataService.getCoActors(name)
                .Select(createCoActorPersonModel);
            return Ok(CoActorPersons);
        }

        private PersonModel createPersonModel(PersonOnMainPageDTO personOnMainPageDTO)
        {
            var model = _mapper.Map<PersonModel>(personOnMainPageDTO);
            model.url = _generator.GetUriByName(HttpContext, nameof(getPerson), new { personOnMainPageDTO.PersonId });
            model.CoActorPersonsUrl = model.url + "/CoActorPerson";
            return model;
        }

        private CoActorPersonsModel createCoActorPersonModel(CoActor_Person coActorPerson)
        {
            var model = _mapper.Map<CoActorPersonsModel>(coActorPerson);
            model.url = _generator.GetUriByName(HttpContext, nameof(getPerson), new { coActorPerson.PersonId });
            return model;
        }

    }
}

