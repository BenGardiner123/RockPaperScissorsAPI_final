using System;
using RPS_WebApi.Models;

namespace RPS_WebApi.Models
{
    public class GameResultResponseModel
    {
        public string Username { get; set; }
        public string PlayerChoice { get; set; }
        public string CpuChoice { get; set; }
        public string Outcome { get; set; }
        public int roundCounter { get; set; }
        public int roundLimit { get; set; }
        public DateTime DateTimeStarted { get; set; } 

        public GameResultResponseModel()
        {
           
        }

        // constructor
        public GameResultResponseModel(GameResultRequestModel angularRequest)
        {
            Username = angularRequest.Username;
            PlayerChoice = angularRequest.PlayerChoice;
            CpuChoice = createAiChoice();
            Outcome = calulatewinner();
            roundCounter = angularRequest.roundCounter;
            roundLimit = angularRequest.roundLimit;
            DateTimeStarted = angularRequest.DateTimeStarted;
        }

        string createAiChoice (){
            Random rnd=new Random();
            string[] choices = { "Rock", "Paper", "Scissors" };
            int randomNumber = rnd.Next(0, choices.Length);
            return choices[randomNumber];

        }


        string calulatewinner(){
            
            if (this.CpuChoice == this.PlayerChoice)
            {
               return this.Outcome = "Draw";
            }
            else if ((this.PlayerChoice == "Rock" && this.CpuChoice == "Scissors") || (this.PlayerChoice == "Paper" && this.CpuChoice == "Rock") || (this.PlayerChoice == "Scissors" && this.CpuChoice == "Paper"))
            {
                return this.Outcome = "Win";
            }
            else 
            {
                return this.Outcome = "Lose";
            }
        }






        
        
        
    
    
    }


}