namespace People.Api.Domain;

public interface IPersonProvider
{
    Task<Option<Person>> GetAsync(Username username);
}