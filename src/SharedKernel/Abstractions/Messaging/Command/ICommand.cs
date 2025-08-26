namespace SharedKernel.Abstractions.Messaging.Command;

public interface ICommand : IBaseCommand;

public interface ICommand<TResponse> : IBaseCommand;


public interface IBaseCommand;