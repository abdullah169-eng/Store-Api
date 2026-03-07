using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.Data.Configurations
{
    public class OrderItemConfigs : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(O=> O.Product, P =>P.WithOwner());
            builder.Property(O => O.Price).HasColumnType("decimal(18,2)");
        }
    }
}
