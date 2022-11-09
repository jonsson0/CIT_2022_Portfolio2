using AutoMapper;
using CIT_2022_Portfolio2.models;
using DataLayer;
using DataLayer.DataTransferObjects;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CIT_2022_Portfolio2.Controllers
{
    [Route("api/titles")]
    [ApiController]
    public class TitlesController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public TitlesController(IDataService dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet (Name = nameof(getTitles))]
        public IActionResult getTitles()
        {
            var titles =
                _dataService.getTitles().Select(x => createTitleModel(x));
            return Ok(titles);
        }

        [HttpGet("{titleId}", Name = nameof(getTitle))]
        public IActionResult getTitle(string titleId)
        {
            var title = _dataService.getTitle(titleId);

            if (title == null)
            {
                return NotFound();
            }

            var model = createTitleModel(title);

            return Ok(title);

        }

        private TitleModel createTitleModel(TitleOnMainPageDTO title)
        {
            var model = _mapper.Map<TitleModel>(title);
            model.url = _generator.GetUriByName(HttpContext, nameof(getTitle), new { title.TitleId });
            return model;
        }

    }
}
