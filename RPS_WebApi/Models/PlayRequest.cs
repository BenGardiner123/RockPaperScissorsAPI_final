namespace RPS_WebApi.Models
{
    public class PlayRequest
    {

        public string PlayerChoice { get; set; }
        public string Username { get; set; }

        public PlayRequest()
        {
            
        }
        public PlayRequest(string angularHttpRequest, string username)
        {
            this.Username = username;
            this.PlayerChoice = angularHttpRequest;

        }
    }

}
