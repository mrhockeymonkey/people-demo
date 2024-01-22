using System.Collections.Concurrent;
using People.Api.Domain;

namespace People.Api.Infrastructure;

public class PersonRepository : IPersonStore, IPersonProvider
{
    private readonly ConcurrentDictionary<Username, Person> _data;

    public PersonRepository()
    {
        _data = new();
    }

    public async Task AddOrUpdate(Person person)
    {
        await Task.Delay(TimeSpan.FromSeconds(2)); // simulate network
        _data.AddOrUpdate(person.Username, person, (_, _) => person);
    }
    
    public async Task<Option<Person>> GetAsync(Username username)
    {
        await Task.Delay(TimeSpan.FromSeconds(2)); // simulate network
        
        if (_data.TryGetValue(username, out var person))
            return Option<Person>.Some(person);

        return Option<Person>.None();

    }
}