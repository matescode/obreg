using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObReg.Core
{
	public interface IConfig
	{
		int GetInt(string key);

		int GetInt(string key, int defaultValue);

		void Set(string key, object value);

		void Save();
	}
}
