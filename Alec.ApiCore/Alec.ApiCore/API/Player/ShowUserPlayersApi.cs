using System;


namespace Elka.ApiCore
{
    public class ShowUserPlayersApi : IRequest<ShowUserPlayerModel>
    {
        private readonly string UserId;

        public ShowUserPlayersApi(int userId)
        {
            UserId = userId.ToString();
        }

        public string URL => string.Concat("/users/", UserId, "/players");

        public string ApiVersion => "1";

        public MethodType Method => MethodType.GET;

        public ContentType Content => ContentType.FormData;

        public void OnResponse(ResponseStruct<ShowUserPlayerModel> result)
        {
            if (result.success)
            {
                ElkaListener.OnShowUserPlayersRecived?.Invoke(result);
            }
            else
            {
                ElkaListener.OnShowUserPlayersFailed?.Invoke(result.Status, result.Message);
            }
        }
    }

    [Serializable]
    public class ShowUserPlayerModel
    {

    }
}
