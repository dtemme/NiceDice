﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NiceDice.Model;
using NiceDice.Models;

namespace NiceDice.Pages;

public partial class Index
{
    private const string LetterDiceStatesStorageKey = "LetterDiceStates";
    private const string NumericDiceStatesStorageKey = "NumericDiceStates";
    private const string DarkModeStorageKey = "DarkMode";

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }
    [Inject]
    public IJSRuntime JsRuntime { get; set; }
    [Inject]
    public LetterRepository LetterRepository { get; set; }

    [Parameter]
    public string Context { get; set; }

    public IList<LetterDice> LetterDices { get; set; } = new List<LetterDice>();
    public IList<NumericDice> NumericDices { get; set; } = new List<NumericDice>();

    private IEnumerable<BaseDice> activeDices => Context != "letters" ? NumericDices : LetterDices;

    public bool DarkMode { get; private set; } = true;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync().ConfigureAwait(false);
        await LoadStateAsync().ConfigureAwait(false);
    }

    public async Task RollDicesAsync()
    {
        foreach (var dice in activeDices)
            dice.IsRolling = true;
        for (var i = 0; i < 7; i++)
        {
            foreach (var dice in activeDices)
                dice.SetRandomValue();
            StateHasChanged();
            await Task.Delay(100).ConfigureAwait(false);
        }
        foreach (var dice in activeDices)
            dice.SetRandomValue();
        foreach (var dice in activeDices)
            dice.IsRolling = false;
    }

    private async Task SaveStateAsync()
    {
        await LocalStorage.SetItemAsync(NumericDiceStatesStorageKey, NumericDices).ConfigureAwait(false);
        await LocalStorage.SetItemAsync(LetterDiceStatesStorageKey, LetterDices).ConfigureAwait(false);
        await LetterRepository.SaveStateAsync(LocalStorage).ConfigureAwait(false);
    }

    private async Task LoadStateAsync()
    {
        DarkMode = await LocalStorage.GetItemAsync<bool?>(DarkModeStorageKey).ConfigureAwait(false) ?? true;
        await SetDarkModeAsync(DarkMode).ConfigureAwait(false);

        await LetterRepository.LoadStateAsync(LocalStorage).ConfigureAwait(false);

        var letterDices = await LocalStorage.GetItemAsync<IList<LetterDice>>(LetterDiceStatesStorageKey).ConfigureAwait(false);
        if (letterDices != null)
        {
            LetterDices = letterDices;
            foreach (var letterDice in LetterDices)
                letterDice.LetterRepository = LetterRepository;
        }

        var numericDices = await LocalStorage.GetItemAsync<IList<NumericDice>>(NumericDiceStatesStorageKey).ConfigureAwait(false);
        if (numericDices != null)
            NumericDices = numericDices;
    }

    private async Task AddNumericDiceAsync()
    {
        var newDice = new NumericDice();
        newDice.SetRandomValue();
        NumericDices.Add(newDice);
        await SaveStateAsync().ConfigureAwait(false);
    }

    private async Task AddLetterDiceAsync()
    {
        var newDice = new LetterDice();
        newDice.LetterRepository = LetterRepository;
        newDice.SetRandomValue();
        LetterDices.Add(newDice);
        await SaveStateAsync().ConfigureAwait(false);
    }

    private async Task RemoveNumericDiceAsync(BaseDice dice)
    {
        if (dice is not NumericDice letterDice)
            return;
        NumericDices.Remove(letterDice);
        await SaveStateAsync().ConfigureAwait(false);
    }

    private async Task RemoveLetterDiceAsync(BaseDice dice)
    {
        if (dice is not LetterDice letterDice)
            return;
        LetterDices.Remove(letterDice);
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

    /*
    private async Task RenameLetterDiceAsync(LetterDice dice)
    {
        var newName = await JSRuntime.InvokeAsync<string>("prompt", "Name", dice.Name).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(newName))
        {
            dice.Name = newName;
            await OnSaveState.InvokeAsync().ConfigureAwait(false);
        }
    }
    */
}
