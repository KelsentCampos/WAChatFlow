using Microsoft.AspNetCore.Mvc;
using WAChatFlow.Server.Repositories.Users;
using WAChatFlow.Shared.Models.Users;

namespace WAChatFlow.Server.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
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
