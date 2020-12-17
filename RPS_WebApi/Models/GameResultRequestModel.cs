using System;

namespace RPS_WebApi.Models
{
    public class GameResultRequestModel
    {
        public string Username { get; set; }
        public string PlayerChoice { get; set; }
        public int roundCounter { get; set; }
        public int roundLimit { get; set; }
        public DateTime DateTimeStarted { get; set; }

    }
}