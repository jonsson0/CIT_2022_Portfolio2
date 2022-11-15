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
    public class PersonController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public PersonController(IDataService dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }


        [HttpGet(Name = nameof(getPersons))]
        public IActionResult getPersons(int page = 0, int pageSize = 10, string? search = null)
        {
            if (string.IsNullOrEmpty(search))
            {
                var persons =
                _dataService.getPersons(page, pageSize)
                    .Select(x => createPersonModel(x));
                var total = _dataService.GetNumberOfPersons();
                return Ok(Paging(page, pageSize, total, persons));
            }
            else
            {
                var data = _dataService.getPersonByName(search);
                return Ok(data);
            }

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

        //[HttpGet("{personId}", Name = nameof(getPersonName))]
        //public IActionResult getPersonName(string personName)
        //{
        //    var Person = _dataService.getPersonName(personName);

        //    if (Person != null)
        //    {
        //        var model = createPersonModel(Person);

        //        return Ok(model);
        //    }
        //    return NotFound();
        //}

        [HttpGet("{id}/CoActors", Name = nameof(getCoActors))]
        public IActionResult getCoActors(string id)
        {
            var CoActorPersons = _dataService.getCoActors(id)
                .Select(createCoActorModel);
            return Ok(CoActorPersons);
        }

        private PersonModel createPersonModel(PersonOnMainPageDTO personOnMainPageDTO)
        {
            var model = _mapper.Map<PersonModel>(personOnMainPageDTO);
            model.url = _generator.GetUriByName(HttpContext, nameof(getPerson), new { personOnMainPageDTO.PersonId });
            model.CoActorPersonsUrl = model.url + "/CoActors";
            return model;
        }

        private CoActorModel createCoActorModel(CoActor coActor)
        {
            var model = _mapper.Map<CoActorModel>(coActor);
            model.url = _generator.GetUriByName(HttpContext, nameof(getCoActors), new { coActor.PersonId });
            return model;
        }

        private const int MaxPageSize = 25;

        private string? CreateLink(int page, int pageSize)
        {
            return _generator.GetUriByName(
                HttpContext,
                nameof(getPersons), new { page, pageSize });
        }

        private object Paging<T>(int page, int pageSize, int total, IEnumerable<T> items)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            //if (pageSize > MaxPageSize)
            //{
            //    pageSize = MaxPageSize;
            //}

            var pages = (int)Math.Ceiling((double)total / (double)pageSize)
                ;

            var first = total > 0
                ? CreateLink(0, pageSize)
                : null;

            var prev = page > 0
                ? CreateLink(page - 1, pageSize)
                : null;

            var current = CreateLink(page, pageSize);

            var next = page < pages - 1
                ? CreateLink(page + 1, pageSize)
                : null;

            var result = new
            {
                first,
                prev,
                next,
                current,
                total,
                pages,
                items
            };
            return result;
        }

    }
}

