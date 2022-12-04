using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NiceDice.Model;

public abstract class BaseDice
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonIgnore]
    public Action AfterValueChanged { get; set; }

    [JsonIgnore]
    public bool IsRolling { get; set; }

    public abstract void SetRandomValue();
    public abstract string Print();

    public async Task RollAsync()
    {
        IsRolling = true;
        for (var i = 0; i < 7; i++)
        {
            SetRandomValue();
            AfterValueChanged?.Invoke();
            await Task.Delay(100).ConfigureAwait(false);
        }
        SetRandomValue();
        IsRolling = false;
    }
}

public abstract class BaseDice<T> : BaseDice
{
    public T Value { get; set; }
}
