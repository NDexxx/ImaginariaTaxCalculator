namespace ImaginariaTaxCalculator.Interfaces
{
    public interface ITaxesCacheHelper
    {
        public string BuildKey(ITaxPayer model);

        public bool DoesKeyExists(string key);

        public ITaxes GetItem(string key);

        public void SetItem(string key, ITaxes item);
    }
}