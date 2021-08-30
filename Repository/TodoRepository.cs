using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Npgsql;
using ToDoListAPI.Models;

namespace ToDoListAPI.Repository
{
    public class TodoRepository : IRepository<ToDo>
    {
        private string connectionString;

        public TodoRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        public void Add(ToDo item)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                var query = "INSERT INTO Todos(todotext, userid, createddate) VALUES(@ToDoText, @UserId, @CreatedDate)";
                db.Query<ToDo>(query, item);
            }
        }

        public IEnumerable<ToDo> FindAll()
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                var query = @"SELECT ToDos.ID, ToDos.todotext, ToDos.UserId, ToDos.CreatedDate,
                        Users.Id, Users.Name, Users.Email, Users.Password
                        FROM ToDos INNER JOIN Users ON ToDos.UserId=Users.Id";
                return db.Query<ToDo, User, ToDo>(query, (todo, user) =>
                {
                    todo.user = user;

                    return todo;
                }).ToList();
            }
        }

        public ToDo FindByID(int id)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                var query = @"SELECT ToDos.ID, ToDos.todotext, ToDos.UserId, ToDos.CreatedDate,
                        Users.Id, Users.Name, Users.Email, Users.Password
                        FROM ToDos INNER JOIN Users ON ToDos.UserId=Users.Id
                        WHERE ToDos.ID=@id";
                return db.Query<ToDo, User, ToDo>(query, (todo, user) =>
                {
                    todo.user = user;
                    return todo;
                }, new { id }).FirstOrDefault();
            }
        }

        public void Remove(int id)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                var query = "DELETE FROM ToDos WHERE ID=@id";
                db.Query<ToDo>(query, new { id });
            }
        }

        public void Update(ToDo item)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                var query = "UPDATE ToDos SET ToDoText=@ToDoText, CreatedDate=NOW() WHERE ID=@ID";
                db.Query<ToDo>(query, item);
            }
        }
    }
}