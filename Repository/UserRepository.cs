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
    public class UserRepository : IRepository<User>
    {
        private string connectionString;

        public UserRepository(IConfiguration configuration)
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

        public void Add(User item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "INSERT INTO Users(Name, Email, Password) VALUES(@Name, @Email, @Password)";
                dbConnection.Query<User>(query, item);
            }
        }

        public IEnumerable<User> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = @"SELECT Users.Id, Users.Name, Users.Email, Users.Password,
                                ToDos.Id, ToDos.ToDoText, ToDos.CreatedDate
                                FROM Users
                                INNER JOIN ToDos ON ToDos.UserId=Users.Id";
                return dbConnection.Query<User, ToDo, User>(query, (user, todo) =>
                {
                    user.ToDos.Add(todo);

                    return user;
                }).ToList();
            }
        }

        public User FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "SELECT * FROM Users WHERE ID=@id";
                return dbConnection.Query<User>(query, new { id }).FirstOrDefault();
            }
        }

        public void Remove(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "DELETE FROM Users WHERE ID=@id";
                dbConnection.Query<User>(query, new { id });
            }
        }

        public void Update(User item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "UPDATE Users SET Name=@Name, Email=@Email, Password=@Password WHERE ID=@ID";
                dbConnection.Query<User>(query, item);
            }
        }

        public void UpdatePasswordByID(User user)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "UPDATE Users SET Password=@Password WHERE ID=@ID";
                dbConnection.Query<User>(query, user);
            }
        }

        public User FindUserByEmail(string Email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "SELECT * FROM Users WHERE Email=@Email";
                return dbConnection.Query<User>(query, new { Email }).FirstOrDefault();
            }
        }
    }
}