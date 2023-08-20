﻿using AutoMapper;
using PantryOrganizer.Application.Dtos;
using PantryOrganizer.Data.Models;

namespace PantryOrganizer.Application.Mappers;

public class StorageItemMapper : Profile
{
    public StorageItemMapper()
    {
        CreateMap<StorageItem, StorageItemDto>();
        CreateMap<StorageItemDto, StorageItem>()
            .ForMember(storageItem => storageItem.Unit, options => options.Ignore());
    }
}
