using ArtBiathlon.DataEntity;
using ArtBiathlon.Models;
using ArtBiathlon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtBiathlon.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class MailingController : Controller
    {
        private readonly IMailingService _mailingService;
        ApplicationDbContext _context;
        public MailingController(IMailingService mailingService)
        {
            _mailingService = mailingService;
        }

        [HttpGet]
        public async Task<IActionResult> Topic()
        {
            var list = _mailingService.GetAll().Result.Data;
            ViewBag.Mailings = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(list, "Id", "Title");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Topic(int id, string message)
        {
            var indexes = _context.MailingTopicSubscribers
                .Where(topic => topic.MailingTopicId == id)
                .Select(topic => topic.UserId)
                .ToList();

            var topic = _context.MailingTopics.FirstOrDefault(topic => topic.Id == id).Title;

            var users = _context.Users.Where(user => indexes.Contains((int)user.Id)).ToList();

            await MailingTopics(users, topic, message);

            _context.Mailings.Add(new Mailing { Date = DateTime.Now, MailingTopicId = id, Message = message });
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [NonAction]
        private async Task MailingTopics(List<User> users, string topic, string message)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (!string.IsNullOrEmpty(users[i].Email))
                {
                    var sender = new MailSender("artbiathlon@mail.ru", users[i].Email, "ArtBiathlon");
                    await sender.Send(topic, message);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topic = await _context.MailingTopics.FirstOrDefaultAsync(x => x.Id == id);
            _context.MailingTopics.Remove(topic);
            await _context.SaveChangesAsync();

            return RedirectToAction("Topic", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> AddTopic(string newTopic)
        {

            if (await _context.MailingTopics.FirstOrDefaultAsync(x => x.Title == newTopic) is null)
            {
                await _context.MailingTopics.AddAsync(new MailingTopic { Title = newTopic });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Topic", "Admin");

        }

        [HttpPost]
        public async Task<IActionResult> UpdateTopic(int id, string updateTopic)
        {
            var topic = await _context.MailingTopics.FirstOrDefaultAsync(x => x.Id == id);
            topic.Title = updateTopic;
            _context.MailingTopics.Update(topic);
            await _context.SaveChangesAsync();

            return RedirectToAction("Topic", "Admin");
        }
    }
}
