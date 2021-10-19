using Microsoft.VisualStudio.TestTools.UnitTesting;
using xSpoFunctions.Triggers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xSpoFunctions.Services;

namespace xSpoFunctions.Triggers.Tests
{
    [TestClass()]
    public class GetxSpoDetailsTests
    {
        [TestMethod()]
        public async Task GetxSpoExtendedDetailsTest()
        {
            xSpoService x = new xSpoService();

            var pools = await x.GetxSpoExtendedDetails();

            if(pools == null)
                Assert.Fail();  
        }
    }
}