using System;
using Xunit;
using TakoDeploy.Core.Model;

namespace TakoDeploy.Core.Tests
{
    public class Notifier_should
    {
        [Fact]
        public void CallOnPropertyChanged()
        {
            var n = new NotifierTest();
            var propChanged = false; 
            n.PropertyChanged += (sender, args) => 
            {
                propChanged = true;
            };
            n.TestProp = 99;
            Assert.True(propChanged);
        }

        public class NotifierTest : Notifier
        {
            private int _testProp;

            public int TestProp
            {
                get { return _testProp; }
                set { SetField(ref _testProp, value); }
            }          

        }
    }
}
