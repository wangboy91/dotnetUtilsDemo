using MediatR;

namespace MediatRDemo;

public class SendEvent : INotification
{
    public string Type { get; set; }
}

public interface IPushDemo
{
    void PushData();
}
public class PushDemo:IPushDemo
{
    private readonly IMediator _mediator;

    public PushDemo(IMediator mediator)
    {
        _mediator= mediator;
    }
    public void PushData()
    {
        _mediator.Publish(new SendEvent() {Type = "测试一下"});
    }
}
public class EventHandle1 : INotificationHandler<SendEvent>
{
    public Task Handle(SendEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"EventHandle1 收到新消息:{notification.Type} 登录成功了");
        return Task.CompletedTask;
    }
}
public class EventHandle2 : INotificationHandler<SendEvent>
{
    public Task Handle(SendEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"EventHandle2 收到新消息:{notification.Type} 登录成功了");
        return Task.CompletedTask;
    }
}