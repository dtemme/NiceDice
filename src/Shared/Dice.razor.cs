﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NiceDice.Model;

namespace NiceDice.Shared;

public partial class Dice
{
    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public BaseDice DiceInfo { get; set; }

    [Parameter]
    public EventCallback OnSaveState { get; set; }

    [Parameter]
    public bool DarkMode { get; set; }

    [Parameter]
    public EventCallback<BaseDice> OnRemoveCounter { get; set; }

    private async Task RemoveCounterAsync()
    {
        if (await JSRuntime.InvokeAsync<bool>("confirm", "Wirklich löschen?").ConfigureAwait(false))
            await OnRemoveCounter.InvokeAsync(DiceInfo).ConfigureAwait(false);
    }
}
