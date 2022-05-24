using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Lecture.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {

            if (message == "exit")
            {
                // Clients.All    : 모두에게 전달
                // Clients.Caller : 요청자에게 전달
                await Clients.Caller.SendAsync("ReceiveMessage", "SYSTEM", "접속이 종료되었습니다.");
                Context.Abort();
            }
            else
            {
                //await Clients.All.SendAsync("ReceiveMessage", user, message);

                await Clients.All.SendAsync("ReceiveMessage", this.Context.User.Identity.Name, message);
            }
        }
    }
}
