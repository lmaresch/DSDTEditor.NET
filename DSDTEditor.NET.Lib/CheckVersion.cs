//---------------------------------------------------------------------------------
// CheckVersion.cs
// 
//---------------------------------------------------------------------------------

namespace DSDTEditor.NET.Lib
{
    using DSDTEditor.NET.Shared;
    using System;
    using System.Net;

    /// <summary>
    /// Defines the <see cref="CheckVersion" />
    /// </summary>
    public class CheckVersion : DSDTProperty
    {
        /// <summary>
        /// Defines the uri
        /// </summary>
        private Uri uri;

        /// <summary>
        /// Defines the webClient
        /// </summary>
        private WebClient webClient;

        /// <summary>
        /// Defines the versionContent
        /// </summary>
        private string versionContent;

        /// <summary>
        /// Defines the versionString
        /// </summary>
        private string versionString;

        /// <summary>
        /// Defines the downloadUri
        /// </summary>
        private Uri downloadUri;

        /// <summary>
        /// Defines the fileName
        /// </summary>
        private string fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckVersion"/> class.
        /// </summary>
        /// <param name="doDownload">The doDownload<see cref="bool"/></param>
        public CheckVersion(bool doDownload = true)
        {
            uri = new Uri("https://acpica.org/downloads/binary-tools");
            webClient = new WebClient
            {
                Proxy = null
            };
            webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
            versionContent = string.Empty;
            if (doDownload)
            {
                webClient.DownloadStringAsync(uri);
            }

            versionString = string.Empty;
            fileName = string.Empty;
        }

        /// <summary>
        /// The WebClient_DownloadStringCompleted
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DownloadStringCompletedEventArgs"/></param>
        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            VersionContent = e.Result;
            EvaluateHTML();
        }

        /// <summary>
        /// Gets or sets the VersionContent
        /// </summary>
        public string VersionContent { get => versionContent; set => SetField(ref versionContent, value); }

        /// <summary>
        /// Gets or sets the DownloadUri
        /// </summary>
        public Uri DownloadUri { get => downloadUri; set => SetField(ref downloadUri, value); }

        /// <summary>
        /// Gets or sets the VersionString
        /// </summary>
        public string VersionString { get => versionString; set => SetField(ref versionString, value); }

        /// <summary>
        /// Gets or sets the FileName
        /// </summary>
        public string FileName { get => fileName; set => SetField(ref fileName, value); }

        /// <summary>
        /// The EvaluateHTML
        /// </summary>
        public void EvaluateHTML()
        {
            string fileName = "iasl-win-";
            foreach (LinkItem linkItem in LinkFinder.Find(VersionContent))
            {
                if (linkItem.Href.Contains(fileName))
                {
                    DownloadUri = new Uri(linkItem.Href);
                    FileName = linkItem.Href.Substring(linkItem.Href.LastIndexOf('/') + 1);
                    VersionString = FileName.Replace(fileName, "").Replace(".zip", "");
                }
            }
        }
    }
}
