using FluentResults;
using MediatR;

namespace VaccineManager.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result> { }

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }