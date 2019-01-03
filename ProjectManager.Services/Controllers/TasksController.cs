using ProjectManager.Business;
using ProjectManager.Business.ServiceRequests;
using System.Web.Http;

namespace ProjectManager.Services.Controllers
{
    [RoutePrefix("v1/tasks")]
    public class TasksController : ApiController
    {
        readonly ITaskBusinessBL _taskBusiness;
        public TasksController(ITaskBusinessBL taskBusiness)
        {
            _taskBusiness = taskBusiness;
        }

        [HttpGet]
        [Route("healthcheck")]
        public IHttpActionResult healthcheck()
        {
            string strhealth = "API has started !!!";
            return Ok(strhealth);
        }

        [HttpGet]
        [Route("parent-tasks")]
        public IHttpActionResult GetAllParentTasks()
        {
            var models = _taskBusiness.GetAllParentTasksBL();
            return Ok(models);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            var task = _taskBusiness.GetById(id);
            return Ok(task);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var tasks = _taskBusiness.GetAllTasksBL();
            return Ok(tasks);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Save(TaskViewModel model)
        {
            _taskBusiness.Save(model);
            return Ok();
        }

        [HttpPost]
        [Route("complete")]
        public IHttpActionResult Complete(TaskViewModel model)
        {
            _taskBusiness.Complete(model);

            var tasks = _taskBusiness.GetAllTasksBL();
            return Ok(tasks); ;
        }
    }
}
