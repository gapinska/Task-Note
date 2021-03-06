using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserService _userService;

        public MessageService(IMessageRepository messageRepository, IUserService userService)
        {
            _messageRepository = messageRepository;
            _userService = userService;
        }
        public async Task<bool> AddMessageAsync(MessagePost messagePost)
        {
            if (await _userService.GetUserByIdAsync(messagePost.ReceiverId) == null)
                return false;

            return await _messageRepository.AddMessageAsync(messagePost);
        }

        public async Task<List<MessagePost>> GetMessagesAsync(int idUser1, int idUser2)
        {
            if (await _userService.GetUserByIdAsync(idUser1) == null || await _userService.GetUserByIdAsync(idUser2) == null)
                return null;
            return await _messageRepository.GetMessagesAsync(idUser1, idUser2);
        }
    }
}
