using System;
using System.Data;
using System.Data.SqlClient;

namespace RPS_WebApi.Models
{
    public class DatabaseLayer
    {
    
        public DatabaseLayer()
        {
        
        }

        public void InsertInitialData(string connectionString, RoundCheckRequestModel angularGameRequest )
        {
            // define INSERT query with parameters
            string query = "INSERT INTO Player (Username) " + 
                            "VALUES (@username) " +
                            "INSERT INTO Game (DateTimeStarted, Username, NumTurns) " + 
                            "VALUES (@dateTimeStarted, @username, @Numrounds) ";

            // create connection and command
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                  using(SqlCommand cmd = new SqlCommand(query, conn))
                  {
                    // define parameters and their values
                    cmd.Parameters.Add("@dateTimeStarted", SqlDbType.DateTime).Value = angularGameRequest.DateTimeStarted;
                    cmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = angularGameRequest.Username;
                    cmd.Parameters.Add("@Numrounds", SqlDbType.Int).Value = angularGameRequest.roundLimit;
                
                    // open connection, execute INSERT, close connection
                    
                    
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    
                  
                  }
                
            }
          
        }

        public void insertTurn(string connectionString,  DateTime dateTimeStarted, string username, int roundLimit, int roundCounter, string outcome, string p1Choice, string p2Choice)
        {
            // define INSERT query with parameters
            string query = 
                            "INSERT INTO Turn (Username, DateTimeStarted, TurnNumber, Outcome, p1Choice, p2Choice ) " + 
                            "VALUES (@username, @dateTimeStarted, @TurnNumber, @outcome, @p1Choice, @p2Choice) ";

            // create connection and command
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                  using(SqlCommand cmd = new SqlCommand(query, conn))
                  {
                    // define parameters and their values
                    cmd.Parameters.Add("@dateTimeStarted", SqlDbType.DateTime).Value = dateTimeStarted;
                    cmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = username;
                    cmd.Parameters.Add("@Numrounds", SqlDbType.Int).Value = roundLimit;
                    cmd.Parameters.Add("@TurnNumber", SqlDbType.Int).Value = roundCounter;
                    cmd.Parameters.Add("@outcome", SqlDbType.VarChar, 1).Value = outcome;
                    cmd.Parameters.Add("@p1Choice", SqlDbType.VarChar, 12).Value = p1Choice;
                    cmd.Parameters.Add("@p2Choice", SqlDbType.VarChar, 12).Value = p2Choice;
                    
                    
                    // open connection, execute INSERT, close connection
                    try{
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch(SqlException) 
                    {
                    
                    }   

                    
                   
                   

                  }
                
            }
          
        }

        
    }
}