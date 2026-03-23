using System.Net;

namespace VaccineManager.Application.Common.Errors;

public class ApplicationErrors
{
    public static class User
    {
        public static ApiError DuplicateEmail  =>
            new(HttpStatusCode.Conflict, "A user with this email already exists.");
        
        public static ApiError NotFound   =>
            new(HttpStatusCode.NotFound, "A user with this ID was not found.");
        
        public static ApiError InvalidPasswordOrEmail =>
            new(HttpStatusCode.Unauthorized, "Invalid password or email.");
    }
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
        
        public static ApiError DuplicateCode => 
            new(HttpStatusCode.Conflict, "A vaccine with this code already exists.");

        public static ApiError HasRecords =>
            new(HttpStatusCode.Conflict, "Cannot delete a vaccine that has vaccination records.");
    }

    public static class VaccinationRecord
    {
        public static ApiError PersonNotFound(Guid id) =>   
            new(HttpStatusCode.NotFound, $"Person with ID '{id}' was not found.");
        
        public static ApiError VaccineNotFound(Guid id) =>
            new(HttpStatusCode.NotFound, $"Vaccine with ID '{id}' was not found.");
        
        public static ApiError NotFound(Guid id) =>
            new(HttpStatusCode.NotFound, $"Vaccination record with ID '{id}' was not found.");

        public static ApiError MaxDosesReached =>
            new(HttpStatusCode.Conflict, "Max amount of doses reached.");
    }
}