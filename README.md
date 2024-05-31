<div>
	<div>
		<img src=https://raw.githubusercontent.com/Byron2016/00_forImages/main/images/Logo_01_00.png align=left alt=MyLogo width=200>
	</div>
	&nbsp;
	<div>
		<h1>079_CSharp_PerfectConsoleApp</h1>
	</div>
</div>

&nbsp;

# Table of contents

---

- [Table of contents](#table-of-contents)
- [Project Description](#project-description)
- [Technologies used](#technologies-used)
- [References](#references)
- [Steps](#steps)

[⏪(Back to top)](#table-of-contents)

# Project Description

---

[⏪(Back to top)](#table-of-contents)
&nbsp;

# Technologies used

---

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![VisualStudio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)

[⏪(Back to top)](#table-of-contents)

# References

---

- Shields.io

  - [Shields.io](https://shields.io/)

  - [Github Ileriayo markdown-badges](https://github.com/Ileriayo/markdown-badges)

  - [Github Ileriayo markdown-badges WebSite](https://ileriayo.github.io/markdown-badges/)

[⏪(Back to top)](#table-of-contents)

# Steps

- IAmTimCorey: The Perfect C# Console Application...Or Not.
	- https://www.youtube.com/watch?v=dZSLm4tOI8o			
		
		- NET 8:
			- Crear un nuevo proyecto 
				- ASP.NET Core console 
					
		- Agregar DI al proyecto 
			- Agregar estos paquetes:
				- Microsoft.Extensions.DependencyInjection
				- Microsoft.Extensions.Hosting
		
		- Agregar a la solución una biblioteca "Class Library" llamada "HelloWorldLibrary" 
		- Referenciar esta librería al proyecto.
		- Agregar un nuevo item de tipo "json" llamado "CustomText.json"
			[
			{
				"language": "en",
				"translations": {
				"Greeting": "Hello World"
				}
			},
			{
				"language": "es",
				"translations": {
				"Greeting": "Hola Mundo"
				}
			}
			]
		
			- Incluir en el build 
				- Botón derecho sobre el archivo seleccionar "propiedades"
				- En "Copy to output Directory" seleccionar "copy if newer"
			- Agregar carpeta "Models"
				- Agregar clase "CustomText"
				
					namespace HelloWorldLibrary.Models;
					
					public class CustomText
					{
						public string Language { get; set; }
						public Dictionary<string, string> Translations { get; set; }
					}
				
				
			- Agregar carpeta "BusinessLogic"
				- Agregar clase "Messages"
				- Instalar paquete "Microsoft.Extensions.Logging.Abstractions;"
				
					using HelloWorldLibrary.Models;
					using Microsoft.Extensions.Logging;
					using System.Text.Json;
					
					namespace HelloWorldLibrary.BusinessLogic;
					
					public class Messages : IMessages
					{
						private readonly ILogger<Messages> _log;
					
						public Messages(ILogger<Messages> log)
						{
							_log = log;
						}
					
						public string Greeting(string language)
						{
							string output = LookUpCustomText("Greeting", language);
							//string output = LookUpCustomText(nameof(Greeting), language);
							return output;
						}
					
						private string LookUpCustomText(string key, string language)
						{
							JsonSerializerOptions options = new JsonSerializerOptions()
							{
								PropertyNameCaseInsensitive = true,
							};
					
							/*
							// También se podía poner así:
							JsonSerializerOptions options = new()
							{
								PropertyNameCaseInsensitive = true,
							};
							*/
					
							try
							{
								List<CustomText>? messageSets = JsonSerializer
								.Deserialize<List<CustomText>>
								(
									File.ReadAllText("CustomText.json"), options
								);
					
								CustomText? messages = messageSets?.Where(x => x.Languge == language).First();
					
								if (messages is null)
								{
									throw new NullReferenceException("The specified language was not found in json file");
								}
					
								return messages.Transltations[key];
							}
							catch (Exception ex)
							{
								_log.LogError("Error lookin up the custom text", ex);
								throw;
							}
						}
					
					
					}

				
				- De la clase "Messages" crear una interfase "IMessages"
				
		- Agregar al proyecto la clase App 
			using HelloWorldLibrary.BusinessLogic;
			
			namespace _079_CSharp_PerfectConsoleApp
			{
				public class App
				{
					private readonly IMessages _messages;
			
					public App(IMessages messages)
					{
						_messages = messages;
					}
			
					public void Run(string[] args)
					{
						string lang = "en";
			
						for(int i=0; i < args.Length; i++)
						{
							if (args[i].ToLower().StartsWith("=-lang="))
							{
								lang = args[i].Substring(6);
								break;
							}
						}
			
						string message = _messages.Greeting(lang);
			
						Console.WriteLine(message);
					}
				}
			}				
			
		- program.cs
			using _079_CSharp_PerfectConsoleApp;
			using HelloWorldLibrary.BusinessLogic;
			using Microsoft.Extensions.DependencyInjection;
			using Microsoft.Extensions.Hosting;
			
			using IHost host = CreateHostBuilder(args).Build();
			
			// Con este using acá para decirle a la aplicación qu cuando nosotros
			// cerrermos la aplicación, estar seguros del dispose adecuado de esta variable host.
			// Va a crear un scope para DI el cual es basicamente la aplicación entera
			// eso determina por nosotros tiempo de vida y cosas como esas.
			using var scope = host.Services.CreateScope();
			
			var services = scope.ServiceProvider;
			
			try
			{
				services.GetRequiredService<App>().Run(args);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			
			
			static IHostBuilder CreateHostBuilder(string[] args)
			{
				return Host.CreateDefaultBuilder(args)
					.ConfigureServices((_, serices) =>
					{
						serices.AddSingleton<IMessages, Messages>();
						serices.AddSingleton<App>();
					});
			}


		- Agregar UnitTest 
			- Add a xUnit Test Project
				- Name: HelloWorldTestProject
				
			- Add a reference to "HelloWorldLibrary" 
			- Add a Class "MessagesTest" inside folder "BusinessLogic"
			
		- Running from command line 
			- Go to: "..../079_CSharp_PerfectConsoleApp/bin/Debug/net8.0 "
			- run: ./079_CSharp_PerfectConsoleApp.exe -lang=es
				- Error: Unhandled exception. System.FormatException: The short switch '-lang=es' is not defined in the switch mappings.
					- Cambiar en " App/Run"
						- DE: if (args[i].ToLower().StartsWith("-lang=")){lang = args[i].Substring(6);
						- A: if (args[i].ToLower().StartsWith("lang=")){lang = args[i].Substring(5);
						
		- Running from debug grap interfase 
			- Ir a las propiedades del proyecto, sección debug, ahí nos crea un archivo "Launchsettings.json" en donde está el parámetro de entrada: "commandLineArgs": "lang=frr"
				- 

---

[⏪(Back to top)](#table-of-contents)
