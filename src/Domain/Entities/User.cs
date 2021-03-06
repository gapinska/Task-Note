using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string Bio { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Board> Boards { get; set; }
        public ICollection<BoardViewer> ViewBoards { get; set; }
        public ICollection<BoardEditor> EditBoards { get; set; }
    }
}