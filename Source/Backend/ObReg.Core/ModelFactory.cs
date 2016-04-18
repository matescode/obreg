using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObReg.Core
{
	public class ModelFactory
	{
		public static IOrderItemDataAccessLayer CreateOrderItemAccessLayer()
		{
			return OrderItemDataAccessLayer.Instance;
		}

		public static IConfig Configuration
		{
			get
			{
				return Config.Instance;
			}
		}
	}
}
