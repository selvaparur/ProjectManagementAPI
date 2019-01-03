﻿using ProjectManager.Business;
using ProjectManager.Business.ServiceRequests;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;


namespace ProjectManager.Services.Controllers
{
    [RoutePrefix("v1/users")]
    public class UsersController : ApiController
    {
        readonly IUserBusinessBL _userBusiness;
        
        public UsersController(IUserBusinessBL userBusiness)
        {
            _userBusiness = userBusiness;            
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<UserViewModel> GetAll()
        {
            return _userBusiness.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            var user = _userBusiness.GetAll().FirstOrDefault(e => e.UserId == id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Save(UserViewModel model)
        {
            _userBusiness.Save(model);
            return Ok();
        }

        [HttpPost]
        [Route("delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            _userBusiness.Delete(id);
            return Ok();
        }
    }
}
