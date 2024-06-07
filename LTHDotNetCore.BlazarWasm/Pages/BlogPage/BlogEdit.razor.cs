using LTHDotNetCore.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace LTHDotNetCore.BlazarWasm.Pages.BlogPage;

public partial class BlogEdit
{
    [Parameter]
    public int id { get; set; }

    private BlogDataModel item;
    protected override async Task OnInitializedAsync()
    {
        var result = await HttpClient.GetAsync("api/BlogEFCore/" + id);
        if (result.IsSuccessStatusCode)
        {
            string jsonStr = await result.Content.ReadAsStringAsync();
            item = JsonConvert.DeserializeObject<BlogDataModel>(jsonStr)!;
        }
    }
    private async Task Update()
    {
        try
        {
            string jsonStr = JsonConvert.SerializeObject(item);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await HttpClient.PutAsync($"api/BlogEFCore/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                Snackbar.Add(message, Severity.Success);
                Nav.NavigateTo("/setup/blog");
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                Snackbar.Add(message, Severity.Error);
                Nav.NavigateTo("/setup/blog");
            }
        }
        catch
        {
            throw;
        }
    }
}
