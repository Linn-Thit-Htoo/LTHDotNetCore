using LTHDotNetCore.Models;
using Newtonsoft.Json;

namespace LTHDotNetCore.BlazarWasm.Pages.Blog
{
    public partial class PageBlog
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

        private async Task List(int pageNo, int pageSize)
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
    }
}
