using AutoMapper;
using BackOffice.Client.Mapping;

namespace WebApps.Backoffice.UnitTests.Client.Mapping;

public class HierarchySelectModelProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<HierarchySelectModelProfile>());

        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
