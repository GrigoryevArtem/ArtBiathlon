using ArtBiathlon.DataEntity;
using ArtBiathlon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtBiathlon.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class SupportController : Controller
    {
        ApplicationDbContext _context;

        public SupportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Question()
        {
            var list = new List<HelpViewModel>();

            list = (from help in _context.Helps
                    select new HelpViewModel
                    {
                        UserId = help.UserId,
                        Answer = help.Answer,
                        Question = help.Question,
                        Date = help.Date,
                        Id = help.Id,
                        UserFio = _context.Users.FirstOrDefault(user => user.Id == help.UserId).FIO
                    }).OrderBy(x => x.Answer == null).ToList();

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Question(int id)
        {
            var model = await _context.Helps.FirstOrDefaultAsync(help => help.Id == id);
            return RedirectToAction("Answer", "Support", model);
        }

        [HttpGet]
        public async Task<IActionResult> Answer(Help model)
        {
            return model is null ? new EmptyResult() : View(model);
        }

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

            return RedirectToAction("Question", "Support");
        }
    }
}
