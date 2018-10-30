using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSDTEditor.NET.acpica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace DSDTEditor.NET.acpica.Tests
{
    [TestClass()]
    public class AcpicaExecuteTests
    {
        private string acpicaTestFolder;
        private string sourceFolder;
        private string testFolder;
        private string archive;
        private List<string> textFiles;

        [TestInitialize()]
        public void Initialize()
        {
            Logger.LogMessage("============= Initialize Begin =============");
            this.Cleanup();
            this.archive = "newVersion.zip";
            this.textFiles = new List<string>() {"TextFile0.txt",
                "TextFile1.txt",
                "TextFile2.txt",
                "TextFile3.txt",
                "TextFile4.txt",
                "TextFile5.txt",
                "TextFile6.txt",
                "TextFile7.txt",
                "TextFile8.txt"};
            this.acpicaTestFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DSDTEditor.NET.acpicaTests");
            this.sourceFolder = Path.Combine(this.acpicaTestFolder, "TestFiles");
            this.testFolder = Path.Combine(this.acpicaTestFolder, "Temp");
            if (!Directory.Exists(this.testFolder))
                Directory.CreateDirectory(this.testFolder);
            foreach (string fileName in this.textFiles)
                File.Copy(Path.Combine(this.sourceFolder, fileName), Path.Combine(this.testFolder, fileName));
            Logger.LogMessage("============= Initialize End =============");
        }

        [TestCleanup()]
        public void Cleanup()
        {
            Logger.LogMessage("============= Cleanup Begin =============");
            if (Directory.Exists(this.testFolder))
                Directory.Delete(this.testFolder, true);
            Logger.LogMessage("============= Cleanup Begin =============");
        }

        [TestMethod()]
        public void CheckBinariesAndExtractArchiveIfNeededTestEverythingUpToDate()
        {
            AcpicaExecute ae = new AcpicaExecute();
            File.Copy(Path.Combine(this.sourceFolder, this.archive), Path.Combine(this.testFolder, this.archive));
            ae.CheckBinariesAndExtractArchiveIfNeeded(Path.Combine(testFolder, this.archive), false);
            foreach(string fileName in this.textFiles)
            {
                string text = File.ReadAllText(Path.Combine(this.testFolder, fileName));
                Assert.AreEqual("0.9", text);
            }
        }

        [TestMethod()]
        public void CheckBinariesAndExtractArchiveIfNeededTestUpdateAllFiles()
        {
            AcpicaExecute ae = new AcpicaExecute();
            File.Copy(Path.Combine(this.sourceFolder, this.archive), Path.Combine(this.testFolder, this.archive));
            ae.CheckBinariesAndExtractArchiveIfNeeded(Path.Combine(testFolder, this.archive), true);
            foreach (string fileName in this.textFiles)
            {
                string text = File.ReadAllText(Path.Combine(this.testFolder, fileName));
                Assert.AreEqual("1.0", text);
            }
        }
    }
}