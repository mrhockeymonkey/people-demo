namespace People.Api.Domain;

public interface IPersonStore
{
    Task AddOrUpdate(Person registeredPerson);
}