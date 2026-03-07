using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.Data.Configurations
{
    public class DeliveryMethodsConfigs : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(D=>D.Cost).HasColumnType("decimal(18,2)");
        }
    }
}
