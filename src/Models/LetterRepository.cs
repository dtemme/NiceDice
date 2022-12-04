using System.Collections.Generic;

namespace NiceDice.Models
{
    public class LetterRepository
    {
        public List<string> AvailableLetters { get; } = new() { "a", "e", "i", "o", "u" };
    }
}
