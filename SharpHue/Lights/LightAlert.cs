using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue
{
    /// <summary>
    /// Specifies a light alert type.
    /// </summary>
    public enum LightAlert
    {
        /// <summary>
        /// Turns off the light alert.
        /// </summary>
        None,

        /// <summary>
        /// Flashes the light once.
        /// </summary>
        Select,

        /// <summary>
        /// Flashes the light repeatedly for 30 seconds, or until <see cref="LightAlert.None" /> is sent.
        /// </summary>
        LSelect
    }
}
