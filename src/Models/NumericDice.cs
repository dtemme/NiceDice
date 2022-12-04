using System.Security.Cryptography;

namespace NiceDice.Model;

public class NumericDice : BaseDice<int>
{
    public override string Print() => Value.ToString();
    public override void SetRandomValue() => Value = RandomNumberGenerator.GetInt32(1, 7);
}
