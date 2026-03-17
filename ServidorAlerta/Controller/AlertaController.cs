using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ServidorAlerta.Infra;

namespace ServidorAlerta.Controller
{

    [ApiController]
    [Route("alerta")]
    public class AlertaController : ControllerBase
    {
        
        private readonly IHubContext<AlertaHub> _hub;

        public AlertaController(IHubContext<AlertaHub> hub)
        {
            _hub = hub;
        }

        [HttpPost("enviar")]
        public async Task<IActionResult> EnviarMensagem(string mensagem)
        {
            await _hub.Clients.All.SendAsync("ReceberMensagem", mensagem);
            return Ok();
        }

    }
}