using System;

namespace RPS_WebApi.Models
{
    public class RoundCheckRequestModel
    {
        public string Username { get; set; }
        public int roundLimit { get; set; }
        public DateTime DateTimeStarted { get; set; }

    }
}