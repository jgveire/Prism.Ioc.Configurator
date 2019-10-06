namespace Prism.Ioc.Configurator.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ContainerBuilderTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var systemUnderTest = new ContainerBuilder();
            systemUnderTest.RegisterType<TestService>().AsImplementedInterfaces();

            //Prism.
            //IContainerRegistry container = new IContainerRegistry;
            //// Act
            //systemUnderTest.Update(container);
        }
    }
}
