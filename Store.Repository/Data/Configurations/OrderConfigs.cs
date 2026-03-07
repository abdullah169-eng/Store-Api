using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Store.Repository.Data.Configurations
{
    public class OrderConfigs : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O=>O.Subtotal).HasColumnType("decimal(18,2)");
            builder.Property(O=>O.status)
                .HasConversion(OStatus=>OStatus.ToString(), OStatus=> (OrderStatus)Enum.Parse(typeof(OrderStatus),OStatus));
            builder.OwnsOne(O=>O.ShipToAddress, a =>{a.WithOwner();});
            builder.HasOne(O=>O.DeliveryMethod).WithMany().HasForeignKey(O=>O.DeliveryMethodId);
        }
    }
}
