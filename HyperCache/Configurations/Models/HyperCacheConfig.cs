namespace HyperCache.Configurations.Models
{
    public class HyperCacheConfig
    {
        private readonly ConfigurationValue absoluteTimeExpiration;

        private readonly ConfigurationValue slidingTimeExpiration;

        public HyperCacheConfig(ConfigurationValue absoluteTimeExpiration, ConfigurationValue slidingTimeExpiration)
        {
            this.absoluteTimeExpiration = absoluteTimeExpiration;
            this.slidingTimeExpiration = slidingTimeExpiration;
        }

        public ConfigurationValue AbsoluteTimeExpiration => absoluteTimeExpiration;

        public ConfigurationValue SlidingTimeExpiration => slidingTimeExpiration;
    }
}
