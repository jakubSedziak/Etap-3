using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt.Model;
using Zycie.Services;

namespace Projekt.Services.Tests
{
    [TestClass()]
    public class SqlSerializationTests
    {
        [TestMethod()]
        public void  SqlSerializationTest()
        {
          SqlSerialization ss = new SqlSerialization();
            AssemblyMetadata am = new AssemblyMetadata(){m_Name = "test1",IsExpanded = false};
             ss.Serialize(am,"nima");

            SQLDeserialization sd = new SQLDeserialization();
            AssemblyMetadata dodane = sd.Deserialize(Convert.ToString(ss.number));

            Assert.IsTrue(dodane.m_Name == "test1");
           ss.Dispose();
        }
    }
}