﻿using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Core;

namespace ZKWeb.Plugins.Common.Base.src.Repositories {
	/// <summary>
	/// 用于获取数据仓储实例的静态类
	/// </summary>
	public static class RepositoryResolver {
		/// <summary>
		/// 获取指定数据的数据仓储
		/// 如果有在Ioc中注册的继承类，例如
		/// [ExportMany]
		/// class UserRepository : GenericRepository[TData] { }
		/// 则返回这个类的实例，否则使用默认的仓储对象
		/// 如果注册了多个，返回最后一个注册的实例
		/// </summary>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="context">数据库上下文</param>
		/// <returns></returns>
		public static GenericRepository<TData> Resolve<TData>(DatabaseContext context)
			where TData : class {
			var repository = Application.Ioc.ResolveMany<GenericRepository<TData>>().LastOrDefault();
			repository = repository ?? new GenericRepository<TData>();
			repository.Context = context;
			return repository;
		}

		/// <summary>
		/// 获取指定类型的数据仓储
		/// 如果注册了多个，返回最后一个注册的实例
		/// </summary>
		/// <typeparam name="TRepository">数据仓储类型</typeparam>
		/// <typeparam name="TData">数据类型</typeparam>
		/// <param name="context">数据库上下文</param>
		/// <returns></returns>
		public static TRepository Resolve<TRepository, TData>(DatabaseContext context)
			where TRepository : GenericRepository<TData>
			where TData : class {
			var repository = Application.Ioc.ResolveMany<TRepository>().Last();
			repository.Context = context;
			return repository;
		}
	}
}
