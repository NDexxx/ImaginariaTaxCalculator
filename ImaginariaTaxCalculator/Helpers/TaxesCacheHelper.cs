using ImaginariaTaxCalculator.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ImaginariaTaxCalculator.Helpers
{
    public class TaxesCacheHelper : ITaxesCacheHelper
    {
        private readonly IMemoryCache _cache;

        public TaxesCacheHelper(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string BuildKey(ITaxPayer model)
        {
            return $"{model.SSN}-{model.GrossIncome}-{model.CharitySpent}";
        }

        public bool DoesKeyExists(string key)
        {
            object cachedItem = _cache.Get(key);
            return cachedItem != null;
        }

        public ITaxes GetItem(string key)
        {
            var cachedItem = (ITaxes)_cache.Get(key);
            return cachedItem;
        }

        public void SetItem(string key, ITaxes item)
        {
            _cache.Set(key, item);
        }
    }
}