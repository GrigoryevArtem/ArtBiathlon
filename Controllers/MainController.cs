using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtBiathlon.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        ApplicationDbContext _context;
       // UserViewModel _user;
        
        public MainController(ApplicationDbContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PersonalAccount()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name);
        
            ViewBag.TitlesDictionary = _context.MailingTopics.Join(_context.Mailings, t => t.Id, m => m.MailingTopicId, (t, m) => new
            {
                TitleTopic = t.Title,
                SubscribeCount = _context.Mailings.Count(x => x.MailingTopicId == t.Id),
                UsersCount = _context.MailingTopicSubscribers.Count(x => x.MailingTopicId == t.Id),
                Subscribers = _context.MailingTopicSubscribers.Where(x => x.MailingTopicId == t.Id).GroupBy(x => x.UserId).Count()
            }).ToDictionary(x => x.TitleTopic, x => (x.SubscribeCount, x.UsersCount, x.Subscribers));

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail()
        {
            string? message = Request.Form["message"];
            
            var mailSender = new MailSender("runapp90@mail.ru", "artbiathlon@mail.ru", "ArtBiathlon");

            await mailSender.Send("Kavo", message ?? "nope");
            return new EmptyResult();
        }

        [HttpGet]
        public IActionResult Subscribe()
        {
            var list = _context.MailingTopics.ToList();
            ViewBag.Titles = new MultiSelectList(list, "Id", "Title");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(int[] titles)
        {
            uint userId = _context.Users.FirstOrDefault(user => user.Login == User.Identity.Name).Id;
            var ids = _context.MailingTopicSubscribers
                .Where(x => x.UserId == userId)
                .Select(x => x.MailingTopicId)
                .ToList();

            var indexes = titles.Where(x => !ids.Contains(x)).ToList();

            foreach (var t in indexes)
            {
                _context.MailingTopicSubscribers.Add(new MailingTopicSubscriber { MailingTopicId = (int)t, UserId = (int)userId });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("PersonalAccount", "Main");
        }
    }
}
