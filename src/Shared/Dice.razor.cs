using Microsoft.AspNetCore.Components;
using NiceDice.Model;

namespace NiceDice.Shared;

public partial class Dice
{
    [Parameter]
    public BaseDice DiceInfo { get; set; }

    [Parameter]
    public bool DarkMode { get; set; }
}
