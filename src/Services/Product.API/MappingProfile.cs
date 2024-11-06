﻿using AutoMapper;
using Infrastructure.Mappings;
using Product.API.Entities;
using Shared.Dtos.Product;

namespace Product.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogProduct, ProductDto>();
            CreateMap<CreateProductDto, CatalogProduct>();
            CreateMap<UpdateProductDto, CatalogProduct>().IgnoreAllNonExisting();
        }
    }
}
