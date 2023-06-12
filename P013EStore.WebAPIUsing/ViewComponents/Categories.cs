using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;


namespace P013EStore.WebAPIUsing.ViewComponents
{
    public class Categories : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public Categories(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private readonly string _apiAdres = "https://localhost:7011/api/Categories";

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Categories>>(_apiAdres);
            return View(model);
        }

    }


}
