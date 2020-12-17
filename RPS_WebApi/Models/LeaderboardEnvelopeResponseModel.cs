using System.Collections.Generic;
using System.Linq;
using RPS_WebApi.Controllers;
using RPS_WebApi.Models;


namespace RPS_WebApi.Models
{
    public class LeaderboardEnvelopeResponseModel
    {
        public List<LeaderBoard> leaderboard = new List<LeaderBoard>();

        public LeaderboardEnvelopeResponseModel()
        {
            
        }

       
    }
}