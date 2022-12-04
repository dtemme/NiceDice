using System;
using System.Text.Json.Serialization;

namespace NiceDice.Model;

public abstract class BaseDice
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonIgnore]
    public bool IsRolling { get; set; }

    public abstract void SetRandomValue();
    public abstract string Print();
}

public abstract class BaseDice<T> : BaseDice
{
    public T Value { get; set; }
}
