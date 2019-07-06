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
                dbConnection.Query<User>("INSERT INTO Users(Name, Email) VALUES('" + item.Name + "','" + item.Email + "')");
            }
        }

        public IEnumerable<User> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM Users");
            }
        }

        public User FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM Users WHERE ID=" + id.ToString()).FirstOrDefault();
            }
        }

        public void Remove(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query<User>("DELETE FROM Users WHERE ID=" + id);
            }
        }

        public void Update(User item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query<User>("UPDATE Users SET Name='" + item.Name + "', Email='" + item.Email + "' WHERE ID=" + item.ID);
            }
        }
    }
}
