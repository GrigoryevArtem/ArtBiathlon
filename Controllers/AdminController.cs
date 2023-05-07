using ArtBiathlon.DataEntity;
using ArtBiathlon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace ArtBiathlon.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class AdminController : Controller
    {
        ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // [HttpGet]
        // public IActionResult Question()
        // {
        //     var list = new List<HelpViewModel>();
        //
        //     list = (from help in _context.Helps
        //             select new HelpViewModel
        //             {
        //                 UserId = help.UserId,
        //                 Answer = help.Answer,
        //                 Question = help.Question,
        //                 Date = help.Date,
        //                 Id = help.Id,
        //                 UserFio = _context.Users.FirstOrDefault(user => user.Id == help.UserId).FIO
        //             }).OrderBy(x => x.Answer == null).ToList();
        //
        //     return View(list);
        // }

        [HttpPost]
        public async Task<IActionResult> Question(int id)
        {
            var model = await _context.Helps.FirstOrDefaultAsync(help => help.Id == id);
            return RedirectToAction("Answer", "Admin", model);
        }

        // [HttpGet]
        // public async Task<IActionResult> Answer(Help model)
        // {
        //     return model is null ? new EmptyResult() : View(model);
        // }

        [HttpPost]
        public async Task<IActionResult> Answer(int id, string answer)
        {
            var model = await _context.Helps.FirstOrDefaultAsync(help => help.Id == id);

            model.Answer = answer;

            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == model.UserId);

            var userEmail = user.Email;

            var sender = new MailSender("artbiathlon@mail.ru", userEmail, "ArtBiathlon");

            await sender.Send("Ответ на вопрос", $"Администратор дал ответ на ваш вопрос.\nВопрос: {model.Question}\nОтвет: {model.Answer}");

            _context.Helps.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Question", "Admin");
        }

        // [HttpGet]
        // public IActionResult Topic()
        // {
        //     var list = _context.MailingTopics.ToList();
        //     ViewBag.Users = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(list, "Id", "Title");
        //
        //     return View();
        // }

        [HttpPost]
        public async Task<IActionResult> Topic(int id, string message)
        {
            var indexes = _context.MailingTopicSubscribers
                .Where(topic => topic.MailingTopicId == id)
                .Select(topic => topic.UserId)
                .ToList();

            var topic = _context.MailingTopics.FirstOrDefault(topic => topic.Id == id)?.Title;

            var users = _context.Users.Where(user => indexes.Contains((int)user.Id)).ToList();

            await MailingTopics(users, topic, message);

            _context.Mailings.Add(new Mailing { Date = DateTime.Now, MailingTopicId = id, Message = message });
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [NonAction]
        private async Task MailingTopics(IEnumerable<User> users, string topic, string message)
        {
            foreach (var sender in from t in users where !string.IsNullOrEmpty(t.Email) select new MailSender("artbiathlon@mail.ru", t.Email, "ArtBiathlon"))
            {
                await sender.Send(topic, message);
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
            if (await _context.MailingTopics.FirstOrDefaultAsync(x => x.Title == newTopic) is not null)
                return RedirectToAction("Topic", "Admin");
            
            await _context.MailingTopics.AddAsync(new MailingTopic { Title = newTopic });
            await _context.SaveChangesAsync();

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