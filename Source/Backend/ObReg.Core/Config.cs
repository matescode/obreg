using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

using Nini.Config;

namespace ObReg.Core
{
	internal sealed class Config : IConfig
	{
		#region Constants

		/// <summary>
		/// Name of the default section.
		/// </summary>
		private const string DefaultSection = "ObReg";

		private const string ConfigFile = "ObReg.config";

		#endregion

		#region Members

		private IConfigSource _configSource = null;

		/// <summary>
		/// Single instance of Config.
		/// </summary>
		private static Config _instance = null;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the instance of Config.
		/// </summary>
		/// <value>The instance.</value>
		public static Config Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Config();
				}
				return _instance;
			}
		}

		#endregion PROPERTIES

		private Config()
		{
			if (!File.Exists(ConfigFile))
			{
				File.Create(ConfigFile);
			}
			_configSource = new XmlConfigSource(ConfigFile);
		}

		/// <summary>
		/// Saves this instance.
		/// </summary>
		public void Save()
		{
			_configSource.Save();
		}

		/// <summary>
		/// Gets the int for specified key in default section.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Config value or defualt.</returns>
		public int GetInt(string key, int defaultValue)
		{
			return _configSource.Configs[DefaultSection].GetInt(key, defaultValue);
		}

		/// <summary>
		/// Gets the int for specified key in default section.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Config value.</returns>
		public int GetInt(string key)
		{
			return _configSource.Configs[DefaultSection].GetInt(key);
		}

		/// <summary>
		/// Sets the specified key into the default section.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public void Set(string key, object value)
		{
			_configSource.Configs[DefaultSection].Set(key, value);
		}
	}
}