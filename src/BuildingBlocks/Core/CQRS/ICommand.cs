
using MediatR;

namespace BuildingBlocks.Core.CQRS;

//ref: https://stackoverflow.com/questions/1516876/when-to-use-in-vs-ref-vs-out

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out T> : IRequest<T>
    where T : notnull
{
}
