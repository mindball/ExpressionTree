using CodeItUp.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeItUp.Controllers
{
    public class AnotherController : Controller
    {
        public IActionResult About()
        {
            int id = 5;
            string query = "alabala";

            return this.RedirectTo<HomeController>(c => c.Index(id, query));
        }
    }
}
