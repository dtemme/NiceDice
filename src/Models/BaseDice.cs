using System.Text.Json.Serialization;

namespace NiceDice.Model;

public abstract class BaseDice
{
    [JsonIgnore]
    public bool IsRolling { get; set; }

    public abstract void SetRandomValue();
    public abstract string Print();
}

public abstract class BaseDice<T> : BaseDice
{
    public T Value { get; set; }
}
