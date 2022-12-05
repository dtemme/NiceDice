using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace NiceDice.Models
{
    public class WordsRepository
    {
        private const string AvailableWordsStorageKey = "AvailableWords";

        public List<Word> AvailableWords { get; } = new() { "a", "e", "i", "o", "u" };

        public async Task SaveStateAsync(ILocalStorageService localStorage)
        {
            await localStorage.SetItemAsync(AvailableWordsStorageKey, AvailableWords).ConfigureAwait(false);
        }

        public async Task LoadStateAsync(ILocalStorageService localStorage)
        {
            var words = await localStorage.GetItemAsync<IEnumerable<Word>>(AvailableWordsStorageKey).ConfigureAwait(false);
            if (words != null && words.Any())
            {
                AvailableWords.Clear();
                AvailableWords.AddRange(words);
            }
        }
    }
}
