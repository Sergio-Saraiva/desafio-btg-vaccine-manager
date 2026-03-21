- Generating base entities: Person, Vaccine, VaccinationRecord.
- Person: the person that receives a Vaccine. Has a system Id, a identifier CPF, for brazilian citizens, and passport number for foreigners. Don´t like that we are using a switch statement for the document validation, depending on the type of the document, might use strategy pattern later thinking when another types of document are added in the system.
- Vaccine, the vaccine it self with a system id, a name and amount of doses. Right now the doses are only a integer, might create another entity for doses later.
- VaccinationRecord: the link between a Vaccine and a Person.
- Every id a UUID V7, wich might help us with idexing when querying the database.
- Every entity has a Id, CreatedAt, UpdatedAt and DeletedAt.
- Next steps: create repository interfaces, creating databsae infrastucture.
- The validation might move when FluentValidation is added.
- Created the Application DbContext, overwritten the SaveChanges method, for automatic CreatedAt and UpdatedAt populating.
- Created BaseRepository for common methods that might be used by other repositories.
- Created unique empty interfaces for specifc entity methods.
- Using UnitOfWork for atomic actions in the database and for better management of possible future transactions.
- Next steps: wire with database (SQLite), implement dependecy injection for infrastructure layer, write first unit tests?


