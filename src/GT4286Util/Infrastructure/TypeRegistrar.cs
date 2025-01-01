using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace GT4286Util.Infrastructure
{
    public sealed class TypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection _builder;

        public TypeRegistrar(IServiceCollection builder)
        {
            _builder = builder;
        }

        public ITypeResolver Build()
        {
            return new TypeResolver(_builder.BuildServiceProvider());
        }

#pragma warning disable IL2092 // 'DynamicallyAccessedMemberTypes' on the parameter of method don't match overridden parameter of method. All overridden members must have the same 'DynamicallyAccessedMembersAttribute' usage.
        public void Register(Type service, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type implementation)
#pragma warning restore IL2092 // 'DynamicallyAccessedMemberTypes' on the parameter of method don't match overridden parameter of method. All overridden members must have the same 'DynamicallyAccessedMembersAttribute' usage.
        {
            _builder.AddSingleton(service, implementation);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            _builder.AddSingleton(service, implementation);
        }

        public void RegisterLazy(Type service, Func<object> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            _builder.AddSingleton(service, (provider) => func());
        }
    }
}


