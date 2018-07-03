using System;
using System.Diagnostics;
using JG.TestFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JG.Demo.CoreTests
{
    public class DemoTest : JGTest
    {
        [AssemblyInitialize]
        public static new void AssemblyInit(TestContext context)
        {
            JGTest.AssemblyInit(context);
        }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            Trace.TraceInformation("Start {0}", this.TestContext.TestName);
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            Trace.TraceInformation("End {0}", this.TestContext.TestName);
            base.TestCleanup();
        }

        [AssemblyCleanup]
        public static new void AssemblyCleanup()
        {
            JGTest.AssemblyCleanup();
        }
    }
}
