using Microsoft.AspNetCore.Mvc;
using People.Api.Domain;

namespace People.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonProvider _provider;
    private readonly IPersonStore _store;

    public PersonController(IPersonProvider provider, IPersonStore store)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    [HttpGet(Name = "GetPerson")]
    public async Task<IActionResult> Get(string username) // in prod would use a dto object
    {
        if (TryGetUsername(username, out var validUsername))
        {
            var personOption = await _provider.GetAsync(validUsername);
            
            return personOption
                .Map(p => Ok(p) as IActionResult)
                .Reduce(NotFound);
        }

        return BadRequest();
    }

    private bool TryGetUsername(string value, out Username username)
    {
        try
        {
            var un = new Username(value);
            username = un;
            return true;
        }
        catch (Exception)
        {
            username = default!;
            return false;
        }
    }
}