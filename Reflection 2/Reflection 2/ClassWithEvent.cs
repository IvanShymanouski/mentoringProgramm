using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflection_2
{
    public class ClassWithInheritedEvent : ClassWithEventAndTrigger
    {
        public void amazingTrigger()
        {
            parentTrigger();
        }
    }

    public class ClassWithEventAndTrigger
    {
        public delegate void EventMethodTemplate();

        public event EventMethodTemplate onSomeEvent;

        public void parentTrigger()
        {
            if (onSomeEvent != null)
            {
                onSomeEvent();
            }
        }
    }
}
