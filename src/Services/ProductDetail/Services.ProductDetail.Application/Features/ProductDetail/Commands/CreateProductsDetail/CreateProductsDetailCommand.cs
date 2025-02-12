﻿using AutoMapper;
using MediatR;
using Services.ProductDetail.Application.Features.ProductDetail.Dtos;
using Services.ProductDetail.Application.Features.ProductDetail.Rules;
using Services.ProductDetail.Application.Services;
using Services.ProductDetail.Domain.Entities;

namespace Services.ProductDetail.Application.Features.ProductDetail.Commands.CreateProductsDetail
{
    public class CreateProductsDetailCommand : IRequest<CreatedProductsDetailDto>
    {
        public WriteProductsDetailDto WriteProductsDetailDto { get; set; }

        public class CreateProductsDetailCommandHandler : IRequestHandler<CreateProductsDetailCommand, CreatedProductsDetailDto>
        {
            private readonly IProductDetailService _productDetailService;
            private readonly IMapper _mapper;
            private readonly ProductDetailBusinessRules _productDetailBusinessRules;

            public CreateProductsDetailCommandHandler(IProductDetailService productDetailService, 
                                                      IMapper mapper, 
                                                      ProductDetailBusinessRules productDetailBusinessRules)
            {
                _productDetailService = productDetailService;
                _mapper = mapper;
                _productDetailBusinessRules = productDetailBusinessRules;
            }

            public async Task<CreatedProductsDetailDto> Handle(CreateProductsDetailCommand request, CancellationToken cancellationToken)
            {
                //rules needs to be fill
                await _productDetailBusinessRules.ProductsDetailNameShouldNotBeAlreadyExistWhenProductsDetailCreated(request.WriteProductsDetailDto.Name);
                await _productDetailBusinessRules.AllProductExtrasShouldBeAlreadyExistWhenProductsDetailCreated(request.WriteProductsDetailDto.ProductExtras);

                ProductsDetail productsDetail = _mapper.Map<ProductsDetail>(request.WriteProductsDetailDto);
                await _productDetailService.AddAsync(productsDetail);

                //productextras should be created with microservice
                //product should be created with microservice
                //image should be created with microservice

                return new();
            }
        }
    }
}
