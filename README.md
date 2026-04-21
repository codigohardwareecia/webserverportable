Para entender o processo de unificação leia este documento e execute passo a passo, o código do projeto já incorpora todas as alterações e está funcional
##### Criando uma WebAPI

Crie uma WebAPI normalmente
##### Alterando o projeto

Editar arquivo do projeto e adicionar as duas linhas abaixo para habilitar o uso de formulário e permiter executar como se fosse um aplicativo normal.

```
<UseWindowsForms>true</UseWindowsForms>
<OutputType>WinExe</OutputType>
```

Mudar o TargetFramework para:

```
<TargetFramework>net10.0-windows</TargetFramework>
```

Depois de editado o arquivo .proj vai ficar assim
```
<Project Sdk="Microsoft.NET.Sdk.Web"> 
   <PropertyGroup>
		
		<TargetFramework>net6.0-windows</TargetFramework> 
	    <Nullable>enable</Nullable>
		<ImplicitUsings>enable</ImplicitUsings>
		<UseWindowsForms>true</UseWindowsForms> 
		<OutputType>WinExe</OutputType><EnableDefaultApplicationConfigurationFile>true</EnableDefaultApplicationConfigurationFile>
  </PropertyGroup>
</Project>
```

Fecha e abra novamente o projeto
##### Adicionando o Formulário Windows Forms

A opçao para adicionarmos um formulario vai aparecer

Faça um Rebuild no projeto para o Visual Studio reconhecer o SDK e o suporte a Windows Forms

Adicione um novo formulário que vai ser o formulário principal

Altere o Program.cs para trabalhar de maneira hibrida inserir as linhas abaixo na sequencia:

```CSharp
// 1. Iniciamos a WebAPI em uma thread separada (Background)
Task.Run(() => app.Run());

// 2. Iniciamos o WinForms na thread principal (UI Thread)
ApplicationConfiguration.Initialize();

// 3. Inicializa nosso formulário
Application.Run(new frmMain());

```

Seu Program.cs vai ficar conforme o exemplo a seguir
```CSharp
using WebApplication2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

// 1. Iniciamos a WebAPI em uma thread separada (Background)
await Task.Run(() => app.Run());

// 2. Iniciamos o WinForms na thread principal (UI Thread)
ApplicationConfiguration.Initialize();

// 3. Inicializa nosso formulário
Application.Run(new frmMain());

```

Faça um teste dando o play e verifiamos que o Form é carregado e o serviço da WebAPI também funciona

Adicionando suporte ao arquivos estáticos

Crie na raiz do projeto uma pasta chamadw wwwroot

Crie um arquivo do tipo html chamado index.html adicione qualquer texto ao html e salve dentro da pasta wwwroot

```HTML
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title></title>
</head>
<body>
    <h1>Hello World!</h1>
</body>
</html>
```

Clique com o botão direito do mouse sobre o arquibo Index.html e vá nas propriedades

Altere "Copy to Output Directory" para "Copy Aways"

Precisamos adicionar as duas linhas abaixo no nosso Program.cs

```
app.UseDefaultFiles();
app.UseStaticFiles();
```

O arquivo Program.cs vai ficar da seguinte forma até aqui:
```CSharp
using WebApplication2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();

app.UseStaticFiles();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

// 1. Iniciamos a WebAPI em uma thread separada (Background)
Task.Run(() => app.Run());

// 2. Iniciamos o WinForms na thread principal (UI Thread)
ApplicationConfiguration.Initialize();

// 3. Inicializa nosso formulário
Application.Run(new frmMain());

```

Teste a aplicaçao pela url fornecida no Console que se abre ao executar por ex:
 http://localhost:5194, ao abrir esse endereço no navegador deve ser possivel visualizar o conteúdo HTML

Atribuindo uma url de execução fixa

Precisamos atribuir uma url fiza para execuçao do nosso servidor web, ainda no Program.cs adicione a seguinte linha de código depois de var app = builder.Build().

```
builder.WebHost.UseUrls($"http://0.0.0.0:5000");
```

O Program.cs vai ficar assim:

```CSharp
using WebApplication2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

builder.WebHost.UseUrls($"http://0.0.0.0:5000");

app.UseDefaultFiles();

app.UseStaticFiles();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

// 1. Iniciamos a WebAPI em uma thread separada (Background)
Task.Run(() => app.Run());

// 2. Iniciamos o WinForms na thread principal (UI Thread)
ApplicationConfiguration.Initialize();

// 3. Inicializa nosso formulário
Application.Run(new frmMain());
```

Ao executar pode aparece uma caixa de diálogo de Segurança de Firewall clique em AllowAcesss, agora vá ao navegador e chame a url http://localhost:5000 é possivel ver novamente o conteúdo html

##### Criando a controller Home

Apague a controller existente que foi criado como exemplo

Vamos criar uma Controller do Zero para servir para as funções que vamos desenvolver para os próximos videos

Clique com o botão direito na pata Controller e clique em Add > Controller

Selecione MVC Controller > Empty e depois Add

Dê o nome de HomeController e clique em Add

Vamos alterar nossa classe da HomeController da seguinte forma

```CSharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Route("api/v1/[Controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("Status")]
        public IActionResult Status()
        {
            return Ok("On Line");
        }
    }
}

```

Execute o código e acesse http://localhost:5000/api/v1/Home/Status e verifique se o retorno ocorre, se retorna "On Line" está tudo Ok.
##### Adicionando suporte a JQuery e Bootstrap

O Jquery e bootstrap fornecem ferramentas de layout e manipulação de dados de forma fácil, é possivel usarmos um CDN que são urls disponiveis na internet, mas será necessário ter conexão com a internet. Vamos adicionar eses recursos ao nosso projeto nos nossos arquivos estáticos

Clique com o botão direito no seu projeto.
    
 Vá em **Add** -> **Client-Side Library...**
    
No Provider, escolha `filesystem` (se você já tiver os arquivos no PC) ou `cdnjs` (se estiver conectado agora mas vai rodar offline depois).
    
Digite `jquery` e `bootstrap`.
    
O Visual Studio vai baixar e colocar os arquivos certinhos na pasta `wwwroot/lib` para você.

Para que todos os arquivos se tornem disponiveis quando for excutado uma publicacao devemos alterer nosso arquivo de projeto e constar o trecho abaixo:

```
<ItemGroup> <Content Update="wwwroot\**\*"> <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory> <ExcludeFromSingleFile>true</ExcludeFromSingleFile> </Content> </ItemGroup>
```

Nosso arquivo de projeot vai ficar assim:

```XML
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net10.0-windows</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
	<UseWindowsForms>true</UseWindowsForms>
	<OutputType>Exe</OutputType>
  </PropertyGroup>

	<ItemGroup>
		<Content Update="wwwroot\**\*">
			<CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
			<ExcludeFromSingleFile>true</ExcludeFromSingleFile>
		</Content>
	</ItemGroup>

</Project>

```

Agora vamos alterar nossa index.html para:

```HTML
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" href="/lib/bootstrap/bootstrap.min.css">
</head>
<body>
    <div class="container mt-5">
        <h1 class="alert alert-info">Funcionando Offline!</h1>
        <button id="meuBotao" class="btn btn-success">Teste jQuery</button>
    </div>

    <script src="/lib/jquery/jquery.min.js"></script>
    <script src="/lib/bootstrap/bootstrap.bundle.min.js"></script>

    <script>
        $(document).ready(function() {
            $('#meuBotao').click(function() {
                alert('jQuery rodando localmente!');
            });
        });
    </script>
</body>
</html>
```

Acesse o endereço http://localhost:5000 para ver se deu certo, se ao clicar no botão a mensagem "JQuery Rodando" está funcionando perfeitamente

Agora vamos ver alterar nosso HTML para consultarmos nossa WebAPI e obter o status que por enquanto está estático.

```HTML
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" href="/lib/bootstrap/bootstrap.min.css">
</head>
<body>
    <div class="container mt-5">
        <h1 class="alert alert-info">Jarvis!</h1>
        <button id="btnStatus" class="btn btn-success">Verificar Status</button>
        <div id="status-display"></div>
    </div>

    <script src="/lib/jquery/jquery.min.js"></script>
    <script src="/lib/bootstrap/bootstrap.bundle.min.js"></script>

    <script>
        $(document).ready(function() {
            $('#btnStatus').click(function() {
                checarStatus();
            });
        });

        function checarStatus() {
            $.get('/api/v1/home/Status')
                .done(function (data) {
                    $('#status-display').text(data);
                })
                .fail(function () {
                    $('#status-display').text("DESCONECTADO");
                });
        }
    </script>
</body>
</html>
```

Ao clica no botão status podemos ver nosso retorno que deu certo

##### Adicionando WebView2 ao nosso projeto

Para usarmos nosso form vamos adicionar o componente WebView2 para também visualizarmos nossa página sem precisamos abrir o navegador

Vá em Tools > Nuget Package Manager > Manage Nuget Package for Solution

Vá em Browse e procure por WebView2 oui Microsoft.Web.Webview2 selecione e instale

Faça um Rebuild do projeto para ele atualizar os componentes

Abra o formulário em modo de design

Clique em View > ToolBox,

Procure logo no topo WebView2, arraste e dimencione até caber no formulário

Vamos alterar as propriedades de ancoragem para ele crescer junto com o formulário, vá em Anchor nas propriedades do WebView2 e marque todos os lados

De duplo clique em qualquer parte ativa do formulário, vai abrir o código, ele via criar o método frmMain_Load

```CSharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WebApplication2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            StartBrowser();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
     
        }

        private async void StartBrowser()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.Source = new Uri("http://127.0.0.1:5000");
        }
    }
}

```

Se por ventura der esse erro System.Runtime.InteropServices.COMException: 'Cannot change thread mode after it is set. (0x80010106 (RPC_E_CHANGED_MODE))'

Altere o arquivo Program.cs e altere a estrutura dele para ficar sem o TopLevel e da seguinte forma 

```CSharp
namespace WebApplication2
{
    public class Program
    {

        [STAThread] // <--- ESSENCIAL: Sem isso o WebView2 sempre dará erro de thread
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            builder.WebHost.UseUrls($"http://0.0.0.0:5000");

            app.UseDefaultFiles();

            app.UseStaticFiles();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.MapControllers();

            // 1. Iniciamos a WebAPI em uma thread separada (Background)
            Task.Run(() => app.Run());

            // 2. Iniciamos o WinForms na thread principal (UI Thread)
            ApplicationConfiguration.Initialize();

            // 3. Inicializa nosso formulário
            Application.Run(new frmMain());

        }
    }
}

```

Pronto, isso deve resolve o conflito de Threads e carregar o formulário

##### Portavel e Minimalista

Para este video já concluimos a codificação da base do nosso aplicativo hibrido, o que vai tornar ele portátil é o modo de publicação

Vamos gerar o minimo de arquivos possiveis

Vamos criar um perfil de publicacao

Clique com o botão direito no projeto e escolha Publish

Depois em Add a publish profile

Selecione Folder e clique em Next, mantenha os arquivos padrao e cliquem em Finish depois em close

Clique no link show all settings e altere

Deplosyment-Mode: Self-Contained
Target Runbtime:  winx-64

File publish Options
Marque produce single file
Marque Enable ReadToRun Compilation

Clique em Save

Clique em Publish

Depois clique no Target Location para abrir o diretório

Copie os arquivos .exe, WebView2Loader.dll e a pasta wwwroot

Execute o arquivo e é para funcionar

Pronto terminamos a base para os demis projetos que vamos fazer nos proximos videos


Um Exemplo de upload de arquivos para HTML 4

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 3.2 Final//EN">
<html>
<head>
    <title>Entrada de Dados</title>
</head>
<body bgcolor="#C0C0C0">

    <table border="1" cellpadding="5" cellspacing="0" bgcolor="#D4D0C8">
        <tr>
            <td>
                <font face="MS Sans Serif, Arial" size="2">
                   <form action="http://192.168.15.160:5000/api/v1/home/upload" method="POST" enctype="multipart/form-data">
                        <label>Selecione o arquivo:</label>
                        <input type="file" name="meuArquivo">
                        <br><br>
                        <input type="submit" value="Enviar para o WinForms">
                    </form>
                </font>
            </td>
        </tr>
    </table>

</body>
</html>

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

