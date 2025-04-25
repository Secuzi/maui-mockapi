using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMockAPI.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string CreatedAt { get; set; }
        public string Name { get; set; }
        public string CommitMessage { get; set; }
        public string Branch { get; set; }
        public string Avatar { get; set; }

    }
}
