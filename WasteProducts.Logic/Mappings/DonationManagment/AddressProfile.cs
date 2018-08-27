﻿using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.DonationManagment;
using WasteProducts.Logic.Common.Models.DonationManagment;

namespace WasteProducts.Logic.Mappings.DonationManagment
{
    class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDB>()
                .ForMember(m => m.CreatedOn, opt => opt.Ignore())
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Donors, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}