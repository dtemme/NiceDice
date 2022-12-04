using System.Security.Cryptography;
using System.Text;

namespace NiceDice.Model;

public class NumericDice : BaseDice<int>
{
    public override string Print()
    {
        var result = new StringBuilder();
        switch (Value)
        {
            case 1:
                result.AppendLine("""<div class="dice_dot dice_dot_cc"></div>""");
                break;
            case 2:
                result.AppendLine("""<div class="dice_dot dice_dot_lt"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_rb"></div>""");
                break;
            case 3:
                result.AppendLine("""<div class="dice_dot dice_dot_lt"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_cc"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_rb"></div>""");
                break;
            case 4:
                result.AppendLine("""<div class="dice_dot dice_dot_lt"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_lb"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_rt"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_rb"></div>""");
                break;
            case 5:
                result.AppendLine("""<div class="dice_dot dice_dot_lt"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_lb"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_cc"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_rt"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_rb"></div>""");
                break;
            case 6:
                result.AppendLine("""<div class="dice_dot dice_dot_lt"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_lc"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_lb"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_rt"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_rc"></div>""");
                result.AppendLine("""<div class="dice_dot dice_dot_rb"></div>""");
                break;
            default:
                break;
        }
        return result.ToString();
    }

    public override void SetRandomValue() => Value = RandomNumberGenerator.GetInt32(1, 7);
}
