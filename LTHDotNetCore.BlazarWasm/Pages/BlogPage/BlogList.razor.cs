using LTHDotNetCore.BlazarWasm.Shared;
using LTHDotNetCore.Models;
using MudBlazor;
using Newtonsoft.Json;

namespace LTHDotNetCore.BlazarWasm.Pages.BlogPage;

public partial class BlogList
{
    private int _pageNo = 1;
    private int _pageSize = 10;

    private BlogListResponseModel? Model;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await List(_pageNo, _pageSize);
        }
    }

    private async Task List(int pageNo, int pageSize = 10)
    {
        _pageNo = pageNo;
        _pageSize = pageSize;
        var result = await HttpClient.GetAsync($"api/BlogEFCore/{pageNo}/{pageSize}");
        if (result.IsSuccessStatusCode)
        {
            var jsonStr = await result.Content.ReadAsStringAsync();
            Model = JsonConvert.DeserializeObject<BlogListResponseModel>(jsonStr)!;
            StateHasChanged();
        }
    }
    private async Task PageChanged(int i)
    {
        _pageNo = i;
        await List(_pageNo);
    }

    private void Edit(int id)
    {
        Nav.NavigateTo($"/setup/blog/edit/{id}");
    }

    private async Task Delete(int id)
    {
        var parameters = new DialogParameters<ConfirmDialog>
        {
            {
                x => x.Message,
                "Are you sure want to delete?"
            }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirm", parameters, options);
        var result = await dialog.Result;
        if (result.Canceled)
            return;

        var response = await HttpClient.DeleteAsync($"api/BlogEFCore/{id}");
        if (response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();
            Snackbar.Add(message, Severity.Success);
            await List(_pageNo, _pageSize);
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            Snackbar.Add(message, Severity.Error);
            await List(_pageNo, _pageSize);
        }
    }
}