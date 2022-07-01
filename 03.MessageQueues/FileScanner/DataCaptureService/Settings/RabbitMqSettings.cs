namespace DataCaptureService;

public class RabbitMqSettings
{
    public string HostName { get; set; } = default!;
    public int Port { get; set; }
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Exchange { get; set; } = default!;
    public string RouteKey { get; set; } = default!;

}
