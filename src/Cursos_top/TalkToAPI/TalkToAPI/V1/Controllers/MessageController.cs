using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Models.DTO;
using TalkToAPI.V1.Repositories.Contracts;

namespace TalkToAPI.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMapper mapper , IMessageRepository message)
        {
            _mapper = mapper;
            _messageRepository = message;
        }

        [Authorize]
        [HttpGet("{User1ID}/{User2Id}", Name = "Get")]
        public IActionResult GetAll(string user1Id, string user2Id)
        {
            if (user1Id == user2Id)
                return UnprocessableEntity();
            var messages = _messageRepository.GetAll(user1Id, user2Id);
            var listMessage = _mapper.Map<List<Message> , List<MessageDTO>>(messages);


            var list = new ListDTO<MessageDTO>() { List = listMessage };
            list.Links.Add(new LinkDTO("_self", Url.Link("GET" , new { user1Id = user1Id, user2Id = user2Id } ), "GET"));


            return Ok(listMessage);
        }

        [Authorize]
        [HttpPost(Name = "MessageRegister")]
        public IActionResult Register([FromBody] Message message)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _messageRepository.Register(message);

                    var MessageDTO = _mapper.Map<Message, MessageDTO>(message);
                    MessageDTO.Links.Add(new LinkDTO("_self", Url.Link("MessageRegister", null), "POST"));
                    MessageDTO.Links.Add(new LinkDTO("_PartialUpdate", Url.Link("PartialUpdate", new { id = message.Id}), "PATCH"));

                    return Ok(MessageDTO);
                }
                catch (Exception e)
                {
                    return UnprocessableEntity(e);
                }

            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        [Authorize]
        [HttpPatch("{id}", Name = "PartialUpdate")]
        public IActionResult PartialUpdate(int id, [FromBody]JsonPatchDocument<Message> jsonPatch)
        {
            if (jsonPatch == null)
                return BadRequest();

            var message = _messageRepository.GetOne(id);
            jsonPatch.ApplyTo(message);

            message.Update = DateTime.UtcNow;

            _messageRepository.Update(message);

            var MessageDTO = _mapper.Map<Message, MessageDTO>(message);
            MessageDTO.Links.Add(new LinkDTO("_self", Url.Link("PartialUpdate", new { id = message.Id }), "PATCH"));

            return Ok(MessageDTO);
        }
    }
}
