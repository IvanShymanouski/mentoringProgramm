using System.Data.Entity.Infrastructure;

namespace CustomSerialization.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Serializable]
    public partial class Product : ISerializable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        public Product(SerializationInfo information, StreamingContext context)
        {
            ProductID = information.GetInt32("ProductID");
            ProductName = information.GetString("ProductName");
            SupplierID = (int?)information.GetValue("SupplierID", typeof(int?));
            CategoryID = (int?)information.GetValue("CategoryID", typeof(int?));
            QuantityPerUnit = information.GetString("QuantityPerUnit");
            UnitPrice = (decimal?)information.GetValue("UnitPrice", typeof(decimal?));
            UnitsInStock = (short?)information.GetValue("UnitsInStock", typeof(short?));
            UnitsOnOrder = (short?)information.GetValue("UnitsOnOrder", typeof(short?));
            ReorderLevel = (short?)information.GetValue("ReorderLevel", typeof(short?));
            Discontinued = information.GetBoolean("Discontinued");
            Category = (Category)information.GetValue("Category", typeof(Category));
            Order_Details = (ICollection<Order_Detail>)information.GetValue("Order_Details", typeof(ICollection<Order_Detail>));
            Supplier = (Supplier)information.GetValue("Supplier", typeof(Supplier));
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual Supplier Supplier { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var dbContext = (context.Context as IObjectContextAdapter).ObjectContext;

            dbContext.LoadProperty(this, x => x.Category);
            dbContext.LoadProperty(this, x => x.Order_Details);
            dbContext.LoadProperty(this, x => x.Supplier);
            info.AddValue("ProductID", ProductID);
            info.AddValue("ProductName", ProductName);
            info.AddValue("SupplierID", SupplierID);
            info.AddValue("CategoryID", CategoryID);
            info.AddValue("QuantityPerUnit", QuantityPerUnit);
            info.AddValue("UnitPrice", UnitPrice);
            info.AddValue("UnitsInStock", UnitsInStock);
            info.AddValue("UnitsOnOrder", UnitsOnOrder);
            info.AddValue("ReorderLevel", ReorderLevel);
            info.AddValue("Discontinued", Discontinued);
            info.AddValue("Category", Category);
            info.AddValue("Order_Details", Order_Details);
            info.AddValue("Supplier", Supplier);
        }
    }
}
