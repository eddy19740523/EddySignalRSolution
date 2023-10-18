//참조추가 
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;



namespace SignalRAzureSignalRApp.Hubs
{
    public class ChatHub : Hub
    {

        //웹브라우저 송신 메시지 수신 및 모든 브라우저에게 재발송
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinGroup(string group, string user)
        {
            //SignalR Hub 채널 그룹 등록 
            await Groups.AddToGroupAsync(Context.ConnectionId, group);

            //호출접속자 자신에게 발송
            await Clients.Caller.SendAsync("GroupJoined", $"{group}채팅방에 정상  접속하였습니다");

            //같은 채팅방내 다른 사용자들에게 
            await Clients.OthersInGroup(group).SendAsync("GroupJoined", $"{user}님이 {group}채팅방에 입장하셨습니다");

            //특정 사용자에게 메시지 보내기 
            //await Clients.Client("ConnectionId").SendAsync("TargetUserMsg", $"특정사용자에게 보내는 메시지입니다.");

        }


        public async Task GroupSendMessage(string group, string user, string message)
        {
            //해당 그룹내 모든 사용자에게 발송
            await Clients.Group(group).SendAsync("GroupReceiveMessage", user, message);
        }



    }
}
