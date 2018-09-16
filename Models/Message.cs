using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TheWall.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Message field must not be empty.")]
        
        [Column("message")]
        public string Context { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

        [Column("user_id")]
        public User User { get; set; }
        public List<Comment> Comments { get; set; }
        
        [NotMapped]
        public string Ago {get; set;}
        public Message()
        {
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
            List<Comment> Comments = new List<Comment>();
        }
    }   
}