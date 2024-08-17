using Alec.Core;
using System;

namespace Alec.Api
{
    public class UpdatePlayerApi : IPutRequest<Response>
    {

        private readonly string PlayerId;

        public UpdatePlayerApi(int playerId)
        {
            PlayerId = playerId.ToString();
        }

        public string Route => "/players/" + PlayerId;

        public string ApiVersion => "2";

        public string JSONData { get; set; } = default;

        public void OnResponse(Response result)
        {
            if (result.Status == ResponseStatus.Successful)
            {
                AlecListener.OnUpdatePlayerRecived?.Invoke(result.Message);
            }
            else
            {
                AlecListener.OnUpdatePlayerFailed?.Invoke(result.Status, result.Message);
            }
        }

    }

}