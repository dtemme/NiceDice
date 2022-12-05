using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NiceDice.Model;
using NiceDice.Models;

namespace NiceDice.Pages;

public partial class Index
{
    private const string WordDiceStatesStorageKey = "WordDiceStates";
    private const string NumericDiceStatesStorageKey = "NumericDiceStates";
    private const string DarkModeStorageKey = "DarkMode";

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }
    [Inject]
    public IJSRuntime JsRuntime { get; set; }
    [Inject]
    public WordsRepository WordsRepository { get; set; }

    [Parameter]
    public string Context { get; set; }

    public IList<WordDice> WordDices { get; set; } = new List<WordDice>();
    public IList<NumericDice> NumericDices { get; set; } = new List<NumericDice>();

    public IEnumerable<BaseDice> ActiveDices => Context != "words" ? NumericDices : WordDices;

    public bool DarkMode { get; private set; } = true;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);
        await LoadStateAsync().ConfigureAwait(false);
    }

    public async Task RollDicesAsync()
    {
        foreach (var dice in ActiveDices)
            dice.IsRolling = true;
        for (var i = 0; i < 7; i++)
        {
            foreach (var dice in ActiveDices)
                dice.SetRandomValue();
            StateHasChanged();
            await Task.Delay(100).ConfigureAwait(false);
        }
        foreach (var dice in ActiveDices)
            dice.SetRandomValue();
        foreach (var dice in ActiveDices)
            dice.IsRolling = false;
        await SaveStateAsync().ConfigureAwait(false);
    }

    private async Task SaveStateAsync()
    {
        await LocalStorage.SetItemAsync(NumericDiceStatesStorageKey, NumericDices).ConfigureAwait(false);
        await LocalStorage.SetItemAsync(WordDiceStatesStorageKey, WordDices).ConfigureAwait(false);
        await WordsRepository.SaveStateAsync(LocalStorage).ConfigureAwait(false);
    }

    private async Task LoadStateAsync()
    {
        DarkMode = await LocalStorage.GetItemAsync<bool?>(DarkModeStorageKey).ConfigureAwait(false) ?? true;
        await SetDarkModeAsync(DarkMode).ConfigureAwait(false);

        await WordsRepository.LoadStateAsync(LocalStorage).ConfigureAwait(false);

        var wordDices = await LocalStorage.GetItemAsync<IList<WordDice>>(WordDiceStatesStorageKey).ConfigureAwait(false);
        if (wordDices == null)
        {
            wordDices = new List<WordDice>() { new(), new(), new() };
            foreach (var dice in wordDices)
            {
                dice.WordsRepository = WordsRepository;
                dice.SetRandomValue();
            }
        }
        WordDices = wordDices;
        foreach (var wordDice in WordDices)
            wordDice.WordsRepository = WordsRepository;

        var numericDices = await LocalStorage.GetItemAsync<IList<NumericDice>>(NumericDiceStatesStorageKey).ConfigureAwait(false);
        if (numericDices == null)
        {
            numericDices = new List<NumericDice>() { new(), new(), new() };
            foreach (var dice in numericDices)
                dice.SetRandomValue();
        }
        NumericDices = numericDices;
    }

    private async Task AddDiceAsync()
    {
        BaseDice newDice;
        if (ActiveDices == WordDices)
        {
            var dice = new WordDice();
            dice.WordsRepository = WordsRepository;
            WordDices.Add(dice);
            newDice = dice;
        }
        else
        {
            var dice = new NumericDice();
            NumericDices.Add(dice);
            newDice = dice;
        }
        newDice.SetRandomValue();
        await SaveStateAsync().ConfigureAwait(false);
    }

    private async Task RemoveDiceAsync()
    {
        if (!ActiveDices.Any())
            return;

        if (ActiveDices == WordDices)
            WordDices.RemoveAt(WordDices.Count - 1);
        else
            NumericDices.RemoveAt(NumericDices.Count - 1);

        await SaveStateAsync().ConfigureAwait(false);
    }

    public async Task ToggleDarkMode()
    {
        DarkMode = !DarkMode;
        await SetDarkModeAsync(DarkMode).ConfigureAwait(false);
        await LocalStorage.SetItemAsync(DarkModeStorageKey, DarkMode).ConfigureAwait(false);
        StateHasChanged();
    }

    private async Task SetDarkModeAsync(bool darkMode)
        => await JsRuntime.InvokeVoidAsync("setDarkMode", darkMode).ConfigureAwait(false);
}
