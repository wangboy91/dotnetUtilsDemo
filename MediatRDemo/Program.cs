// See https://aka.ms/new-console-template for more information

using MediatR;
using MediatRDemo;
using Microsoft.Extensions.DependencyInjection;


Console.WriteLine("Hello, World!");
var services = new ServiceCollection();
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
services.AddSingleton<IPushDemo, PushDemo>();
var serviceProvider = services.BuildServiceProvider();
var pushDemo = serviceProvider.GetService<IPushDemo>();
pushDemo?.PushData();