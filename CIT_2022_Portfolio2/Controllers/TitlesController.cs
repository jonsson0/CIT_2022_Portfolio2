using AutoMapper;
using CIT_2022_Portfolio2.Models;
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
                _dataService.getTitles()
                    .Select(x => createTitleModel(x)).ToList();
            return Ok(titles);
        }

        [HttpGet("{titleId}", Name = nameof(getTitle))]
        public IActionResult getTitle(string titleId)
        {
            var title = _dataService.getTitle(titleId);

            if (title != null)
            {
                var model = createTitleModel(title);

                return Ok(model);
            }
            return NotFound();
        }

        [HttpGet("{titleId}/similartitles", Name = nameof(getSimilarTitle))]
        public IActionResult getSimilarTitle(string titleId)
        {
            var similarTitles = _dataService.getSimilarTitles(titleId)
                .Select(createSimilarTitleModel);
            return Ok(similarTitles);
        }

        private TitleModel createTitleModel(TitleOnMainPageDTO titleOnMainPageDTO)
        {
            var model = _mapper.Map<TitleModel>(titleOnMainPageDTO);
            model.url = _generator.GetUriByName(HttpContext, nameof(getTitle), new { titleOnMainPageDTO.TitleId });
            return model;
        }

        private SimilarTitlesModel createSimilarTitleModel(Similar_Title similarTitle)
        {
            var model = _mapper.Map<SimilarTitlesModel>(similarTitle);
            model.url = _generator.GetUriByName(HttpContext, nameof(getTitle), new { similarTitle.TitleId });
            return model;
        }
    }
}