
namespace BuildingBlocks.Signalr;

public interface ISignalrHub
{
    Task ProcessAsync(
        string target, 
        string message, 
        CancellationToken cancellationToken = default);

    Task BoardCastAsync(
        string message,
        CancellationToken cancellationToken = default);
}
