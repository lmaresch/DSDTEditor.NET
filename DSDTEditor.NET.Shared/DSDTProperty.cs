//---------------------------------------------------------------------------------
// DSDTProperty.cs
// 
//---------------------------------------------------------------------------------

namespace DSDTEditor.NET.Shared
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines the <see cref="DSDTProperty" />
    /// </summary>
    public class DSDTProperty : INotifyPropertyChanged
    {
        /// <summary>
        /// Defines the PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The OnPropertyChanged
        /// </summary>
        /// <param name="propertyName">The propertyName<see cref="string"/></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The SetField
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field<see cref="T"/></param>
        /// <param name="value">The value<see cref="T"/></param>
        /// <param name="propertyName">The propertyName<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
