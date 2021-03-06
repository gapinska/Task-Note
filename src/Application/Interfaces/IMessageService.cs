using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMessageService
    {
        Task<bool> AddMessageAsync(MessagePost messagePost);
        Task<List<MessagePost>> GetMessagesAsync(int idUser1, int idUser2);
    }
}
