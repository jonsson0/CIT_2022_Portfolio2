using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection.Emit;
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
            return Ok(Paging(nameof(getTitles), page, pageSize, total, titles));
            }
            else
            {
                var titles = 
                    _dataService.getTitleByName(page, pageSize, search)
                    .Select(createPersonsSearchInListModel);
                var total = titles.Count();

                return Ok(Paging(nameof(getTitles), page, pageSize, total, titles));
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

            return Ok(Paging(nameof(getSimilarTitles), page, pageSize, total, similarTitles));
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
            model.url = _generator.GetUriByName(HttpContext, nameof(getTitle), new { similarTitle.TitleId });
            return model;
        }

        private TitleSearchInListModel createPersonsSearchInListModel(TitleSearchInListDTO titleSearchInListDTO)
        {
            var model = _mapper.Map<TitleSearchInListModel>(titleSearchInListDTO);
            model.url = _generator.GetUriByName(HttpContext, nameof(getTitle), new { titleSearchInListDTO.TitleId });
            return model;
        }

        // paging

        private const int MaxPageSize = 25;

        private string? CreateGetTilesLink(int page, int pageSize)
        {
            return CreateLink(nameof(getTitles), new { page, pageSize });
        }

        private string? CreateLink(string endpointName, object? values)
        {
            return _generator.GetUriByName(HttpContext, endpointName,values);
        }

        private object Paging<T>(string endpointName, int page, int pageSize, int total, IEnumerable<T> items)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            //if (pageSize > MaxPageSize)
            //{
            //    pageSize = MaxPageSize;
            //}

            var pages = (int)Math.Ceiling((double)total / (double)pageSize);

            var first = total > 0
                ? CreateLink(endpointName, new { page, pageSize })
                : null;

            var prev = page >  0
                ? CreateLink(endpointName, new { page = page -1, pageSize })
                : null;

            var current = CreateLink(endpointName, new { page, pageSize });

            var next = page < pages - 1
                ? CreateLink(endpointName, new { page = page + 1, pageSize })
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