using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ProductCatalog.Core.Entities.ProductAggregate;
using ProductCatalog.Data;
using ProductCatalog.Data.Config;
using ProductCatalog.Data.Repositories;

namespace ProductCatalog.Tests.Data.Repositories;

public class ProductItemRepositoryShould
{
    private readonly ProductDbContext _context;

    public ProductItemRepositoryShould()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["UseOnlyInMemoryDatabase"] = "true"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddDataLayerDependencies(configuration).ConfigureAwait(false).GetAwaiter().GetResult();

        var serviceProvider = services.BuildServiceProvider();
        _context = serviceProvider.GetRequiredService<ProductDbContext>();
    }

    [Fact]
    public async Task CreateItemAsync()
    {
        // Arrange
        var item = new ProductItem
        {
            Id = Guid.NewGuid(),
            Name = "Test Item",
            Description = "Test Description",
            Price = 100,
            ProductBrand = new ProductBrand
            {
                Id = Guid.NewGuid(),
                Name = "Test Brand"
            },
            ProductType = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = "Test Type"
            }
        };

        var repository = new ProductItemRepository(_context, NullLogger<ProductItemRepository>.Instance);

        // Act
        var result = await repository.CreateItemAsync(item);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetItemByIdAsync()
    {
        // Arrange
        var item = new ProductItem
        {
            Id = Guid.NewGuid(),
            Name = "Test Item",
            Description = "Test Description",
            Price = 100,
            ProductBrand = new ProductBrand
            {
                Id = Guid.NewGuid(),
                Name = "Test Brand"
            },
            ProductType = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = "Test Type"
            }
        };

        var repository = new ProductItemRepository(_context, NullLogger<ProductItemRepository>.Instance);
        await repository.CreateItemAsync(item);

        // Act
        var result = await repository.GetItemByIdAsync(item.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
        result.Description.Should().Be(item.Description);
        result.Price.Should().Be(item.Price);
        result.ProductBrand.Should().NotBeNull();
        result.ProductBrand!.Id.Should().Be(item.ProductBrand.Id);
        result.ProductBrand.Name.Should().Be(item.ProductBrand.Name);
        result.ProductType.Should().NotBeNull();
        result.ProductType!.Id.Should().Be(item.ProductType.Id);
        result.ProductType.Name.Should().Be(item.ProductType.Name);
    }

    [Fact]
    public async Task GetItemsAsync()
    {
        // Arrange
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();

        var item1 = new ProductItem
        {
            Id = Guid.NewGuid(),
            Name = "Test Item 1",
            Description = "Test Description 1",
            Price = 100,
            ProductBrand = new ProductBrand
            {
                Id = Guid.NewGuid(),
                Name = "Test Brand 1"
            },
            ProductType = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = "Test Type 1"
            }
        };
        var item2 = new ProductItem
        {
            Id = Guid.NewGuid(),
            Name = "Test Item 2",
            Description = "Test Description 2",
            Price = 200,
            ProductBrand = new ProductBrand
            {
                Id = Guid.NewGuid(),
                Name = "Test Brand 2"
            },
            ProductType = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = "Test Type 2"
            }
        };

        var repository = new ProductItemRepository(_context, NullLogger<ProductItemRepository>.Instance);
        await repository.CreateItemAsync(item1);
        await repository.CreateItemAsync(item2);

        // Act
        var result = await repository.GetItemsAsync(1, 0);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(item1.Id);
        result[0].Name.Should().Be(item1.Name);
        result[0].Description.Should().Be(item1.Description);
        result[0].Price.Should().Be(item1.Price);
        result[0].ProductBrand.Should().NotBeNull();
        result[0].ProductBrand!.Id.Should().Be(item1.ProductBrand.Id);
        result[0].ProductBrand!.Name.Should().Be(item1.ProductBrand.Name);
        result[0].ProductType.Should().NotBeNull();
        result[0].ProductType!.Id.Should().Be(item1.ProductType.Id);
        result[0].ProductType!.Name.Should().Be(item1.ProductType.Name);
    }

    [Fact]
    public async Task GetItemsAsync_WhenNoItems()
    {
        // Arrange
        var repository = new ProductItemRepository(_context, NullLogger<ProductItemRepository>.Instance);
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();

        // Act
        var result = await repository.GetItemsAsync(1, 0);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetItemsByCategoryAsync()
    {
        // Arrange
        var brand = new ProductBrand
        {
            Id = Guid.NewGuid(),
            Name = "Test Brand"
        };

        var type = new ProductType
        {
            Id = Guid.NewGuid(),
            Name = "Test Type"
        };

        var item1 = new ProductItem
        {
            Id = Guid.NewGuid(),
            Name = "Test Item 1",
            Description = "Test Description 1",
            Price = 100,
            ProductBrand = brand,
            ProductType = type
        };

        var item2 = new ProductItem
        {
            Id = Guid.NewGuid(),
            Name = "Test Item 2",
            Description = "Test Description 2",
            Price = 200,
            ProductBrand = brand,
            ProductType = type
        };

        var repository = new ProductItemRepository(_context, NullLogger<ProductItemRepository>.Instance);
        await repository.CreateItemAsync(item1);
        await repository.CreateItemAsync(item2);

        // Act
        var result = await repository.GetItemsByCategoryAsync(brand.Id, type.Id);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(2);
        result[0].Id.Should().Be(item1.Id);
        result[0].Name.Should().Be(item1.Name);
        result[0].Description.Should().Be(item1.Description);
        result[0].Price.Should().Be(item1.Price);
        result[0].ProductBrand.Should().NotBeNull();
        result[0].ProductBrand!.Id.Should().Be(item1.ProductBrand.Id);
        result[0].ProductBrand!.Name.Should().Be(item1.ProductBrand.Name);
        result[0].ProductType.Should().NotBeNull();
        result[0].ProductType!.Id.Should().Be(item1.ProductType.Id);
        result[0].ProductType!.Name.Should().Be(item1.ProductType.Name);
        result[1].Id.Should().Be(item2.Id);
        result[1].Name.Should().Be(item2.Name);
        result[1].Description.Should().Be(item2.Description);
        result[1].Price.Should().Be(item2.Price);
        result[1].ProductBrand.Should().NotBeNull();
        result[1].ProductBrand!.Id.Should().Be(item2.ProductBrand.Id);
        result[1].ProductBrand!.Name.Should().Be(item2.ProductBrand.Name);
    }

    [Fact]
    public async Task GetItemsByCategoryAsync_WhenNoItems()
    {
        // Arrange
        var repository = new ProductItemRepository(_context, NullLogger<ProductItemRepository>.Instance);

        // Act
        var result = await repository.GetItemsByCategoryAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateItemAsync()
    {
        // Arrange
        var item = new ProductItem
        {
            Id = Guid.NewGuid(),
            Name = "Test Item",
            Description = "Test Description",
            Price = 100,
            ProductBrand = new ProductBrand
            {
                Id = Guid.NewGuid(),
                Name = "Test Brand"
            },
            ProductType = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = "Test Type"
            }
        };

        var repository = new ProductItemRepository(_context, NullLogger<ProductItemRepository>.Instance);
        await repository.CreateItemAsync(item);

        item.Name = "Updated Item";
        item.Description = "Updated Description";
        item.Price = 200;

        // Act
        var result = await repository.UpdateItemAsync(item);

        // Assert
        result.Should().BeTrue();

        var updatedItem = await repository.GetItemByIdAsync(item.Id);
        updatedItem.Should().NotBeNull();
        updatedItem!.Id.Should().Be(item.Id);
        updatedItem.Name.Should().Be(item.Name);
        updatedItem.Description.Should().Be(item.Description);
        updatedItem.Price.Should().Be(item.Price);
        updatedItem.ProductBrand.Should().NotBeNull();
        updatedItem.ProductBrand!.Id.Should().Be(item.ProductBrand.Id);
        updatedItem.ProductBrand.Name.Should().Be(item.ProductBrand.Name);
        updatedItem.ProductType.Should().NotBeNull();
        updatedItem.ProductType!.Id.Should().Be(item.ProductType.Id);
        updatedItem.ProductType.Name.Should().Be(item.ProductType.Name);
    }

    [Fact]
    public async Task UpdateItemAsync_WhenItemNotFound()
    {
        // Arrange
        var item = new ProductItem
        {
            Id = Guid.NewGuid(),
            Name = "Test Item",
            Description = "Test Description",
            Price = 100,
            ProductBrand = new ProductBrand
            {
                Id = Guid.NewGuid(),
                Name = "Test Brand"
            },
            ProductType = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = "Test Type"
            }
        };

        var repository = new ProductItemRepository(_context, NullLogger<ProductItemRepository>.Instance);

        // Act
        Func<Task> act = async () => await repository.UpdateItemAsync(item);

        // Assert
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }

    [Fact]
    public async Task DeleteItemAsync()
    {
        // Arrange
        var item = new ProductItem
        {
            Id = Guid.NewGuid(),
            Name = "Test Item",
            Description = "Test Description",
            Price = 100,
            ProductBrand = new ProductBrand
            {
                Id = Guid.NewGuid(),
                Name = "Test Brand"
            },
            ProductType = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = "Test Type"
            }
        };

        var repository = new ProductItemRepository(_context, NullLogger<ProductItemRepository>.Instance);
        await repository.CreateItemAsync(item);

        // Act
        var result = await repository.DeleteItemAsync(item.Id, false);

        // Assert
        result.Should().BeTrue();

        var deletedItem = await repository.GetItemByIdAsync(item.Id);
        deletedItem.Should().BeNull();
    }

    [Fact]
    public async Task DeleteItemAsync_WhenItemNotFound()
    {
        // Arrange
        var repository = new ProductItemRepository(_context, NullLogger<ProductItemRepository>.Instance);

        // Act
        var result = await repository.DeleteItemAsync(Guid.NewGuid(), false);

        // Assert
        result.Should().BeFalse();
    }
}
