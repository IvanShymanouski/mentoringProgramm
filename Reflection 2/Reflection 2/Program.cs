using System;
using System.Reflection;

namespace Reflection_2
{
    class Program
    {
        public void Message()
        {
            Console.WriteLine("Hi, I'm instanse handler!");
        }

        public static void SecondMessage()
        {
            Console.WriteLine("Hi, I'm static handler!");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("---Befor reflection---");

            ClassWithInheritedEvent InsOfClass = Activator.CreateInstance<ClassWithInheritedEvent>();            
            EventInfo eventInfo = InsOfClass.GetType().GetEvent("onSomeEvent");
            Type handlerType = eventInfo.EventHandlerType;

            //add instanse method
            Program pInst = Activator.CreateInstance<Program>();
            MethodInfo handler = pInst.GetType().GetMethod("Message", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance);
            Delegate handlerDelegate = Delegate.CreateDelegate(handlerType, pInst, handler);
            eventInfo.AddEventHandler(InsOfClass, handlerDelegate);

            //add static method
            handlerDelegate = Delegate.CreateDelegate(handlerType, typeof(Program), "SecondMessage");
            eventInfo.AddEventHandler(InsOfClass, handlerDelegate);


            MethodInfo triggerMethod = InsOfClass.GetType().GetMethod("amazingTrigger", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.DeclaredOnly); //null for parrentTrigger obviously

            triggerMethod.Invoke(InsOfClass, null);

            Console.WriteLine("---After trigger---");

            Console.ReadKey();
        }
    }
}
