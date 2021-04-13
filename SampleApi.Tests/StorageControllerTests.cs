using Moq;
using SampleApi.Controllers;
using System;
using System.IO.Abstractions;
using Xunit;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;

namespace SampleApi.Tests
{
    public class StorageControllerTests
    {
        readonly Mock<IFileSystem> _fileSystem;
        readonly Fixture _fixture;
        public StorageControllerTests()
        {
            _fixture = new Fixture();
            _fileSystem = new Mock<IFileSystem>();
            _fileSystem.Setup(f => f.Directory.CreateDirectory(It.IsAny<String>())).Verifiable();
            _fileSystem.Setup(f => f.File.Delete(It.IsAny<String>())).Verifiable();
            _fileSystem.Setup(f => f.File.WriteAllText(It.IsAny<String>(),It.IsAny<String>())).Verifiable();
            _fileSystem.Setup(f => f.File.ReadAllText(It.IsAny<String>())).Returns(_fixture.Create<String>());
        }

        [Fact]
        public void Get_FolderDoesNotExists_Returns_OK()
        {
            _fileSystem.Setup(f => f.Directory.Exists(It.IsAny<String>())).Returns(false);
            var controller = new StorageController(_fileSystem.Object);
            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);

            _fileSystem.Verify(f => f.Directory.CreateDirectory(It.IsAny<String>()), Times.Once);
            _fileSystem.Verify(f => f.File.Delete(It.IsAny<String>()), Times.Never);
            _fileSystem.Verify(f => f.File.WriteAllText(It.IsAny<String>(),It.IsAny<String>()), Times.Once);
            _fileSystem.Verify(f => f.File.ReadAllText(It.IsAny<String>()), Times.Once);
        }

        [Fact]
        public void Get_FolderExists_FileDoesNotExists_Returns_OK()
        {
            _fileSystem.Setup(f => f.Directory.Exists(It.IsAny<String>())).Returns(true);
            _fileSystem.Setup(f => f.File.Exists(It.IsAny<String>())).Returns(false);
            var controller = new StorageController(_fileSystem.Object);
            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);

            _fileSystem.Verify(f => f.Directory.CreateDirectory(It.IsAny<String>()), Times.Once);
            _fileSystem.Verify(f => f.File.Delete(It.IsAny<String>()), Times.Never);
            _fileSystem.Verify(f => f.File.WriteAllText(It.IsAny<String>(), It.IsAny<String>()), Times.Once);
            _fileSystem.Verify(f => f.File.ReadAllText(It.IsAny<String>()), Times.Once);
        }

        [Fact]
        public void Get_FolderExists_FileExists_Returns_OK()
        {
            _fileSystem.Setup(f => f.Directory.Exists(It.IsAny<String>())).Returns(true);
            _fileSystem.Setup(f => f.File.Exists(It.IsAny<String>())).Returns(true);
            var controller = new StorageController(_fileSystem.Object);
            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);

            _fileSystem.Verify(f => f.Directory.CreateDirectory(It.IsAny<String>()), Times.Once);
            _fileSystem.Verify(f => f.File.Delete(It.IsAny<String>()), Times.Once);
            _fileSystem.Verify(f => f.File.WriteAllText(It.IsAny<String>(), It.IsAny<String>()), Times.Once);
            _fileSystem.Verify(f => f.File.ReadAllText(It.IsAny<String>()), Times.Once);
        }
    }
}

    