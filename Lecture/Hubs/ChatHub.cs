using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Lecture.Hubs
{
    //[Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message, string targetId)
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
                if(string.IsNullOrWhiteSpace(targetId))
                {
                    await Clients.All.SendAsync("ReceiveMessage", this.Context.ConnectionId, message);
                }
                else
                {
                    //await Clients.Client(targetId.Trim()).SendAsync("ReceiveMessage", this.Context.User.Identity.Name, message);
                    await Clients.Client(targetId.Trim()).SendAsync("ReceiveMessage", this.Context.ConnectionId, message);
                }

            }
        }

        public override async Task OnConnectedAsync()
        {
            //await Clients.All.SendAsync("ReceiveMessage", "SYSTEM", $"{this.Context.User.Identity.Name}님이 입장하였습니다.");
            await Clients.All.SendAsync("ReceiveMessage", "SYSTEM", $"{this.Context.ConnectionId}님이 입장하였습니다.");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //await Clients.All.SendAsync("ReceiveMessage", "SYSTEM", $"{this.Context.User.Identity.Name}님이 퇴장하였습니다.");
            await Clients.All.SendAsync("ReceiveMessage", "SYSTEM", $"{this.Context.ConnectionId}님이 퇴장하였습니다.");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
