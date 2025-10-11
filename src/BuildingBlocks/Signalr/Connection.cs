namespace BuildingBlocks.Signalr;

public class Connection
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ConnectionId { get; set; } = default!;
    public bool IsConnected { get; set; }
    public DateTime ConnectionAt { get; set; }
    public DateTime? DisconnectionAt { get; set; }
    public long? TokenExpiry { get; set; }

    public static Connection Create(Guid userId, string connectionId, long? tokenExpiry = null)
    {
        var connection = new Connection
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ConnectionId = connectionId,
            IsConnected = true,
            ConnectionAt = DateTime.UtcNow,
            TokenExpiry = tokenExpiry
        };

        return connection;
    }

    public void ChangeState(bool isConnected)
    {
        IsConnected = isConnected;
        DisconnectionAt = DateTime.UtcNow;
    }
}
