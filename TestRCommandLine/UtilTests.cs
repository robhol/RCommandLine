using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCommandLine;

namespace TestRCommandLine
{
    [TestClass]
    public class UtilTests
    {

        [TestMethod]
        public void BumpyCaseToHyphenateTest()
        {

            //trivial case (hue)
            Assert.AreEqual("value", Util.BumpyCaseToHyphenate("Value"));

            //single separator start/mid/end
            Assert.AreEqual("z-value", Util.BumpyCaseToHyphenate("ZValue"));
            Assert.AreEqual("test-value", Util.BumpyCaseToHyphenate("TestValue"));
            Assert.AreEqual("plan-b", Util.BumpyCaseToHyphenate("PlanB"));

            //initialism start/mid/end
            Assert.AreEqual("rng-mode", Util.BumpyCaseToHyphenate("RNGMode"));
            Assert.AreEqual("is-ssl-active", Util.BumpyCaseToHyphenate("IsSSLActive"));
            Assert.AreEqual("use-http", Util.BumpyCaseToHyphenate("UseHTTP"));

        }

        [TestMethod]
        public void JoinStringsTest()
        {

            Func<string, string> transform = s => string.Join(",", Util.JoinQuotedStringSegments(s.Split(' ')));

            Assert.AreEqual("cake,foo", transform("cake foo"));

            Assert.AreEqual("cake foo", transform("\"cake foo\""));

            Assert.AreEqual("herp,cake foo", transform("\"herp\" \"cake foo\""));

            Assert.AreEqual("herp,cake foo,asdf", transform("\"herp\" \"cake foo\" asdf"));

            Assert.AreEqual("herp,cake foo asdf,derp", transform("\"herp\" \"cake foo asdf\" derp"));

            Assert.AreEqual("\"cake foo", transform("\"cake foo"));

        }

    }
}
