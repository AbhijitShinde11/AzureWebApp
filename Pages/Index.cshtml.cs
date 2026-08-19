using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureWebApp.Pages;

public class IndexModel(IConfiguration config) : PageModel
{
    private readonly IConfiguration _config = config;

    public void OnGet()
    {
        ViewData["AppSettingsGreeting"] = _config["Greeting"];
    }
}
