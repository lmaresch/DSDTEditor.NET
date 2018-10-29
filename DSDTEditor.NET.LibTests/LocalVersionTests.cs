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
    public class LocalVersionTests
    {
        private string workingFolder;
        private string notExistingFolder;

        [TestInitialize]
        public void Initialize()
        {
            this.workingFolder = AppDomain.CurrentDomain.BaseDirectory;
            this.notExistingFolder = Path.Combine(this.workingFolder, "notExistingFolder");
            this.Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(this.notExistingFolder))
                Directory.Delete(this.notExistingFolder, true);
        }

        [TestMethod()]
        public void CheckAndCreateNeededDirectoriesTestDirsExisting()
        {
            LocalVersion lv = new LocalVersion(this.workingFolder);
            lv.CheckAndCreateNeededDirectories();
            Assert.IsTrue(Directory.Exists(Path.Combine(this.workingFolder, ".DSDTEditor.NET")));
            Assert.IsTrue(Directory.Exists(Path.Combine(Path.Combine(this.workingFolder, ".DSDTEditor.NET"), "acpica")));
        }

        [TestMethod()]
        public void CheckAndCreateNeededDirectoriesTestDirsNotExisting()
        {
            LocalVersion lv = new LocalVersion(this.notExistingFolder);
            lv.CheckAndCreateNeededDirectories();
            Assert.IsTrue(Directory.Exists(Path.Combine(this.workingFolder, ".DSDTEditor.NET")));
            Assert.IsTrue(Directory.Exists(Path.Combine(Path.Combine(this.workingFolder, ".DSDTEditor.NET"), "acpica")));
        }

        [TestMethod()]
        public void RetrieveCurrentVersionTest()
        {
            long expectedVersion = 20181003;
            LocalVersion lv = new LocalVersion(this.workingFolder);
            lv.CheckAndCreateNeededDirectories();
            lv.RetrieveCurrentVersion();
            Assert.AreEqual(expectedVersion, lv.AcpicaVersion);
            Assert.AreEqual(Path.Combine(new string[] { this.workingFolder, ".DSDTEditor.NET", "acpica", "iasl-win-" + expectedVersion.ToString() + ".zip" }), lv.AcpicaArchivePath);
        }

        [TestMethod()]
        public void RetrieveCurrentVersionNoArchivePresentTest()
        {
            long expectedVersion = 0;
            LocalVersion lv = new LocalVersion(this.notExistingFolder);
            lv.CheckAndCreateNeededDirectories();
            lv.RetrieveCurrentVersion();
            Assert.AreEqual(expectedVersion, lv.AcpicaVersion);
            Assert.AreEqual(string.Empty, lv.AcpicaArchivePath);
        }
    }
}