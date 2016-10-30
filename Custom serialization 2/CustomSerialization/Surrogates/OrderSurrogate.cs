using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CustomSerialization.DB;
using CustomSerialization.Mapping;

namespace CustomSerialization.Surrogates
{
    public class OrderSurrogate : IDataContractSurrogate
    {
        #region Not implemented
        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }
        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }
        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }
        #endregion

        public Type GetDataContractType(Type type)
        {
            if (ObjectContext.GetObjectType(type) == typeof(Order))
                return typeof(Order);
            if (ObjectContext.GetObjectType(type) == typeof(Customer))
                return typeof(Customer);
            if (ObjectContext.GetObjectType(type) == typeof(Shipper))
                return typeof(Shipper);
            if (ObjectContext.GetObjectType(type) == typeof(Order_Detail))
                return typeof(Order_Detail);
            if (ObjectContext.GetObjectType(type) == typeof(Employee))
                return typeof(Employee);
                
            return type;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            if (obj.GetType().Namespace == "System.Data.Entity.DynamicProxies")
            {
                var config = MappingInitializer.GetConfiguration(obj.GetType());
                var mapper = config.CreateMapper();
                var result = Activator.CreateInstance(targetType);
                mapper.Map(obj, result, obj.GetType(), targetType);

                return result;
            }

            return obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj;
        }
    }
}
