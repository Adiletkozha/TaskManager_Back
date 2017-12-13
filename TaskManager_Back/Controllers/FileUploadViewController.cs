using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskManager_Back.Controllers
{
    public class FileUploadViewController : Controller
    {
        // GET: FileUploadView
        public ActionResult FileUpload()
        {
            return View();
        }
    }
}