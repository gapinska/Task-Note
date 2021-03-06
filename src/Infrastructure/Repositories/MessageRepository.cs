using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;

        public MessageRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> AddMessageAsync(MessagePost messagePost)
        {
            messagePost.Sent = DateTime.Now;

            await _context.Messages.AddAsync(messagePost);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public async Task<List<MessagePost>> GetMessagesAsync(int idUser1, int idUser2)
        {
            List<MessagePost> outcome = new List<MessagePost>();

            outcome = await _context.Messages
                                    .OrderBy(m => m.Sent)
                                    .Where(m => (m.SenderId == idUser1 && m.ReceiverId == idUser2) 
                                                ||
                                                (m.ReceiverId == idUser1 && m.SenderId == idUser2)).ToListAsync();

            return outcome;
        }
    }
}
