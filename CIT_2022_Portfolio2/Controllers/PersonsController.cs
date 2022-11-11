using System;
using AutoMapper;
using CIT_2022_Portfolio2.models;
using DataLayer;
using DataLayer.DataTransferObjects;
using DataLayer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CIT_2022_Portfolio2.Controllers
{
    [Route("api/persons")]

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


        //[HttpGet(Name = nameof(getPerson))]
        //public IActionResult getPerson()
        //{
        //    var persons =
        //        _dataService.getPerson()
        //            .Select(x => createPersonModel(x)).ToList();
        //    return Ok(persons);
        //}

        //private PersonModel createPersonModel(Person name)
        //{
        //    var model = _mapper.Map<PersonModel>(name);
        //    model.url = _generator.GetUriByName(HttpContext, nameof(getPerson), new { title.TitleId });
        //    return model;
        //}



    }
}

