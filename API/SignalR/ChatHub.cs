using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Create.Command > Application.Commentsden geliyor
        // client buraya bağlanıp metodları invoke edebilecek
        public async Task SendComment(Create.Command command)
        {
            var comment = await _mediator.Send(command);

            await Clients.Group(command.ActivityId.ToString())
            // clientın kullanacağı metod adı , geriye döndüreceğin şey
            // her yeni comment huba bağlı kullanıcılara gönderilecek
                .SendAsync("RecieveComment", comment.Value);
        }

        public override async Task OnConnectedAsync()
        {
            // signalR da query string kullanıyorsun
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            // grup için connection string üretiyor
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
            var result = await _mediator.Send(new List.Query { ActivityId = Guid.Parse(activityId) });
            // bu metod kullanılınca commentleri isteği atana geri yolla
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}