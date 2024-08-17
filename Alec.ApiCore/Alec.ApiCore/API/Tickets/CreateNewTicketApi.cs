using System;
using System.Collections.Generic;
using Elka.Api;
using Elka.Core;
using UnityEngine;

namespace Elka.Api
{
    public class CreateNewTicketApi : IPostRequest<CreateNewTicketmodel>
    {
        public string URL => "/tickets";

        public string ApiVersion => "1";

        public string JSONData { get; set; } = default;
        public Dictionary<string, string> FormData { get; set; } = default;

        public void OnResponse(CreateNewTicketmodel result)
        {
            if (result.Status == ResponseStatus.Successful)
            {
                ElkaListener.OnCreateNewTicketRecived?.Invoke(result);
            }
            else
            {
                ElkaListener.OnCreateNewTicketFailed?.Invoke(result.Status, result.Message);
            }
        }
    }

    [Serializable]
    public class CreateNewTicketmodel : Response<CreateNewTicketmodel>
    {

    }

}
