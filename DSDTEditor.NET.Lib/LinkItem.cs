//---------------------------------------------------------------------------------
// CheckVersion.cs
// 
//---------------------------------------------------------------------------------

namespace DSDTEditor.NET.Lib
{
    /// <summary>
    /// Defines the <see cref="LinkItem" />
    /// </summary>
    public struct LinkItem
    {
        /// <summary>
        /// Defines the Href
        /// </summary>
        public string Href;

        /// <summary>
        /// Defines the Text
        /// </summary>
        public string Text;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return Href + "\n\t" + Text;
        }
    }
}
