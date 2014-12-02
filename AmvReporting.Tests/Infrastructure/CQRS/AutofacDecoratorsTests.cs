using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AmvReporting.Infrastructure.Autofac;
using AmvReporting.Infrastructure.CQRS;
using Autofac;
using FluentAssertions;
using Xunit.Extensions;


namespace AmvReporting.Tests.Infrastructure.CQRS
{
    public class AutofacDecoratorsTests
    {
        [Theory]
        [InlineData(typeof(IQuery<>), typeof(IQueryHandler<,>), new[] { typeof(CachedQueryHandlerDecorator<,>) })]
        public void Autoafac_AllHandlersForWeb_AreDecorated(Type handleDefinition, Type handlerTypeDefinition, Type[] decoratorOrdering)
        {
            //Arrange
            var allHandleTypes = Assembly.GetAssembly(handleDefinition).GetTypes()
                .Where(t => !t.IsAbstract)
                    .Where(t => t.IsClass && handleDefinition.IsAssignableFrom(t))
                    .ToList();

            var container = AutofacConfig.Configure();

            var errors = new List<String>();

            // Act
            foreach (var handleType in allHandleTypes)
            {
                var handlerType = handlerTypeDefinition.MakeGenericType(handleType);

                var currentHandler = container.Resolve(handlerType);

                var currentLevel = currentHandler.GetType();
                var depth = 0;
                //Check all the decorators we expect and make sure we are stopping at the non decorated version (probably a better way of checking)
                while (depth < decoratorOrdering.Count())
                {
                    var currentDecoratorToConfirm = decoratorOrdering[depth];
                    var assignableFrom = currentDecoratorToConfirm.MakeGenericType(handleType).IsAssignableFrom(currentLevel);
                    if (!assignableFrom)
                    {
                        errors.Add(handlerType.Name + "is not assignable to " + currentDecoratorToConfirm.Name);
                    }

                    var innerHandler = currentLevel.GetProperty("Decorated");
                    currentHandler = innerHandler.GetValue(currentHandler, null);
                    currentLevel = currentHandler.GetType();
                    depth = depth + 1;
                }
            }
            // Assert
            var separator = String.Format("{0}{0}------------------------------------{0}", Environment.NewLine);
            var finalMessage = separator + String.Join(separator, errors);
            errors.Should().BeEmpty(finalMessage);
        }
    }
}
