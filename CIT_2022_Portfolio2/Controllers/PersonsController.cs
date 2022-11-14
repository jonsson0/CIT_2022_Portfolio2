﻿using System;
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

        [HttpGet("{personId}/CoActorPerson", Name = nameof(getCoActorPersons))]
        public IActionResult getCoActorPersons(string personId)
        {
            var CoActorPersons = _dataService.getCoActors(personId)
                .Select(createCoActorPersonModel);
            return Ok(CoActorPersons);
        }

        private PersonModel createPersonModel(PersonOnMainPageDTO personOnMainPageDTO)
        {
            var model = _mapper.Map<PersonModel>(personOnMainPageDTO);
            model.url = _generator.GetUriByName(HttpContext, nameof(getPerson), new { personOnMainPageDTO.PersonId });
            return model;
        }

        private CoActorPersonModel createCoActorPersonModel(CoActor_Person coActorPerson)
        {
            var model = _mapper.Map<CoActorPersonModel>(coActorPerson);
            model.url = _generator.GetUriByName(HttpContext, nameof(getPerson), new { coActorPerson.PersonId });
            return model;
        }

    }
}

