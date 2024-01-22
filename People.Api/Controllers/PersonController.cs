using Microsoft.AspNetCore.Http.HttpResults;
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
        try
        {
            var id = new Username(username);
            var personOption = await _provider.GetAsync(id);

            // not sure I like this... note to self look for a nicer way to optionally return from controller. 
            return personOption.Reduce(() => null) switch
            {
                null => NotFound(),
                Person p => Ok(p)
            };
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}