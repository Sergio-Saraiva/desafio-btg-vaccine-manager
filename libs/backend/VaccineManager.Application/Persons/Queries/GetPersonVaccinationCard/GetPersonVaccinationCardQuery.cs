using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.Persons.Queries.GetPersonVaccinationCard;

public sealed record GetPersonVaccinationCardQuery(Guid Id) : IQuery<GetPersonVaccinationCardResponse>;