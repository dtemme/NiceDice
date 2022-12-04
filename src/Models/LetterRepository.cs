using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace NiceDice.Models
{
    public class LetterRepository
    {
        private const string AvailableLettersStorageKey = "AvailableLetters";

        public List<string> AvailableLetters { get; } = new() { "a", "e", "i", "o", "u" };

        public async Task SaveStateAsync(ILocalStorageService localStorage)
        {
            await localStorage.SetItemAsync(AvailableLettersStorageKey, AvailableLetters).ConfigureAwait(false);
        }

        public async Task LoadStateAsync(ILocalStorageService localStorage)
        {
            var letters = await localStorage.GetItemAsync<IEnumerable<string>>(AvailableLettersStorageKey).ConfigureAwait(false);
            if (letters != null && letters.Any())
            {
                AvailableLetters.Clear();
                AvailableLetters.AddRange(letters);
            }
        }
    }
}
