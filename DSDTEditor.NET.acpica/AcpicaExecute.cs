using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSDTEditor.NET.Lib;
using DSDTEditor.NET.Shared;

namespace DSDTEditor.NET.acpica
{
    public class AcpicaExecute : DSDTProperty
    {
        private string acpicaFolder;
        public AcpicaExecute()
        {
            this.acpicaFolder = string.Empty;
        }

        public void CompareLocalAndRemoteVersion()
        {
            CheckVersion cv = new CheckVersion();
            LocalVersion lv = new LocalVersion();
            bool newerVersionAvailable = cv.RemoteVersion > lv.AcpicaVersion;
            if (newerVersionAvailable)
                cv.DownloadArchive(lv);
            this.acpicaFolder = lv.AcpicaFolder;
            this.CheckBinariesAndExtractArchiveIfNeeded(lv.AcpicaArchivePath, newerVersionAvailable);
        }

        public void CheckBinariesAndExtractArchiveIfNeeded(string archivePath, bool newerVersionAvailable)
        {
            string destination = Path.GetDirectoryName(archivePath);
            ZipArchive zipArchive = ZipFile.OpenRead(archivePath);
            foreach(ZipArchiveEntry entry in zipArchive.Entries)
            {
                string destinationFile = Path.Combine(destination, entry.FullName);
                bool extract = false;
                if (File.Exists(destinationFile))
                {
                    if (newerVersionAvailable)
                    {
                        File.Delete(destinationFile);
                        extract = true;
                    }
                }
                else
                {
                    extract = true;
                }

                if (extract)
                    entry.ExtractToFile(destinationFile);
            }
        }
    }
}
