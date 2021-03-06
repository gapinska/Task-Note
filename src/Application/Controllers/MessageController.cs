using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models.Dtos;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<MessageHub> _messageHub;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public MessageController(IHubContext<MessageHub> messageHub, IMessageService messageService, IUserService userService, IMapper mapper)
        {
            _messageHub = messageHub;
            _messageService = messageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesAsync(int user1, int user2)
        {
            if ((user1 == 0 || user2 == 0) || (user1 == user2))
                return BadRequest("Invalid IDs provided by User.");

            var outcome = await _messageService.GetMessagesAsync(user1, user2);

            if (outcome == null)
                return BadRequest("No Users with provided IDs have been found.");

            return Ok(outcome.Select(p => _mapper.Map<MessageForGetDto>(p)));
        }

        [HttpPost]
        public async Task<IActionResult> Create(MessageDto messagePost)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var message = _mapper.Map<MessagePost>(messagePost);

            message.SenderId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!await _messageService.AddMessageAsync(message))
                return BadRequest($"No user with provided ID { message.ReceiverId } has been found.");

            await _messageHub.Clients
                            .User(messagePost.ReceiverId.ToString())
                            .SendAsync("sendMessage", message.SenderId, message.ReceiverId, message.Sent, message.Message);

            return NoContent();
        }
    }
}