namespace HyperCacheUnitTests
{
    using AutoFixture;

    public class BaseTest
    {
        protected Fixture Fixture;

        public BaseTest()
        {
            this.Fixture = new Fixture();
        }
    }
}
