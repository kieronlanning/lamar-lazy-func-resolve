﻿using FluentAssertions;
using Lamar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LamarLazyFunc
{
	[TestClass]
	public class LamarTests
	{
		[TestMethod]
		[DataTestMethod]
		[DataRow(ServiceLifetime.Scoped)]
		[DataRow(ServiceLifetime.Singleton)]
		[DataRow(ServiceLifetime.Transient)]
		public void Resolve_GivenLazyRegisteredService_ResolvesAsExpected(ServiceLifetime lifetime)
		{
			// Arrange
			IServiceCollection serviceCollection = new ServiceRegistry();

			serviceCollection.Add(new ServiceDescriptor(typeof(Service), typeof(Service), lifetime));
			serviceCollection.Add(new ServiceDescriptor(typeof(ServiceWithLazy), typeof(ServiceWithLazy), lifetime));

			IServiceProvider serviceProvider = new Container(serviceCollection);
			using (IServiceScope scope = serviceProvider.CreateScope())
			{
				// Act
				Action action = () => scope.ServiceProvider.GetRequiredService<ServiceWithLazy>();

				// Assert
				action
					.Should()
					.NotThrow();
			}
		}

		[TestMethod]
		[DataTestMethod]
		[DataRow(ServiceLifetime.Scoped)]
		[DataRow(ServiceLifetime.Singleton)]
		[DataRow(ServiceLifetime.Transient)]
		public void Resolve_GivenFuncRegisteredService_ResolvesAsExpected(ServiceLifetime lifetime)
		{
			// Arrange
			IServiceCollection serviceCollection = new ServiceRegistry();

			serviceCollection.Add(new ServiceDescriptor(typeof(Service), typeof(Service), lifetime));
			serviceCollection.Add(new ServiceDescriptor(typeof(ServiceWithFunc), typeof(ServiceWithFunc), lifetime));

			IServiceProvider serviceProvider = new Container(serviceCollection);
			using (IServiceScope scope = serviceProvider.CreateScope())
			{
				// Act
				Action action = () => scope.ServiceProvider.GetRequiredService<ServiceWithFunc>();

				// Assert
				action
					.Should()
					.NotThrow();
			}
		}

		[TestMethod]
		[DataTestMethod]
		[DataRow(ServiceLifetime.Scoped)]
		[DataRow(ServiceLifetime.Singleton)]
		[DataRow(ServiceLifetime.Transient)]
		public void Resolve_GivenRegisteredService_ResolvesAsExpected(ServiceLifetime lifetime)
		{
			// Arrange
			IServiceCollection serviceCollection = new ServiceRegistry();

			serviceCollection.Add(new ServiceDescriptor(typeof(Service), typeof(Service), lifetime));

			IServiceProvider serviceProvider = new Container(serviceCollection);
			using (IServiceScope scope = serviceProvider.CreateScope())
			{
				// Act
				Action action = () => scope.ServiceProvider.GetRequiredService<Service>();

				// Assert
				action
					.Should()
					.NotThrow();
			}
		}

		public class Service
		{
		}

		public class ServiceWithLazy
		{
			public ServiceWithLazy(Lazy<ServiceWithLazy> service)
			{
			}
		}

		public class ServiceWithFunc
		{
			public ServiceWithFunc(Func<ServiceWithLazy> service)
			{
			}
		}
	}
}
