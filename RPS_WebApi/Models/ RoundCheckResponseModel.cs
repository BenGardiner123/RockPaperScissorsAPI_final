using System;

namespace RPS_WebApi.Models
{
    public class  RoundCheckResponseModel
    {
        public string Username { get; set; }
        public int roundLimit { get; set; }
        public int currentRound { get; set; }
        public DateTime DateTimeStarted { get; set; }

      
    }
}