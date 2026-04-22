using Microsoft.AspNetCore.Mvc;

namespace MyPersonalWebServer.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HomeController : ControllerBase
    {
        public static event Action<string>? OnSendCommand;

        [HttpGet("About")]
        public IActionResult About()
        {
            return Ok("Hello World!");
        }

        [HttpGet("Status")]
        public IActionResult Status()
        {
            //OnSendCommand?.Invoke("Tudo certo por aqui");
            //var nome = DataShare.Instance.Nome;
            return Ok("Oi tudo bem!");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> ReceberArquivo(IFormFile meuArquivo)
        {
            if (meuArquivo == null || meuArquivo.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            string nomeComExtensao = Path.GetFileName(meuArquivo.FileName);

            var path = Path.Combine(@$"D:\", nomeComExtensao);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await meuArquivo.CopyToAsync(stream);
            }

            return Ok($"Arquivo salvo em: {path}");
        }
    }
}
