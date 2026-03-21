using FluentResults;
using MediatR;

namespace VaccineManager.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }