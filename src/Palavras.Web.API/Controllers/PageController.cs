using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using Services;
namespace Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PageController : ControllerBase
    {

        [HttpPost]
        [Route("Store")]
        public Page Store(Page page)
        {
            page.Datahora = DateTime.Now;
 
            var client = ElasticService.GetClient("bruto");

            var indexResponse = client.IndexDocument(page);

            return page;

        }

        [HttpGet]
        [Route("GetUltimaAtualizacao")]
        public string GetUltimaAtualizacao()
        {

            var client = ElasticService.GetClient("bruto");

            var searchResponse = client.Search<Page>(s => s
                .From(0)
                .Take(1)
                .Sort(sort =>
                    sort.Descending(f => f.Datahora)
    )
            );

            var page = searchResponse.Documents.First();

            return page.Datahora.ToString("dd/MM/yyyy HH:mm");

        }

        [HttpGet]
        [Route("Get")]
        public IEnumerable<Page> Get()
        {

 
            var client = ElasticService.GetClient("bruto");

            var searchResponse = client.Search<Page>(s => s
                .From(0)
            );

            var pages = searchResponse.Documents;

            return pages;

        }

        [HttpGet]
        [Route("GetGroups")]
        public List<PageWordCount> GetGroups()
        {


            List<PageWordCount> result = new List<PageWordCount>();

            var client = ElasticService.GetClient("bruto");

            var searchResponse = client.Search<Page>(s => s
                .From(0)
            );

            var pages = (List<Page>)searchResponse.Documents;

            result = PageService.ProcessaAgrupamentoBySite(pages);


            return result;

        }

        [HttpGet]
        [Route("GetRank")]
        public List<WordCount> GetRank()
        {


            List<WordCount> result = new List<WordCount>();

            var client = ElasticService.GetClient("bruto");

            var searchResponse = client.Search<Page>(s => s
                .From(0)
            );

            var pages = (List<Page>)searchResponse.Documents;

            result = PageService.ProcessaAgrupamento(pages);


            return result;

        }

        [HttpGet]
        [Route("GetRankThisMonth")]
        public List<WordCount> GetRankThisMonth()
        {
            return new List<WordCount>() { };
        }

        [HttpGet]
        [Route("GetRankThisWeek")]
        public List<WordCount> GetRankThisWeek()
        {
            return new List<WordCount>() { };
        }
    }
}