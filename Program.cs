using Microsoft.Extensions.FileProviders;
using System.CommandLine;
var rootcmd=new RootCommand("FileServer");
var diropt=new Option<DirectoryInfo>(name:"--dir",description:"directory",getDefaultValue:() => new DirectoryInfo(Directory.GetCurrentDirectory()));
diropt.ExistingOnly();
var portopt=new Option<int>(name:"--port",description:"port",getDefaultValue: () => 8877);
rootcmd.AddOption(diropt);
rootcmd.AddOption(portopt);
rootcmd.SetHandler((diropt,portopt) => {
var builder = WebApplication.CreateBuilder();
builder.Services.AddDirectoryBrowser();
var app = builder.Build();
app.UseStaticFiles(new StaticFileOptions() {
	ServeUnknownFileTypes=true,
	DefaultContentType="text/plain",	
	FileProvider=new PhysicalFileProvider(diropt.FullName)
});
app.UseDirectoryBrowser(new DirectoryBrowserOptions() {
	FileProvider=new PhysicalFileProvider(diropt.FullName)
});
app.Run($"http://*:{portopt}/");
},diropt,portopt);
rootcmd.Invoke(args);

