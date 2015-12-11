using MessageBoard.Controllers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MessageBoard.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBoard.Data;

namespace MessageBoard.Controllers
{
    public class HomeController : Controller
    {
        private IMailService _mail;
        private IMessageBoardRepository _repo;

        public HomeController(IMailService mail, IMessageBoardRepository repo)
        {
            _mail = mail;
            _repo = repo;
        }

        public HomeController()
        {
        }

        public ActionResult Index()
        {
            var topics = _repo.GetTopics()
                        .OrderByDescending(t => t.Created)
                        .Take(25)
                        .ToList();//iquery becomes ienum
            return View(topics);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            var msg = string.Format("New Comment: {1}{0}From: {2}{0}Website: {3}{0}", Environment.NewLine, model.Comment, model.Name, model.Website);

            //var svc = new MailService();

            var sent = _mail.SendMail("noreply@yourdomain.com", "comments@yourdomain.com", "Webste Comment", msg);
            ViewBag.MailSent = sent;
            return View();
        }
        //[Authorize] google openid loginInfo null problem
        public ActionResult MyMessages()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Moderation()
        {
            return View();
        }
    }
}