using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ToDoListAPI.Models;
using ToDoListAPI.Repository;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly TodoRepository todoRepository;

        public ToDoController(IConfiguration configuration)
        {
            todoRepository = new TodoRepository(configuration);
        }

        // GET: api/ToDo
        [HttpGet]
        public ActionResult<IEnumerable<ToDo>> Get()
        {
            return todoRepository.FindAll().ToList();
        }

        [HttpGet("{userid}", Name = "Get")]
        public ActionResult<IEnumerable<ToDo>> GetByUserId(int userId)
        {
            return todoRepository.FindAllByUser(userId).ToList();
        }

        // GET: api/ToDo/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<ToDo> Get(int id)
        {
            return todoRepository.FindByID(id);
        }

        // POST: api/ToDo
        [HttpPost]
        public void Post(ToDo toDo)
        {
            todoRepository.Add(toDo);
        }

        // PUT: api/ToDo/5
        [HttpPut("{id}")]
        public void Put(ToDo toDo)
        {
            todoRepository.Update(toDo);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            todoRepository.Remove(id);
        }
    }
}