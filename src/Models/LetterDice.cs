﻿using System.Security.Cryptography;
using System.Text.Json.Serialization;
using NiceDice.Models;

namespace NiceDice.Model;

public class LetterDice : BaseDice<string>
{
    [JsonIgnore]
    public LetterRepository LetterRepository { get; set; }

    public override string Print() => Value;
    public override void SetRandomValue()
    {
        if (LetterRepository == null)
            return;
        var availavleLetters = LetterRepository.AvailableLetters;
        Value = availavleLetters[RandomNumberGenerator.GetInt32(0, availavleLetters.Count)];
    }
}