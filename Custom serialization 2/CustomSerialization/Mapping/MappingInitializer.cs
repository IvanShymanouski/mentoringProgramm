using System;
using System.Data.Entity.Core.Objects;
using AutoMapper;
using CustomSerialization.DB;

namespace CustomSerialization.Mapping
{
    public static class MappingInitializer
    {
        public static MapperConfiguration GetConfiguration(Type sourceType)
        {
            var destinationType = ObjectContext.GetObjectType(sourceType);

            if (destinationType == typeof(Customer))
            {
                return new MapperConfiguration(conf =>
                {
                    conf.CreateMap(sourceType, destinationType)
                        .ForMember("Orders", opt => opt.Ignore())
                        .ForMember("CustomerDemographics", opt => opt.Ignore());
                });
            }

            if (destinationType == typeof(Employee))
            {
                return new MapperConfiguration(conf =>
                {
                    conf.CreateMap(sourceType, destinationType)
                        .ForMember("Employees1", opt => opt.Ignore())
                        .ForMember("Orders", opt => opt.Ignore())
                        .ForMember("Territories", opt => opt.Ignore())
                        .ForMember("Employee1", opt => opt.Ignore());
                });
            }

            if (destinationType == typeof(Order_Detail))
            {
                return new MapperConfiguration(conf =>
                {
                    conf.CreateMap(sourceType, destinationType)
                        .ForMember("Order", opt => opt.Ignore())
                        .ForMember("Product", opt => opt.Ignore());
                });
            }

            if (destinationType == typeof(Shipper))
            {
                return new MapperConfiguration(conf =>
                {
                    conf.CreateMap(sourceType, destinationType)
                        .ForMember("Orders", opt => opt.Ignore());
                });
            }

            return new MapperConfiguration(conf =>
            {
                conf.CreateMap(sourceType, destinationType);
            });
        }
    }
}