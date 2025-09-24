using Microsoft.AspNetCore.Mvc;
using WAChatFlow.Server.Infrastructure.Repositories.Contacts;
using WAChatFlow.Shared.Contacts;

namespace WAChatFlow.Server.Controllers.Contacts
{
    [ApiController]
    [Route("api/users")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsRepository _usersRepository;

        public ContactsController(IContactsRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet("catalog")]
        public async Task<IActionResult> GetCatalog(CancellationToken ct)
        {
            var result = await _usersRepository.GetUserStatusCatalogAsync(ct);

            return Ok(result);
        }

        [HttpPost("upsert-name")]
        public async Task<IActionResult> UpsertName([FromBody] NotificationUser dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                return BadRequest("PhoneNumber requerido.");
            }

            var idUser = await _usersRepository.UpsertByPhoneAsync(dto.PhoneNumber, dto.FullName ?? "", ct);
            return Ok(new { userId = idUser });
        }
    }
}
