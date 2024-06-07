using Azure.Core;
using LTHDotNetCore.ConsoleApp.DapperExamples;
using LTHDotNetCore.RestApi.AdoDotNetExamples;
using LTHDotNetCore.RestApi.HttpClientExamples;
using LTHDotNetCore.RestApi.RefitExamples;
using LTHDotNetCore.RestApi.RestClientExamples;

//AdoDotNetExample2 adoDotNetExample = new();
//adoDotNetExample.Run();

DapperExample2 dapper = new();
dapper.Run();

//HttpClientExample httpClientExample = new HttpClientExample();
//await httpClientExample.Run();

//RestClientExample restClientExample = new RestClientExample();
//await restClientExample.Run();


//Console.WriteLine("Please wait for api...");
//Console.ReadKey();

//RefitExample refitExample = new RefitExample();
//await refitExample.Run();

Console.ReadKey();