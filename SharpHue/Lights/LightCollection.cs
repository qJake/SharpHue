using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System;

namespace SharpHue
{
    /// <summary>
    /// Represents the entire set of lights in a Hue system.
    /// </summary>
    public class LightCollection : IReadOnlyCollection<Light>
    {
        /// <summary>
        /// Stores the light objects for this collection.
        /// </summary>
        private List<Light> Lights { get; set; }

        /// <summary>
        /// Initializes a new instance of the LightCollection class.
        /// </summary>
        public LightCollection()
        {
            if (Configuration.DeviceIP == null)
            {
                throw new HueConfigurationException("DeviceIP has not been initialized. Try calling Configuration.Initialize().");
            }
            if (Configuration.Username == null)
            {
                throw new HueConfigurationException("Username has not been initialized. Try calling Configuration.Initialize(), or, if you need to register a new user, call Configuration.AddUser() first.");
            }

            Lights = new List<Light>();

            Refresh();
        }

        /// <summary>
        /// Gets a light object by its stored name. Case-sensitive.
        /// </summary>
        /// <param name="Name">The light's name.</param>
        /// <returns>A <see cref="Light" /> object, or <c>null</c> if no light was found with the specified name.</returns>
        public Light this[string Name]
        {
            get
            {
                return Lights.FirstOrDefault(l => l.Name == Name);
            }
        }

        /// <summary>
        /// Gets a light object by its ID.
        /// </summary>
        /// <param name="Name">The light's ID.</param>
        /// <returns>A <see cref="Light" /> object, or <c>null</c> if no light was found with the specified index.</returns>
        public Light this[int index]
        {
            get
            {
                return Lights.FirstOrDefault(l => l.ID == index);
            }
        }

        /// <summary>
        /// Refreshes all light objects and state information.
        /// </summary>
        public void Refresh()
        {
            Lights.Clear();

            JObject lights = JsonClient.RequestSecure(HttpMethod.Get, "/lights") as JObject;

            foreach (KeyValuePair<string, JToken> light in lights)
            {
                Light l = new Light();
                l = light.Value.ToObject<Light>();
                l.ID = Convert.ToInt16(light.Key);
                Lights.Add(l);
                l.RefreshState();
            }
        }

        #region ICollection<T> Implementation

        /// <summary>
        /// Gets a count of all lights in this collection.
        /// </summary>
        public int Count
        {
	        get { return Lights.Count; }
        }

        /// <summary>
        /// Gets the enumerator for this collection.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<Light> GetEnumerator()
        {
 	        return Lights.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for this collection.
        /// </summary>
        /// <returns>The enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
 	        return Lights.GetEnumerator();
        }

        #endregion

    }
}