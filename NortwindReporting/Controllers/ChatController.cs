using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthiwindModels.DTO;
using NorthwindService;

namespace NortwindReporting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController(IChatService chatService) : ControllerBase
    {

        [HttpPost]
        [Route("askme")]
        public async Task<IActionResult> AskMe([FromBody] ChatRequest chatRequest)
        {
            var response = await chatService.AskAsync(chatRequest.Message);
            return Ok(response);
        }

    }
}
