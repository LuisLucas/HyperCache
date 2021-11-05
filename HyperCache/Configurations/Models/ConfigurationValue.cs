namespace HyperCache.Configurations.Models
{
    public class ConfigurationValue
    {
        private readonly bool hasConfig;
        private readonly int value;

        public ConfigurationValue(bool hasConfig, int value)
        {
            this.hasConfig = hasConfig;
            this.value = value;
        }

        public bool HasConfiguration => hasConfig;

        public int Value => value;
    }
}
