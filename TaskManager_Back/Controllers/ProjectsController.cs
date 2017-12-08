using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace TaskManager_Back.Controllers
{
    public class ProjectsController : ApiController
    {
        TaskManagerEntities3 entities = new TaskManagerEntities3();

        [Authorize(Roles="admin")]
        [Route("api/projects/all")]
        public HttpResponseMessage GetAll()
        {
            var projects = entities.Project.ToList();
            if (projects == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Internal error");
            }
            else {
                return Request.CreateResponse(HttpStatusCode.OK, projects);
            }

        }
        
        [Authorize]
        public HttpResponseMessage Get()
        {
            var projects = entities.AllowedProjects;
            if (projects == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Internal error");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, User.Identity.GetUserId());
            }

        }
    }
}
