namespace NiceDice.Models;

public class Word
{
    public Word(string value) => Value = value;

    public string Value { get; set; }

    public static implicit operator string(Word value) => value?.Value;
    public static implicit operator Word(string value) => new(value);
}
