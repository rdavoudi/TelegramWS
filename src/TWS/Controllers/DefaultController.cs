using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TWS.Controllers
{
    
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return Json("It is working properly");
        }
    }
}
