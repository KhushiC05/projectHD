using HelpDeskManagement.Models;
using HelpDeskManagement.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskManagement.Controllers
{
    public class AdminController : Controller
    {

        private readonly HelpDeskDBContext _context;

        public AdminController(HelpDeskDBContext context)
        {
            _context = context;
        }

        public IActionResult AdminHome()
        {
            int UserID = Convert.ToInt32( HttpContext.Session.GetString("UserId"));
            if(UserID == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _context.UserMsts.Find(UserID);
            return View(user);
        }

        public IActionResult ListTickets(string status)
        {
            var ticketList = new List<TicketDetail>();

            if (status == null)
            {
                ticketList = _context.TicketDetails.ToList();
               
            }
            else
            {
                ticketList = _context.TicketDetails.Where(t => t.TicketStatus == status).ToList();
               
            }

            var userlist = _context.UserMsts.ToList();
            ViewBag.userlist = userlist;


            //List<TicketDetail> list = new List<TicketDetail>();

            //foreach(TicketDetail t in ticketList)
            //{
            //    var username = userlist.FirstOrDefault(x => x.Id == t.CreatedBy).UserName;
            //    //t.CreatedByName = username;
            //}
            return View(ticketList);
        }


        public IActionResult EditProfile()
        {
            int UserID = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            if (UserID == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _context.UserMsts.Find(UserID);
            return View(user);
        }

        public IActionResult UserDetailScreen()
        {
            var userList = _context.UserMsts.ToList();
            return View(userList);
        }

    

        public IActionResult EditTicketStatus(int TId)
        {
            var ticketdetails = _context.TicketDetails.FirstOrDefault(x => x.Id == TId);

            return View(ticketdetails);
        }


        [HttpPost]
        public IActionResult TicketStatusChange([FromBody] TicketDetail model)
        {

            var ticket = _context.TicketDetails.FirstOrDefault(x => x.Id == model.Id);


            ticket.Issue = model.Issue;
            ticket.Attachment = model.Attachment;
            ticket.Urgency = model.Urgency;
            ticket.Category = model.Category;
            ticket.ProblemDescription = model.ProblemDescription;
            ticket.TicketStatus = model.TicketStatus;
            _context.SaveChanges();
            return Ok(ticket);

        }


        [HttpGet]
        public IActionResult getCount()
        {
            int p = _context.TicketDetails.Where(t => t.TicketStatus == "Pending").ToArray().Length;
            int o = _context.TicketDetails.Where(t => t.TicketStatus == "OnGoing").ToArray().Length;
            int  c= _context.TicketDetails.Where(t => t.TicketStatus == "Complete").ToArray().Length;


            CountRes cnt = new CountRes();
            cnt.pending = p;
            cnt.ongoing = o;
            cnt.complete = c;

            return Ok(cnt);
        }

        public IActionResult UserTickets(int Uid)
        {
            var ticketList = _context.TicketDetails.Where(t => t.CreatedBy == Uid).ToList();

            var userlist = _context.UserMsts.ToList();
            ViewBag.userlist = userlist;


            return View(ticketList);
        }
    }
}
