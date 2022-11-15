using System.Numerics;
using AutoMapper;
using CIT_2022_Portfolio2.Models;
using DataLayer;
using DataLayer.DataTransferObjects;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CIT_2022_Portfolio2.Controllers
{
    [Route("api/titles")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public TitleController(IDataService dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet (Name = nameof(getTitles))]
        public IActionResult getTitles(int page = 0, int pageSize = 10, string? search = null)
        {
            if (string.IsNullOrEmpty(search))
            {
            var titles =
                _dataService.getTitles(page, pageSize)
                    .Select(x => createTitleModel(x));
            var total = _dataService.GetNumberOfTitles();
            return Ok(Paging(page, pageSize, total, titles));
            }
            else
            {
                var data = _dataService.getTitleByName(search);
                return Ok(data);
            }

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

        [HttpGet("{titleId}/similartitles", Name = nameof(getSimilarTitles))]
        public IActionResult getSimilarTitles(string titleId, int page, int pageSize)
        {
            var similarTitles = _dataService.getSimilarTitles(titleId, 0, 10)
                .Select(x => createSimilarTitleModel(x)).ToList();
           
            var total = similarTitles.Count();

            return Ok(Paging(page, pageSize, total, similarTitles));
        }

        private TitleModel createTitleModel(TitleOnMainPageDTO titleOnMainPageDTO)
        {
            var model = _mapper.Map<TitleModel>(titleOnMainPageDTO);
            model.url = _generator.GetUriByName(HttpContext, nameof(getTitle), new { titleOnMainPageDTO.TitleId });
           // model.SimilarTitlesUrl = model.url + "/similartitles";
            model.SimilarTitlesUrl = _generator.GetUriByName(HttpContext, nameof(getSimilarTitles), new { titleOnMainPageDTO.TitleId });

            return model;
        }


        private SimilarTitleModel createSimilarTitleModel(Similar_Title similarTitle)
        {
            var model = _mapper.Map<SimilarTitleModel>(similarTitle);
            model.url = _generator.GetUriByName(HttpContext, nameof(getSimilarTitles), new { similarTitle.TitleId });
            model.url = model.url.Replace("/similartitles", "");
            return model;
        }

        // paging

        private const int MaxPageSize = 25;

        private string? CreateLink(int page, int pageSize)
        {
            return _generator.GetUriByName(
                HttpContext,
                nameof(getTitles), new { page, pageSize });
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