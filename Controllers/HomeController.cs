using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TheWall.Models;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace TheWall.Controllers
{
    public class HomeController : Controller
    {
        private YourContext _context;
        public HomeController(YourContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("wall")]
        public IActionResult Wall()
        {
            ViewModel view = new ViewModel()
            {
                Users = new User(),
                Comments = new Comment(),
                Messages = new Message(),
            };
            List<Message> allMessages = _context.Messages.Include(m => m.User).ToList();
            List<Comment> allComments = _context.Comments.Include(m => m.User).ToList();
            int? user_id = HttpContext.Session.GetInt32("id");
            User CurrentUser = _context.Users.SingleOrDefault(u => u.Id == user_id);
            User currentuser = _context.Users
                                .Include(user => user.Messages)
                                .Where(user => user.Id == user_id).SingleOrDefault();
            foreach(var i in allMessages){
              i.Ago = i.Created_At.TimeAgo();
            }
            foreach(var i in allComments){
              i.Ago = i.Created_At.TimeAgo();
            }
            ViewBag.User = currentuser;
            ViewBag.Messages = allMessages.OrderByDescending(m => m.Created_At);
            ViewBag.Comments = allComments.OrderByDescending(c => c.Ago);
            return View(view);
        }

        [HttpPost]
        [Route("wall/post")]
        public IActionResult PostMessage(Message message)
        {
            if(ModelState.IsValid) {
                int? user_id = HttpContext.Session.GetInt32("id");
                User CurrentUser = _context.Users.SingleOrDefault(user => user.Id == user_id);
                message.User = CurrentUser;
                System.Console.WriteLine(message);
                _context.Add(message);
                _context.SaveChanges();
                return RedirectToAction("Wall");
            }
            return RedirectToAction("Wall"); 
        }

        [HttpPost]
        [Route("wall/comment")]
        public IActionResult PostComment(Comment comment)
        {
            if(ModelState.IsValid) {
                int? user_id = HttpContext.Session.GetInt32("id");
                int MessageId = Int32.Parse(Request.Form["messageid"]);
                User CurrentUser = _context.Users.SingleOrDefault(user => user.Id == user_id);
                comment.User = CurrentUser;
                comment.MessageId = MessageId;
                _context.Add(comment);
                _context.SaveChanges();
                return RedirectToAction("Wall");
            }
            return RedirectToAction("Wall"); 
        }

        [HttpGet]
        [Route("Delete/{MessageId}")]
        public IActionResult Delete(int MessageId)
        {
            if(HttpContext.Session.GetInt32("id") == null) {
                return RedirectToAction("Index", "User");
            }
            Message thisMessage = _context.Messages
                            .Where(m => m.Id == MessageId).SingleOrDefault();
            _context.Messages.Remove(thisMessage);
            _context.SaveChanges();
            return RedirectToAction("Wall");
        }

        [HttpGet]
        [Route("Delete/Comment/{CommentId}")]
        public IActionResult DeleteComment(int CommentId)
        {
            if(HttpContext.Session.GetInt32("id") == null) {
                return RedirectToAction("Index", "User");
            }
            Comment thisComment = _context.Comments
                            .Where(c => c.Id == CommentId).SingleOrDefault();
            _context.Comments.Remove(thisComment);
            _context.SaveChanges();
            return RedirectToAction("Wall");
        }
    }
}
