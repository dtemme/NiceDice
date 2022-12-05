using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NiceDice.Models;

namespace NiceDice.Shared;

public partial class Settings
{
    [Parameter]
    public EventCallback OnSaveChanges { get; set; }

    [Inject]
    public WordsRepository WordsRepository { get; set; }

    [Inject]
    public IJSRuntime JsRuntime { get; set; }

    private async Task RemoveWordAsync(Word word)
    {
        if (WordsRepository.AvailableWords.Count <= 1)
        {
            await JsRuntime.InvokeVoidAsync("alert", "Ein Wort muss bestehen bleiben.", word.Value).ConfigureAwait(false);
            return;
        }
        WordsRepository.AvailableWords.Remove(word);
        await OnSaveChanges.InvokeAsync().ConfigureAwait(false);
    }

    private async Task AddWordAsync()
    {
        var newWord = await JsRuntime.InvokeAsync<string>("prompt", "Neues Wort", "a").ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(newWord))
        {
            WordsRepository.AvailableWords.Add(newWord);
            await OnSaveChanges.InvokeAsync().ConfigureAwait(false);
        }
    }

    private async Task EditWordAsync(Word word)
    {
        var newWord = await JsRuntime.InvokeAsync<string>("prompt", "Wort ändern", word.Value).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(newWord))
        {
            word.Value = newWord;
            await OnSaveChanges.InvokeAsync().ConfigureAwait(false);
        }
    }
}
