using System.Collections.Generic;

namespace RPS_WebApi.Models
{
    public class GameResultEnvelopeResponseModel
    {
         public List<GameResultResponseModel> gameResult = new List<GameResultResponseModel>();

        public GameResultEnvelopeResponseModel()
        {
            
        }
    }
}