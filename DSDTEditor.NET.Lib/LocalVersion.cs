//-----------------------------------------------------------------------
// <copyright file="LocalVersion.cs" company="SAP SE">
//     Copyright 2018 (c) SAP SE. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DSDTEditor.NET.Lib
{
    using DSDTEditor.NET.Shared;
    using System;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="LocalVersion" />
    /// </summary>
    public class LocalVersion : DSDTProperty
    {
        #region Fields

        /// <summary>
        /// Defines the archivePartName
        /// </summary>
        private static string archivePartName = "iasl-win-";

        /// <summary>
        /// Defines the acpicaArchivePath
        /// </summary>
        private string acpicaArchivePath;

        /// <summary>
        /// Defines the acpicaFolder
        /// </summary>
        private string acpicaFolder;

        /// <summary>
        /// Defines the acpicaVersion
        /// </summary>
        private long acpicaVersion;

        /// <summary>
        /// Defines the appFolder
        /// </summary>
        private string appFolder;

        /// <summary>
        /// Defines the userFolder
        /// </summary>
        private string userFolder;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalVersion"/> class.
        /// </summary>
        /// <param name="alternateUserFolder">The alternateUserFolder<see cref="string"/></param>
        public LocalVersion(string alternateUserFolder = "")
        {
            if (string.IsNullOrEmpty(alternateUserFolder))
                this.userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            else
                this.userFolder = alternateUserFolder;
            this.appFolder = Path.Combine(this.userFolder, ".DSDTEditor.NET");
            this.acpicaFolder = Path.Combine(this.appFolder, "acpica");
            this.acpicaVersion = 0;
            this.acpicaArchivePath = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the AcpicaArchivePath
        /// </summary>
        public string AcpicaArchivePath { get => this.acpicaArchivePath; set => this.SetField(ref this.acpicaArchivePath, value); }

        /// <summary>
        /// Gets or sets the AcpicaFolder
        /// </summary>
        public string AcpicaFolder { get => this.acpicaFolder; set => this.SetField(ref this.acpicaFolder, value); }

        /// <summary>
        /// Gets or sets the AcpicaVersion
        /// </summary>
        public long AcpicaVersion { get => this.acpicaVersion; set => this.SetField(ref this.acpicaVersion, value); }

        /// <summary>
        /// Gets or sets the AppFolder
        /// </summary>
        public string AppFolder { get => this.appFolder; set => this.SetField(ref this.appFolder, value); }

        #endregion

        #region Methods

        /// <summary>
        /// The CheckAndCreateNeededDirectories
        /// </summary>
        public void CheckAndCreateNeededDirectories()
        {
            if (!Directory.Exists(this.appFolder))
                Directory.CreateDirectory(this.appFolder);
            if (!Directory.Exists(this.acpicaFolder))
                Directory.CreateDirectory(this.acpicaFolder);
        }

        /// <summary>
        /// The RetrieveCurrentVersion
        /// </summary>
        public void RetrieveCurrentVersion()
        {
            long higestVersion = 0;
            foreach (string archiveName in Directory.EnumerateFiles(this.AcpicaFolder, archivePartName + "*.zip", SearchOption.TopDirectoryOnly))
            {
                string currentVersionString = Path.GetFileName(archiveName).Replace(archivePartName, "").Replace(".zip", "");
                long currentVersion = 0;
                if (long.TryParse(currentVersionString, out currentVersion))
                    if (currentVersion > higestVersion)
                    {
                        higestVersion = currentVersion;
                        this.AcpicaArchivePath = archiveName;
                    }
            }

            this.AcpicaVersion = higestVersion;
        }

        #endregion
    }
}
