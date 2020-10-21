using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MimicAPI.Helpers;
using MimicAPI.V1.Models;
using MimicAPI.V1.Models.DTO;
using MimicAPI.V1.Repositories.Contracts;
using Newtonsoft.Json;
using System;

namespace MimicAPI.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v1/[controller]")]
    public class WordsController : ControllerBase
    {

        private readonly IWordRepository _repository;
        private readonly IMapper _mapper;

        public WordsController(IWordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }


        [HttpGet]
        public IActionResult GetAll([FromQuery] WordQueryUrl query)
        {
            var item = _repository.GetAll(query);

            if (item.Results.Count == 0)
                return StatusCode(404);

            PaginationList<WordDTO> List = CreateLinksListWordDTO(query, item);

            return Ok(List);
        }



        //[HttpGet]
        //public IActionResult GetOne(int id)
        //{
        //    var obj = _repository.GetOne(id);
        //    if (obj == null)
        //        return StatusCode(404);

        //    WordDTO wordDTO = _mapper.Map<Word, WordDTO>(obj);

        //    wordDTO.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = wordDTO.Id }), "GET"));
        //    wordDTO.Links.Add(new LinkDTO("update", Url.Link("UpdateWord", new { id = wordDTO.Id }), "PUT"));
        //    wordDTO.Links.Add(new LinkDTO("delete", Url.Link("DeleteWord", new { id = wordDTO.Id }), "DELETE"));



        //    return Ok(wordDTO);
        //}


        //[Route("")]
        //[HttpPost]
        //public IActionResult Register([FromBody] Word Words)
        //{
        //    if (Words == null)
        //        return StatusCode(400);

        //    if (!ModelState.IsValid)
        //        return UnprocessableEntity(ModelState);


        //    Words.Status = true;
        //    Words.Creation = DateTime.Now;
        //    _repository.Register(Words);

        //    WordDTO wordDTO = _mapper.Map<Word, WordDTO>(Words);
        //    wordDTO.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = wordDTO.Id }), "GET"));

        //    return Created($"api/palavras/{Words.Id}", wordDTO);

        //}


        //[HttpPut]
        //public IActionResult Update(int id, [FromBody] Word Words)
        //{

        //    var obj = _repository.GetOne(id);
        //    if (obj == null)
        //        return StatusCode(404);

        //    if (Words == null)
        //        return StatusCode(400);

        //    if (!ModelState.IsValid)
        //        return UnprocessableEntity(ModelState);

        //    Words.Id = id;
        //    Words.Status = obj.Status;
        //    Words.Creation = obj.Creation;
        //    Words.Update = DateTime.Now;
        //    _repository.Update(Words);

        //    WordDTO wordDTO = _mapper.Map<Word, WordDTO>(Words);
        //    wordDTO.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = wordDTO.Id }), "GET"));

        //    return Ok(wordDTO);
        //}


        //[HttpDelete]
        //public IActionResult Delete(int id)
        //{

        //    var obj = _repository.GetOne(id);
        //    if (obj == null)
        //        return StatusCode(404);
        //    _repository.Delete(id);

        //    return StatusCode(204);
        //}


        private PaginationList<WordDTO> CreateLinksListWordDTO(WordQueryUrl query, PaginationList<Word> item)
        {
            var List = _mapper.Map<PaginationList<Word>, PaginationList<WordDTO>>(item);

            foreach (var word in List.Results)
            {
                word.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = word.Id }), "GET"));
            }

            List.Links.Add(new LinkDTO("self", Url.Link("GetAll", query), "GET"));

            if (item.Pagination != null)
            {
                Response.Headers.Add("x-pagination", JsonConvert.SerializeObject(item.Pagination));


                if (query.NumPage + 1 <= item.Pagination.TotalPages)
                {
                    var queryString = new WordQueryUrl() { NumPage = query.NumPage + 1, NumRecordsPage = query.NumRecordsPage, Date = query.Date };
                    List.Links.Add(new LinkDTO("next", Url.Link("GetAll", queryString), "GET"));
                }

                if (query.NumPage - 1 > 0)
                {
                    var queryString = new WordQueryUrl() { NumPage = query.NumPage - 1, NumRecordsPage = query.NumRecordsPage, Date = query.Date };
                    List.Links.Add(new LinkDTO("Prev", Url.Link("GetAll", queryString), "GET"));
                }
            }

            return List;
        }
    }
}

