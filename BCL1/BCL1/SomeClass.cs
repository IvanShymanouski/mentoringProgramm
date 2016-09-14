using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCL1
{
    [CustomAttr("Ivan", "Ivan_Shymanouski@email.by")]
    public class SomeClass: Attribute
    {
       public void print()
       {
           var type = this.GetType();
           if (Attribute.IsDefined(type, typeof(CustomAttr))) // проверка на существование атрибута
           {
               var attributeValue = Attribute.GetCustomAttribute(type, typeof(CustomAttr)) as CustomAttr; // получаем значение атрибута
               Console.WriteLine("Name : {0}, Email : {1}", attributeValue.Name, attributeValue.Email);
           }
       }
    }
}
