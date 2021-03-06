﻿using System;
using ZKWeb.Logging;
using ZKWeb.Plugins.Common.Base.src.Components.ScheduledTasks.Interfaces;
using ZKWeb.Plugins.Shopping.Order.src.Domain.Services;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.Components.ScheduledTasks {
	/// <summary>
	/// 清理过期的购物车商品
	/// </summary>
	[ExportMany]
	public class CartProductCleaner : IScheduledTaskExecutor {
		/// <summary>
		/// 任务键名
		/// </summary>
		public string Key { get { return "Shopping.Order.CartProductCleaner"; } }

		/// <summary>
		/// 每小时执行一次
		/// </summary>
		public bool ShouldExecuteNow(DateTime lastExecuted) {
			return ((DateTime.UtcNow - lastExecuted).TotalHours > 1.0);
		}

		/// <summary>
		/// 清理过期的购物车商品
		/// </summary>
		public void Execute() {
			var cartProductManager = Application.Ioc.Resolve<CartProductManager>();
			var count = cartProductManager.ClearExpiredCartProducts();
			var logManager = Application.Ioc.Resolve<LogManager>();
			logManager.LogInfo(string.Format(
				"CartProductCleaner executed, {0} cart products removed", count));
		}
	}
}
