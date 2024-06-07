using LTHDotNetCore.Models;
using MudBlazor;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace LTHDotNetCore.BlazarWasm.Pages.BlogPage;

public partial class BlogCreate
{
    private BlogDataModel requestModel = new();
    private async Task Save()
    {
        var blogJson = JsonConvert.SerializeObject(requestModel);
        HttpContent content = new StringContent(blogJson, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await HttpClient.PostAsync("api/BlogEFCore", content);
        if (response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();
            //await JsRuntime.InvokeVoidAsync("alert", message);
            Snackbar.Add(message, Severity.Success);
            Nav.NavigateTo("/setup/blog");
        }
    }
}