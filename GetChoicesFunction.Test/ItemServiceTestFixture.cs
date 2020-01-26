using Adventure.Core.Exceptions;
using Adventure.Persistence;
using Adventure.Persistence.Containers;
using GetChoicesFunction.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace GetChoicesFunction.Test
{
    public class ItemServiceTestFixture
    {
        private static string partitionKey = Guid.NewGuid().ToString();

        public static IEnumerable<object[]> ItemList()
        {
            yield return new object[]
            {
                new List<Doughnut> 
                {
                    new Doughnut { Id = partitionKey, ParenId = null, Value = "Parent" },
                    new Doughnut { Id = Guid.NewGuid().ToString(), ParenId = partitionKey, Value = "Child1" },
                    new Doughnut { Id = Guid.NewGuid().ToString(), ParenId = partitionKey, Value = "Child2" },
                    new Doughnut { Id = Guid.NewGuid().ToString(), ParenId = Guid.NewGuid().ToString(), Value = "Child3" }
                }
            };
        }

        [Theory]
        [MemberData(nameof(ItemList))]
        public async Task TestGet_GivenPartitionKey_ReturnsItems(List<Doughnut> items)
        {
            var repository = new Mock<IGenericRepository<Doughnut>>();
            repository.Setup(i => i.GetAll(It.IsAny<string>()))
                .ReturnsAsync(items.AsQueryable().Where(i => i.ParenId == partitionKey).OrderBy(i => i.ParenId));

            var itemService = new ItemService(repository.Object);
            var children = await itemService.Get(partitionKey);

            Assert.NotNull(children);
            Assert.Equal(items.Where(i => i.ParenId == partitionKey), children);

            repository.Verify(i => i.GetAll(It.IsAny<string>()), Times.Once);
            repository.VerifyAll();
        }

        [Fact]
        public async Task TestGetRoot_WhenRootNotExists_ReturnsException()
        {
            var items = new List<Doughnut>
            {
                new Doughnut { Id = partitionKey, ParenId = Guid.NewGuid().ToString(), Value = "Parent" },
                new Doughnut { Id = Guid.NewGuid().ToString(), ParenId = partitionKey, Value = "Child1" }
            };
            var repository = new Mock<IGenericRepository<Doughnut>>();
            repository.Setup(i => i.GetWhere(It.IsAny<Expression<Func<Doughnut, bool>>>(), It.IsAny<string>()))
                .ReturnsAsync(items.AsQueryable().Where(i => i.ParenId == null).OrderBy(i => i.ParenId));


            var itemService = new ItemService(repository.Object);
            await Assert.ThrowsAsync<ItemNotFoundException>(() => itemService.GetRoot());
        }

        [Theory]
        [MemberData(nameof(ItemList))]
        public async Task TestGetRoot_ReturnsRootItem(List<Doughnut> items)
        {
            var repository = new Mock<IGenericRepository<Doughnut>>();
            repository.Setup(i => i.GetWhere(It.IsAny<Expression<Func<Doughnut, bool>>>(), It.IsAny<string>()))
              .ReturnsAsync(items.AsQueryable().Where(i => i.ParenId == null).OrderBy(i => i.ParenId));

            var itemService = new ItemService(repository.Object);
            var rootNode = await itemService.GetRoot();

            Assert.NotNull(rootNode);
            Assert.True(rootNode.Count() == 1);

            repository.Verify(i => i.GetWhere(It.IsAny<Expression<Func<Doughnut, bool>>>(), It.IsAny<string>()), Times.Once);
            repository.VerifyAll();
        }
    }
}
