using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSDTEditor.NET.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DSDTEditor.NET.Lib.Tests
{
    [TestClass()]
    public class CheckVersionTests
    {
        [TestMethod()]
        public void CheckVersionTest()
        {
            CheckVersion checkVersion = new CheckVersion();
            System.Threading.Thread.Sleep(5000);
            Assert.AreNotEqual(string.Empty, checkVersion.VersionContent);
        }

        [TestMethod()]
        public void EvaluateHTMLTest()
        {
            string content = File.ReadAllText("Result.html");
            CheckVersion checkVersion = new CheckVersion(false);
            checkVersion.VersionContent = content;
            checkVersion.EvaluateHTML();
            Assert.AreEqual(new Uri("https://acpica.org/sites/acpica/files/iasl-win-20181003.zip"), checkVersion.DownloadUri);
            Assert.AreEqual("iasl-win-20181003.zip", checkVersion.FileName);
            Assert.AreEqual("20181003", checkVersion.VersionString);
        }
    }
}