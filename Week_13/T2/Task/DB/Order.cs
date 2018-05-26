namespace Task.DB
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Reflection;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        [StringLength(5)]
        public string CustomerID { get; set; }
        [DataMember]
        public int? EmployeeID { get; set; }
        [DataMember]
        public DateTime? OrderDate { get; set; }
        [DataMember]
        public DateTime? RequiredDate { get; set; }
        [DataMember]
        public DateTime? ShippedDate { get; set; }
        [DataMember]
        public int? ShipVia { get; set; }
        [DataMember]
        [Column(TypeName = "money")]
        public decimal? Freight { get; set; }
        [DataMember]
        [StringLength(40)]
        public string ShipName { get; set; }
        [DataMember]
        [StringLength(60)]
        public string ShipAddress { get; set; }
        [DataMember]
        [StringLength(15)]
        public string ShipCity { get; set; }
        [DataMember]
        [StringLength(15)]
        public string ShipRegion { get; set; }
        [DataMember]
        [StringLength(10)]
        public string ShipPostalCode { get; set; }
        [DataMember]
        [StringLength(15)]
        public string ShipCountry { get; set; }
        [DataMember]
        public virtual Customer Customer { get; set; }
        [DataMember]
        public virtual Employee Employee { get; set; }
        [DataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }
        [DataMember]
        public virtual Shipper Shipper { get; set; }
    }

    //class CustomerSurrogate : IDataContractSurrogate
    //{
    //    public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public object GetCustomDataToExport(Type clrType, Type dataContractType)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Type GetDataContractType(Type type)
    //    {
            
    //        var customerType = typeof(Customer);
    //        if (customerType.IsAssignableFrom(type))
    //        {
    //            return typeof(Customer);
    //        }
    //        return type;
    //    }

    //    public object GetDeserializedObject(object obj, Type targetType)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public object GetObjectToSerialize(object obj, Type targetType)
    //    {
    //        if(obj is Customer)
    //        {
    //            Customer customer = (Customer)obj;
    //        }
    //        return obj;
    //    }

    //    public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //[DataContract]
    //class CustomerSurrogated
    //{
    //    [DataMember]
    //    public string CustomerID { get; set; }

    //    [DataMember]
    //    public string CompanyName { get; set; }

    //    [DataMember]
    //    public string ContactName { get; set; }

    //    [DataMember]
    //    public string ContactTitle { get; set; }

    //    [DataMember]
    //    public string Address { get; set; }

    //    [DataMember]
    //    public string City { get; set; }

    //    [DataMember]
    //    public string Region { get; set; }

    //    [DataMember]
    //    public string PostalCode { get; set; }

    //    [DataMember]
    //    public string Country { get; set; }

    //    [DataMember]
    //    public string Phone { get; set; }

    //    [DataMember]
    //    public string Fax { get; set; }

    //    [DataMember]
    //    public virtual ICollection<Order> Orders { get; set; }

    //    [DataMember]
    //    public virtual ICollection<CustomerDemographic> CustomerDemographics { get; set; }
    //}

}
