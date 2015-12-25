﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoInterfaces
{
    public class DAL
    {
        private string ErrorMessage { get; set; }
        private SqlConnection conn;
        private static string connString;
        private SqlCommand command;
        private static List<ToDo> toDoList;

        public DAL(string _connString)
        {
            connString = _connString;
        }
        /// <summary>
        /// AddToDo
        /// </summary>
        /// <param name="toDo"></param>
        public void AddToDo(ToDo toDo)
        {
            try
            {
                using (conn)
                {
                    //using parametirized query
                    string sqlInserString =
                    "INSERT INTO ToDoList (Description, Name, CreatedDate, DeadLine, EstimationTime, Finnished) VALUES ( @description, @name, @CreatedDate, @deadLine, @estimationTime, @finnished)";

                    conn = new SqlConnection(connString);

                    command = new SqlCommand();
                    command.Connection = conn;
                    command.Connection.Open();
                    command.CommandText = sqlInserString;

                    SqlParameter descriptionParam = new SqlParameter("@description", toDo.Description);
                    SqlParameter userParam = new SqlParameter("@name", toDo.Name);
                    SqlParameter createdParam = new SqlParameter("@createdDate", toDo.CreatedDate);
                    SqlParameter deadLineParam = new SqlParameter("@deadLine", toDo.DeadLine);
                    SqlParameter estimateParam = new SqlParameter("@estimationTime", toDo.EstimationTime);
                    SqlParameter flagParam = new SqlParameter("@finnished", toDo.Finnished ? 1 : 0);


                    command.Parameters.AddRange(new SqlParameter[] { descriptionParam, userParam, createdParam, deadLineParam, estimateParam, flagParam });
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        /// <summary>
        /// UpdateToDo
        /// </summary>
        /// <param name="toDo"></param>
        public void UpdateToDo(ToDo toDo)
        {
            try
            {
                using (conn)
                {
                    string sqlUpdateString =
                    "UPDATE ToDoList SET Description=@description, Name=@name, CreatedDate=@createdDate, DeadLine=@deadLine, EstimationTime=@estimationTime, Finnished=@finnished WHERE ID=" + toDo.Id;

                    conn = new SqlConnection(connString);

                    command = new SqlCommand();
                    command.Connection = conn;
                    command.Connection.Open();
                    command.CommandText = sqlUpdateString;

                    SqlParameter descriptionParam = new SqlParameter("@Description", toDo.Description);
                    SqlParameter userParam = new SqlParameter("@Name", toDo.Name);
                    SqlParameter createdParam = new SqlParameter("@createdDate", toDo.CreatedDate);
                    SqlParameter deadLineParam = new SqlParameter("@deadLine", toDo.DeadLine);
                    SqlParameter estimateParam = new SqlParameter("@EstimationTime", toDo.EstimationTime);
                    SqlParameter flagParam = new SqlParameter("@finnished", toDo.Finnished ? 1 : 0);

                    command.Parameters.AddRange(new SqlParameter[] { descriptionParam, userParam, createdParam, deadLineParam, estimateParam, flagParam });
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        /// <summary>
        /// DeleteToDo
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteToDo(int ID)
        {
            try
            {
                using (conn)
                {
                    string sqlDeleteString = "DELETE FROM ToDoLIst WHERE ID=@ID ";

                    conn = new SqlConnection(connString);

                    command = new SqlCommand();
                    command.Connection = conn;
                    command.Connection.Open();
                    command.CommandText = sqlDeleteString;

                    SqlParameter IdParam = new SqlParameter("@ID", ID);
                    command.Parameters.Add(IdParam);
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// GetToDoList
        /// </summary>
        /// <returns></returns>
        public List<ToDo> GetToDoList()
        {
            try
            {
                using (conn)
                {
                    toDoList = new List<ToDo>();

                    conn = new SqlConnection(connString);

                    string sqlSelectString = "SELECT * FROM ToDoList";
                    command = new SqlCommand(sqlSelectString, conn);
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ToDo toDo = new ToDo();
                        toDo.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        toDo.Description = reader.GetString(reader.GetOrdinal("Description"));
                        toDo.Name = reader.GetString(reader.GetOrdinal("Name"));
                        toDo.DeadLine = reader.GetDateTime(reader.GetOrdinal("DeadLine"));
                        toDo.CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
                        toDo.EstimationTime = reader.GetInt32(reader.GetOrdinal("EstimationTime"));
                        toDo.Finnished = reader.GetBoolean(reader.GetOrdinal("Finnished"));
                        toDoList.Add(toDo);
                    }
                    command.Connection.Close();
                    return toDoList;
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return null;


        }

        /// <summary>
        ///  GetToDoById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToDo GetToDoById(int id)
        {
            try
            {
                using (conn)
                {

                    conn = new SqlConnection(connString);

                    string sqlSelectString = "SELECT * FROM ToDoLIst WHERE ID=" + id;
                    command = new SqlCommand(sqlSelectString, conn);
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    ToDo toDo = new ToDo();
                    toDo.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                    toDo.Description = reader.GetString(reader.GetOrdinal("Description"));
                    toDo.Name = reader.GetString(reader.GetOrdinal("Name"));
                    toDo.DeadLine = reader.GetDateTime(reader.GetOrdinal("DeadLine"));
                    toDo.CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
                    toDo.EstimationTime = reader.GetInt32(reader.GetOrdinal("EstimationTime"));
                    toDo.Finnished = reader.GetBoolean(reader.GetOrdinal("Finnished"));

                    command.Connection.Close();
                    return toDo;
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return null;


        }
        /// <summary>
        ///  GetToDoListByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        public List<ToDo> GetToDoListByName(string name)
        {
            try
            {
                using (conn)
                {
                    toDoList = new List<ToDo>();

                    conn = new SqlConnection(connString);

                    string sqlSelectString = "select * from ToDoList where Name like '%" + name + "%'";
                    command = new SqlCommand(sqlSelectString, conn);
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ToDo toDo = new ToDo();
                        toDo.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        toDo.Description = reader.GetString(reader.GetOrdinal("Description"));
                        toDo.Name = reader.GetString(reader.GetOrdinal("Name"));
                        toDo.DeadLine = reader.GetDateTime(reader.GetOrdinal("DeadLine"));
                        toDo.CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
                        toDo.EstimationTime = reader.GetInt32(reader.GetOrdinal("EstimationTime"));
                        toDo.Finnished = reader.GetBoolean(reader.GetOrdinal("Finnished"));
                        toDoList.Add(toDo);
                    }
                    command.Connection.Close();
                    return toDoList;
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return null;


        }

        public String GetErrorMessage()
        {
            return ErrorMessage;
        }
    }
}
