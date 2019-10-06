namespace Prism.Ioc.Configurator.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ContainerBuilderTests
    {
        [TestMethod]
        public void When_update_is_called_then_class_should_be_registered()
        {
            // Arrange
            var systemUnderTest = new ContainerBuilder();
            systemUnderTest.RegisterType<TestService>().Where(type => type.Name.EndsWith("Service"));

            var containerMock = new Mock<IContainerRegistry>(MockBehavior.Strict);
            containerMock.Setup(container => container.Register(typeof(TestService), typeof(TestService)));

            // Act
            systemUnderTest.Update(containerMock.Object);

            // Assert
            containerMock.VerifyAll();
        }

        [TestMethod]
        public void When_update_is_called_then_class_should_be_registered_as_class()
        {
            // Arrange
            var systemUnderTest = new ContainerBuilder();
            systemUnderTest.RegisterType<TestService>().AsSelf();

            var containerMock = new Mock<IContainerRegistry>(MockBehavior.Strict);
            containerMock.Setup(container => container.Register(typeof(TestService), typeof(TestService)));

            // Act
            systemUnderTest.Update(containerMock.Object);

            // Assert
            containerMock.VerifyAll();
        }

        [TestMethod]
        public void When_update_is_called_then_class_should_be_registered_as_single_instance()
        {
            // Arrange
            var systemUnderTest = new ContainerBuilder();
            systemUnderTest.RegisterType<TestService>().SingleInstance();

            var containerMock = new Mock<IContainerRegistry>(MockBehavior.Strict);
            containerMock.Setup(container => container.RegisterSingleton(typeof(TestService), typeof(TestService)));

            // Act
            systemUnderTest.Update(containerMock.Object);

            // Assert
            containerMock.VerifyAll();
        }

        [TestMethod]
        public void When_update_is_called_then_class_should_be_registered_with_implemented_interface()
        {
            // Arrange
            var systemUnderTest = new ContainerBuilder();
            systemUnderTest.RegisterType<TestService>().AsImplementedInterfaces();

            var containerMock = new Mock<IContainerRegistry>(MockBehavior.Strict);
            containerMock.Setup(container => container.Register(typeof(ITestService), typeof(TestService)));

            // Act
            systemUnderTest.Update(containerMock.Object);

            // Assert
            containerMock.VerifyAll();
        }

        [TestMethod]
        public void When_update_is_called_then_class_should_not_be_registered()
        {
            // Arrange
            var systemUnderTest = new ContainerBuilder();
            systemUnderTest.RegisterType<TestService>().Where(type => type.Name.EndsWith("Manager"));

            var containerMock = new Mock<IContainerRegistry>(MockBehavior.Strict);

            // Act
            systemUnderTest.Update(containerMock.Object);

            // Assert
            containerMock.VerifyAll();
        }

        [TestMethod]
        public void When_update_is_called_then_only_services_should_be_registered()
        {
            // Arrange
            var systemUnderTest = new ContainerBuilder();
            systemUnderTest.RegisterAssemblyTypes(typeof(ServiceBase).Assembly).Where(type => type.Name.EndsWith("Service"));

            var containerMock = new Mock<IContainerRegistry>(MockBehavior.Strict);
            containerMock.Setup(container => container.Register(typeof(TestService), typeof(TestService)));

            // Act
            systemUnderTest.Update(containerMock.Object);

            // Assert
            containerMock.VerifyAll();
        }

        [TestMethod]
        public void When_update_is_called_then_a_instance_should_be_registered()
        {
            // Arrange
            var service = new TestService();
            var systemUnderTest = new ContainerBuilder();
            systemUnderTest.RegisterInstance(service);

            var containerMock = new Mock<IContainerRegistry>(MockBehavior.Strict);
            containerMock.Setup(container => container.RegisterInstance(typeof(TestService), service));

            // Act
            systemUnderTest.Update(containerMock.Object);

            // Assert
            containerMock.VerifyAll();
        }
    }
}
