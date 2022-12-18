using System;
using AutoMapper;
using CIT_2022_Portfolio2.Models;
using DataLayer;
using DataLayer.DataTransferObjects;
using DataLayer.Models;
using DataLayer.Models.ObjectsFromFunctions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
                    .Select(createPersonModel);
                var total = _dataService.GetNumberOfPersons();
                return Ok(Paging(nameof(getPersons), page, pageSize, total, persons, search));
            }
            else
            {
                var persons =
                        _dataService.getPersonsByNamePaging(page, pageSize, search)
                        .Select(createPersonsSearchInListModel);
                var total = _dataService.getPersonsByName(page, pageSize, search).Count();
                return Ok(Paging(nameof(getPersons), page, pageSize, total, persons, search));
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

        [HttpGet("{id}/CoActors", Name = nameof(getCoActors))]
        public IActionResult getCoActors(string id, int page = 0, int pageSize = 10)
        {
            var CoActorPersons = _dataService.getCoActors(id, page, pageSize)
                .Select(createCoActorModel);
            var total = CoActorPersons.Count();

            return Ok(Paging(nameof(getCoActors), page, pageSize, total, CoActorPersons));
        }

        private PersonModel createPersonModel(PersonOnMainPageDTO personOnMainPageDTO)
        {
            var model = _mapper.Map<PersonModel>(personOnMainPageDTO);
            model.url = _generator.GetUriByName(HttpContext, nameof(getPerson), new { personOnMainPageDTO.PersonId });
            model.CoActorsUrl = model.url + "/CoActors";
            //model.CoActorsUrl = _generator.GetUriByName(HttpContext, nameof(getCoActors), new { personOnMainPageDTO.PersonId });
            return model;
        }

        private CoActorModel createCoActorModel(CoActor coActor)
        {
            var model = _mapper.Map<CoActorModel>(coActor);
            model.url = _generator.GetUriByName(HttpContext, nameof(getPerson), new { coActor.PersonId });

            return model;
        }

        private PersonsSearchInListModel createPersonsSearchInListModel(PersonsSearchInListDTO personsSearchInListDTO)
        {
            var model = _mapper.Map<PersonsSearchInListModel>(personsSearchInListDTO);
            model.url = _generator.GetUriByName(HttpContext, nameof(getPerson), new { personsSearchInListDTO.PersonId });
            return model;
        }


        private const int MaxPageSize = 25;

        private string? CreateGetTilesLink(int page, int pageSize)
        {
            return CreateLink(nameof(getPersons), new { page, pageSize });
        }

        private string? CreateLink(string endpointName, object? values)
        {
            return _generator.GetUriByName(HttpContext, endpointName, values);
        }

        private object Paging<T>(string endpointName, int page, int pageSize, int total, IEnumerable<T> items, string search = null)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            //if (pageSize > MaxPageSize)
            //{
            //    pageSize = MaxPageSize;
            //}

            var pages = (int)Math.Ceiling((double)total / (double)pageSize);

            var first = total > 0
                ? CreateLink(endpointName, new { page = 0, pageSize, search })
                : null;

            var prev = page > 0
                ? CreateLink(endpointName, new { page = page - 1, pageSize, search })
                : null;

            var current = CreateLink(endpointName, new { page, pageSize, search });

            var next = page < pages - 1
                ? CreateLink(endpointName, new { page = page + 1, pageSize, search })
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

