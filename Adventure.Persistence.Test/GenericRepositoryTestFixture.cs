using Adventure.Persistence.Providers;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Adventure.Persistence.Test
{
    public class GenericRepositoryTestFixture
    {
        private const int TestDocumentCount = 15;

        private readonly GenericRepository<TestDocument> repository;

        private readonly Random random = new Random();

        public GenericRepositoryTestFixture(IDocumentClientFactory documentClientFactory)
        {
            this.repository = new GenericRepository<TestDocument>(documentClientFactory, new DatabaseProvider(documentClientFactory));
        }

        public async Task RunOrderedTest()
        {
            await this.CreateQuery();

            await this.GetAllEmpty();

            await this.Create();

            await this.GetAll();

            await this.GetWhere();

            await this.GetPredicate();

            await this.GetId();

            await this.Update();

            await this.Delete();

            await this.GetNonExistent();

            await this.UpdateNonExistent();

            await this.DeleteNonExistent();
        }

        private async Task CreateQuery()
        {
            var query = await this.repository.CreateDocumentQuery();

            Assert.NotNull(query);
        }

        private async Task DeleteNonExistent()
        {
            try
            {
                await this.repository.Delete(new TestDocument
                {
                    Seed = TestDocumentCount + 1
                });
            }
            catch (InvalidOperationException)
            {
                return;
            }

            Assert.True(false);
        }

        private async Task UpdateNonExistent()
        {
            try
            {
                await this.repository.Update(new TestDocument
                {
                    Seed = TestDocumentCount + 1
                });
            }
            catch (InvalidOperationException)
            {
                return;
            }

            Assert.True(false);
        }

        private async Task GetNonExistent()
        {
            var nonexistent = await this.repository.Get(d => d.Seed == TestDocumentCount + 1);

            Assert.NotNull(nonexistent);
        }

        private async Task Delete()
        {
            var seed = this.GetRandomSeed();
            var randomDocument = await this.repository.Get(d => d.Seed == seed);

            await this.repository.Delete(randomDocument);

            var deletedDocument = await this.repository.Get(d => d.Seed == seed);

            Assert.NotNull(deletedDocument);
        }

        private async Task Update()
        {
            var seed = this.GetRandomSeed();
            var randomDocument = await this.repository.Get(d => d.Seed == seed);

            string updatedName = "Different name";
            randomDocument.Name = updatedName;
            var returnedDocument = await this.repository.Update(randomDocument);

            Assert.Equal(
                updatedName,
                returnedDocument.Name);

            var updatedDocument = await this.repository.Get(d => d.Seed == seed);

            Assert.True(
                updatedDocument.Equals(returnedDocument) &&
                updatedDocument.Equals(randomDocument));

            Assert.False(
                TestDocumentFactory.Create(seed).Equals(updatedDocument));
        }

        private async Task GetId()
        {
            var seed = this.GetRandomSeed();
            var randomDocument = await this.repository.Get(d => d.Seed == seed);

            var randomDocById = await this.repository.Get(randomDocument.Id);

            Assert.True(
                randomDocument.Equals(randomDocById));
        }

        private async Task GetPredicate()
        {
            var seed = this.GetRandomSeed();
            var randomDocument = await this.repository.Get(d => d.Seed == seed);

            Assert.True(
                TestDocumentFactory.Create(seed).Equals(randomDocument));
        }

        private async Task GetWhere()
        {
            var seed1 = this.GetRandomSeed();
            var seed2 = this.GetRandomSeed();

            List<TestDocument> testDocuments = (await this.repository.GetWhere(d => d.Seed == seed1 || d.Seed == seed2))
                .ToList()
                .OrderBy(d => d.Seed)
                .ToList();

            Assert.True(
                TestDocumentFactory.Create(seed1).Equals(testDocuments.Find(d => d.Seed == seed1)));

            Assert.True(
                TestDocumentFactory.Create(seed2).Equals(testDocuments.Find(d => d.Seed == seed2)));
        }

        private async Task GetAllEmpty()
        {
            Assert.Empty(
                (await this.repository.GetAll()).AsEnumerable());
        }

        private async Task GetAll()
        {
            List<TestDocument> testDocuments = (await this.repository.GetAll()).ToList();

            Assert.Equal(
                TestDocumentCount,
                testDocuments.Count);

            int seed = this.GetRandomSeed();
            TestDocument randomDocument = testDocuments.OrderBy(d => d.Seed).ElementAt(seed);

            Assert.True(
                TestDocumentFactory.Create(seed).Equals(randomDocument));
        }

        private async Task Create()
        {
            for (int i = 0; i < TestDocumentCount; i++)
            {
                await this.repository.Create(
                    TestDocumentFactory.Create(i));
            }
        }

        private int GetRandomSeed()
        {
            return this.random.Next(TestDocumentCount);
        }
    }
}
