﻿using AutoMapper;
using BackOffice.Application.Mapping.Retail;

namespace WebApps.Backoffice.UnitTests.Application.Mapping.Retail;

public class ReceiptItemProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ReceiptItemProfile>();
            cfg.AddProfile<RetailProductItemProfile>();
        });

        // Assert & Act
        configuration.AssertConfigurationIsValid();
    }
}
