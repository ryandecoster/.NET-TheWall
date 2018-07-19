using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TheWall.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Comment field must not be empty.")]
        public string Content { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public User User { get; set; }
        public Message Message { get; set; }
        public int UserId { get; set; }
        public int MessageId { get; set; }
        public Comment()
        {
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
}