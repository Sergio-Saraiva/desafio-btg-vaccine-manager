using System.Net;

namespace VaccineManager.Application.Common.Errors;

public class ApplicationErrors
{
    public static class Person
    {
        public static ApiError NotFound(Guid id) =>
            new(HttpStatusCode.NotFound, $"Person with ID '{id}' was not found.");

        public static ApiError DuplicateDocument =>
            new(HttpStatusCode.Conflict, "A person with this document already exists.");
    }

    public static class Vaccine
    {
        public static ApiError NotFound(Guid id) =>
            new(HttpStatusCode.NotFound, $"Vaccine with ID '{id}' was not found.");

        public static ApiError DuplicateName =>
            new(HttpStatusCode.Conflict, "A vaccine with this name already exists.");

        public static ApiError HasRecords =>
            new(HttpStatusCode.Conflict, "Cannot delete a vaccine that has vaccination records.");
    }

    public static class VaccinationRecord
    {
        public static ApiError NotFound(Guid id) =>
            new(HttpStatusCode.NotFound, $"Vaccination record with ID '{id}' was not found.");

        public static ApiError InvalidDose(int max) =>
            new(HttpStatusCode.BadRequest, $"Dose must be between 1 and {max}.");

        public static ApiError DuplicateDose =>
            new(HttpStatusCode.Conflict, "This dose has already been administered.");
    }
}