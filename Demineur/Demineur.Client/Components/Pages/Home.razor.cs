using Microsoft.AspNetCore.Components;

namespace Demineur.Client.Components.Pages;

public class HomeBase : ComponentBase
{
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    
    public void GoToGamePage(int id)
    {
        _navigationManager.NavigateTo($"/Game/{id}");
    }
}
