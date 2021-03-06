using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMessageRepository
    {
        Task<bool> AddMessageAsync(MessagePost messagePost);
        Task<List<MessagePost>> GetMessagesAsync(int idUser1, int idUser2);
    }
}
