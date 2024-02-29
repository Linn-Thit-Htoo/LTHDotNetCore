using Azure.Core;
using LTHDotNetCore.ConsoleApp.AdoDotNetExamples;
using LTHDotNetCore.ConsoleApp.HttpClientExamples;
using LTHDotNetCore.ConsoleApp.RefitExamples;
using LTHDotNetCore.ConsoleApp.RestClientExamples;

AdoDotNetExample2 adoDotNetExample = new();
adoDotNetExample.Run();

//HttpClientExample httpClientExample = new HttpClientExample();
//await httpClientExample.Run();

//RestClientExample restClientExample = new RestClientExample();
//await restClientExample.Run();


//Console.WriteLine("Please wait for api...");
//Console.ReadKey();

//RefitExample refitExample = new RefitExample();
//await refitExample.Run();

Console.ReadKey();