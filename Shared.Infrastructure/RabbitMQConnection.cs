public class RabbitMQConnection
{
    private readonly IConnectionFactory _connectionFactory;

    public RabbitMQConnection(IConfiguration configuration)
    {
        _connectionFactory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:HostName"],
            UserName = configuration["RabbitMQ:UserName"],
            Password = configuration["RabbitMQ:Password"],
            Port = int.Parse(configuration["RabbitMQ:Port"]),
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };
    }
}