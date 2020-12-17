using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RPS_WebApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;

namespace RPS_WebApi.Controllers
{
   

    [ApiController]
    [Route("[controller]")]
    public class rockPaperScissorsController: ControllerBase
    {

       public static List<GameResultResponseModel> results = new List<GameResultResponseModel>();

        // private readonly IOptions<AppDb> appDB;
        IConfiguration configuration;
        SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder();

        string connectionString;
        
       
        public rockPaperScissorsController(IConfiguration iConfig)
        {
            this.configuration = iConfig;

            this.stringBuilder.DataSource = this.configuration.GetSection("DBConnectionStrings").GetSection("Url").Value;
            this.stringBuilder.InitialCatalog = this.configuration.GetSection("DBConnectionStrings").GetSection("Database").Value;
            this.stringBuilder.UserID = this.configuration.GetSection("DBConnectionStrings").GetSection("User").Value;
            this.stringBuilder.Password = this.configuration.GetSection("DBConnectionStrings").GetSection("Password").Value;
            this.connectionString = this.stringBuilder.ConnectionString;


            
        }

       

        [Route("NewGame")]
        [HttpPost]
        public ActionResult<RoundCheckResponseModel> postNewGame(RoundCheckRequestModel angularGameRequest)
        {
            var dbl = new DatabaseLayer();
            var rCheck = new RoundCheckResponseModel();
        
            try
            {
                dbl.InsertInitialData(this.connectionString, angularGameRequest);

                rCheck.currentRound = 0;
                rCheck.Username = angularGameRequest.Username;
                rCheck.roundLimit = angularGameRequest.roundLimit;
                rCheck.DateTimeStarted = angularGameRequest.DateTimeStarted;

            }
            catch (SqlException ex)
            {
                BadRequest(ex);
            }
            return rCheck;
        } 

        [Route("Leaderboard")]
        [HttpGet]
        public List<LeaderBoard> GetLeaderBoard(){

            // so now i need to return the eladboard view into something then return that to angualr
            // not sure if i combine the two here or just use the SQLdataclient. 
           
            LeaderboardEnvelopeResponseModel Lbrm = new LeaderboardEnvelopeResponseModel();
            string leadbrd = "Select * from Leaderboard " +
                             "Order by Winratio DESC ";
                                
            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);
            
            SqlCommand leadCmd = new SqlCommand(leadbrd, connecting);
                    
            try{
                connecting.Open();  
  
                using(SqlDataReader reader = leadCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // ORM - Object Relation Mapping
                        Lbrm.leaderboard.Add(
                            // major problem here was that float in SQL and float in c# are different - so was throwing a casting error - winratio had to be cast as a "single"
                            new LeaderBoard() {Username = reader[0].ToString(), WinRatio = Convert.ToSingle(reader[1]), TurnsPlayed = (int)reader[2]});                
                    }
                    reader.Close();
                }
            }
            catch(SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex}");
            }

            return Lbrm.leaderboard;

        //     //this returns the data into an Ienumerable - soes not have built in iterator - so we need to do soemthing with it afterwards
        //     var output = from result in results
        //     group result by result.Username into linqLeaderboard
        //     select new
        //     {
        //         Username = linqLeaderboard.Key, 
        //         Wins = linqLeaderboard.Count(a => a.Outcome == "Win"),
        //         Games = linqLeaderboard.Count()
        //     };

        //     // here process the data which is in an ienumarable (shitty list)
        //     foreach (var result in output) {
        //         // the calculation for the winratio was moved here from the class to make it easier
        //         float winRatio = (float)result.Wins/result.Games;
        //         // turn into an object that maps to the Leaderboard class
        //         LeaderBoard line = new LeaderBoard() { Username = result.Username, WinRatio = winRatio, TurnsPlayed = result.Games};
        //         //push that object into the list of objects of the same type to be returned.
        //         Lbrm.leaderboard.Add(line);
        //     }

        //    return Lbrm.leaderboard;
            
        } 

    
        
        [Route("Rounds")]
        [HttpPost]
        public GameResultResponseModel PostRound(GameResultRequestModel angularGameRequest){

            
            GameResultResponseModel GameResponse = new GameResultResponseModel(angularGameRequest);
            var dbl = new DatabaseLayer();
            
            dbl.insertTurn(this.connectionString, GameResponse.DateTimeStarted, GameResponse.Username, GameResponse.roundLimit, GameResponse.roundCounter, GameResponse.Outcome, GameResponse.PlayerChoice, GameResponse.CpuChoice);
        
            return GameResponse;

        } 

        [Route("GameResult")]
        [HttpPost]

        // need to add some data from angualr
        public List<GameResultResponseModel> PostGameResult(endOfGameRequestModel angualrEndOfGameRequest){
        
            GameResultEnvelopeResponseModel gameEnv = new GameResultEnvelopeResponseModel();

            // define select query with parameters
            string query1 = "Select * from Turn " 
                                + "where Username = @username" +
                                " AND dateTimeStarted = @dateTimeStarted";

            // create connection and command
            SqlConnection conn = new SqlConnection(connectionString);
            
            SqlCommand getGameCmd = new SqlCommand(query1, conn);
            getGameCmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = angualrEndOfGameRequest.Username;
            getGameCmd.Parameters.Add("@dateTimeStarted", SqlDbType.DateTime).Value = angualrEndOfGameRequest.DateTimeStarted;
            

            try{
                conn.Open();  
                    
                using(SqlDataReader reader = getGameCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // ORM - Object Relation Mapping
                        gameEnv.gameResult.Add(
                            new GameResultResponseModel() { Username = reader[0].ToString(), DateTimeStarted = Convert.ToDateTime(reader[1]), roundCounter = (int)reader[2], Outcome = reader[3].ToString(), PlayerChoice = reader[4].ToString(), CpuChoice = reader[5].ToString() });                
                    }
                    reader.Close();
                }
            }
            catch(SqlException)
            {
                throw new ApplicationException("Some sql error happened");
            }

            return gameEnv.gameResult;      
         
    
        } 


    }
}