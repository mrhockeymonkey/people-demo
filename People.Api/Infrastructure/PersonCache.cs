using Microsoft.Extensions.Caching.Memory;
using People.Api.Domain;

namespace People.Api.Infrastructure;

public class PersonCache : IPersonProvider
{
    private readonly IMemoryCache _cache;
    private readonly IPersonProvider _provider;
    private readonly ILogger<PersonCache> _logger;
    private readonly SemaphoreSlim _semaphore;

    public PersonCache(IMemoryCache cache, IPersonProvider provider, ILogger<PersonCache> logger)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _semaphore = new(1, 1);
    }

    public async Task<Option<Person>> GetAsync(Username username)
    {
        if (_cache.TryGetValue(username, out Option<Person> cachedPerson) && cachedPerson.IsSome())
            return cachedPerson;

        await _semaphore.WaitAsync();
        try
        {
            // double check
            if (_cache.TryGetValue(username, out Option<Person> cachedLaterPerson) && cachedLaterPerson.IsSome())
                return cachedLaterPerson;

            // apis like _cache.GetOrUpdate not "actually" thread safe so better to take control
            _logger.LogInformation("Getting person from store");
            var newPerson = await _provider.GetAsync(username);
            
            if (newPerson.IsSome()) 
            {
                _cache.Set(username, newPerson, TimeSpan.FromSeconds(5));
            }
            
            return newPerson;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}