using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ToDoListAPI.Models;
using ToDoListAPI.Repository;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository userRepository;

        public UserController(IConfiguration configuration)
        {
            userRepository = new UserRepository(configuration);
        }

        // GET api/values
        [HttpGet]
        [EnableCors("MyPolicy")]
        public ActionResult<IEnumerable<User>> Get()
        {
            var users = userRepository.FindAll().ToList();
            return users;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [EnableCors("MyPolicy")]
        public ActionResult<User> Get(int id)
        {
            return userRepository.FindByID(id);
        }

        // POST api/values
        [HttpPost]
        [EnableCors("MyPolicy")]
        public void Post(User user)
        {
            userRepository.Add(user);
        }

        // PUT api/values/5
        [HttpPut]
        [EnableCors("MyPolicy")]
        public void Put(User user)
        {
            userRepository.Update(user);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [EnableCors("MyPolicy")]
        public void Delete(int id)
        {
            userRepository.Remove(id);
        }
    }
}