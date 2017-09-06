using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace todolistwithauth.Models
{
    public class ToDoModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public bool? Complete { get; set; } = false;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime CompleteDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public void CompleteTask()
        {
            Complete = true;
            CompleteDate = DateTime.Now;
        }
    }
}