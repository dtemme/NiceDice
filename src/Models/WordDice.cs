using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using NiceDice.Models;

namespace NiceDice.Model;

public class WordDice : BaseDice<string>
{
    [JsonIgnore]
    [Inject]
    public WordsRepository WordsRepository { get; set; }

    public override string Print() => $"""<div class="word_dice">{Value}</div>""";
    public override void SetRandomValue()
    {
        if (WordsRepository == null)
            return;
        var availavleWords = WordsRepository.AvailableWords;
        Value = availavleWords[RandomNumberGenerator.GetInt32(0, availavleWords.Count)].Value;
    }
}
