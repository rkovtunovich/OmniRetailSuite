﻿namespace RetailAssistant.Application.Mapping.Retail;

public class ReceiptProfile : Profile
{
    public ReceiptProfile()
    {
        CreateMap<Receipt, ReceiptDto>();
        CreateMap<ReceiptDto, Receipt>();
    }
}
