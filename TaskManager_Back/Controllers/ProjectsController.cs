using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace TaskManager_Back.Controllers
{
    public class ProjectsController : ApiController
    {
        TaskManagerEntities3 entities = new TaskManagerEntities3();


        [Authorize(Roles = "admin")]
        [Route("api/projects/all")]
        public HttpResponseMessage GetAll()
        {
            var projects = entities.Project.ToArray();
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
            DataSet ds = new DataSet();

            if (User.Identity.GetUserName() == "admin@example.com") {
                return GetAll();
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spGetAllowedProjects", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("UserId", SqlDbType.NVarChar).Value = User.Identity.GetUserId();
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    adp.Fill(ds);
                    return Request.CreateResponse(HttpStatusCode.OK, ds);
                }
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] ProjectModel pr)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spCreateNewProject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("Title", SqlDbType.NVarChar).Value = pr.Title;
                    cmd.Parameters.Add("Description", SqlDbType.NVarChar).Value = pr.Description;
                    cmd.Parameters.Add("CreatedAt", SqlDbType.DateTime).Value = DateTime.Now;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "succSess");
        }
        
        [HttpPost]
        [Route("api/projects/giveAccessToUser")]
        [Authorize(Roles="admin")]
        public HttpResponseMessage GiveAccess([FromBody] AllowedProjectModel ap)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spGiveAccessToProject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("PersonId", SqlDbType.NVarChar).Value = ap.PersonID;
                    cmd.Parameters.Add("ProjectId", SqlDbType.NVarChar).Value = ap.ProjectID;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "succSess");
        }

        [HttpGet]
        [Authorize]
        [Route("api/projects/GetAllowedUsers")]
        public HttpResponseMessage GetAllowedUsersToProject(int ProjectId)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spGetUsersByProjectId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("ProjectId", SqlDbType.Int).Value = ProjectId;
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    adp.Fill(ds);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, ds);
        }




    }
}
