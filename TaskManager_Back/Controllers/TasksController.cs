using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Microsoft.AspNet.Identity;

namespace TaskManager_Back.Controllers
{
    public class TasksController : ApiController
    {
        TaskManagerEntities3 entities = new TaskManagerEntities3();
        [Authorize]
        public HttpResponseMessage Get(int projectID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spGetTaskByProjectID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("ProjectId", SqlDbType.Int).Value = projectID;
                    cmd.Parameters.Add("UserId", SqlDbType.NVarChar).Value = User.Identity.GetUserId();
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    adp.Fill(ds);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, ds);
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error. Required projectID. api/tasks/projectID={id}");
        }


        [HttpGet]
        [Route("api/tasks/subtasks")]
        public HttpResponseMessage getSubtasks([FromUri] int taskID)
        {

            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spGetSubtasks", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("TaskId", SqlDbType.Int).Value = taskID;
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    adp.Fill(ds);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, ds);
        }


        [HttpPost]
       // [Route("api/tasks/create")]
        public HttpResponseMessage Create([FromBody] Tasks ts)
        {


            entities.Tasks.Add(ts);
            entities.SaveChanges();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            //{
            //    using (SqlCommand cmd = new SqlCommand("spGiveAccessToProject", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add("Parent", SqlDbType.Int).Value = ts.Parent;
            //        cmd.Parameters.Add("ProjectId", SqlDbType.Int).Value = ts.ProjectID;
            //        cmd.Parameters.Add("Title", SqlDbType.NVarChar).Value = ts.Title;
            //        cmd.Parameters.Add("Description", SqlDbType.NVarChar).Value = ts.Description_;
            //        cmd.Parameters.Add("StartAt", SqlDbType.DateTime).Value = DateTime.Now;
            //        cmd.Parameters.Add("EndAt", SqlDbType.DateTime).Value = DateTime.Now;
            //        cmd.Parameters.Add("ExpectedAt", SqlDbType.DateTime).Value = DateTime.Now;
            //        cmd.Parameters.Add("CreatedAt", SqlDbType.DateTime).Value = DateTime.Now;
            //        cmd.Parameters.Add("LastUpdatedAt", SqlDbType.DateTime).Value = DateTime.Now;
            //        cmd.Parameters.Add("CreatorId", SqlDbType.NVarChar).Value = ts.CreatorID;
            //        cmd.Parameters.Add("Type_", SqlDbType.Int).Value = ts.Type_;
            //        cmd.Parameters.Add("Color", SqlDbType.Int).Value = ts.Color;
            //        cmd.Parameters.Add("Status_", SqlDbType.Int).Value = ts.Status_;
            //        cmd.Parameters.Add("Performer", SqlDbType.NVarChar).Value = ts.Performer;

            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //    }
            //}
            return Request.CreateResponse(HttpStatusCode.OK, "success");

        }

    }


}
