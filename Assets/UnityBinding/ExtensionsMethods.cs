using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// Extension methods for the <see cref="SimpleContainer"/>.
/// </summary>
public static class ExtensionMethods {
    /// <summary>
    /// Registers a singleton.
    /// </summary>
    /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
    /// <param name="container">The container.</param>
    /// <returns>The container.</returns>
    public static SimpleContainer Singleton<TImplementation>(this SimpleContainer container) {
        container.RegisterSingleton(typeof(TImplementation), null, typeof(TImplementation));
        return container;
    }

    /// <summary>
    /// Registers a singleton.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
    /// <param name="container">The container.</param>
    /// <returns>The container.</returns>
    public static SimpleContainer Singleton<TService, TImplementation>(this SimpleContainer container)
        where TImplementation : TService {
        container.RegisterSingleton(typeof(TService), null, typeof(TImplementation));
        return container;
    }

    /// <summary>
    /// Registers an service to be created on each request.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
    /// <param name="container">The container.</param>
    /// <returns>The container.</returns>
    public static SimpleContainer PerRequest<TService, TImplementation>(this SimpleContainer container)
        where TImplementation : TService {
        container.RegisterPerRequest(typeof(TService), null, typeof(TImplementation));
        return container;
    }

    /// <summary>
    /// Registers an service to be created on each request.
    /// </summary>
    /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
    /// <param name="container">The container.</param>
    /// <returns>The container.</returns>
    public static SimpleContainer PerRequest<TImplementation>(this SimpleContainer container) {
        container.RegisterPerRequest(typeof(TImplementation), null, typeof(TImplementation));
        return container;
    }

    /// <summary>
    /// Registers an instance with the container.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="container">The container.</param>
    /// <param name="instance">The instance.</param>
    /// <returns>The container.</returns>
    public static SimpleContainer Instance<TService>(this SimpleContainer container, TService instance) {
        container.RegisterInstance(typeof(TService), null, instance);
        return container;
    }

    /// <summary>
    /// Registers a custom service handler with the container.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="container">The container.</param>
    /// <param name="handler">The handler.</param>
    /// <returns>The container.</returns>
    public static SimpleContainer Handler<TService>(this SimpleContainer container, Func<SimpleContainer, object> handler) {
        container.RegisterHandler(typeof(TService), null, handler);
        return container;
    }

    /// <summary>
    /// Registers all specified types in an assembly as singletong in the container.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="container">The container.</param>
    /// <param name="assembly">The assembly.</param>
    /// <param name="filter">The type filter.</param>
    /// <returns>The container.</returns>
    public static SimpleContainer AllTypesOf<TService>(this SimpleContainer container, Assembly assembly, Func<Type, bool> filter = null) {
        if(filter == null)
            filter = type => true;

#if WinRT
        var serviceInfo = typeof(TService).GetTypeInfo();
        var types = from type in assembly.DefinedTypes
                    let info = type
                    where serviceInfo.IsAssignableFrom(info)
                            && !info.IsAbstract
                            && !info.IsInterface
                            && filter(type.GetType())
                    select type;
#else
        var serviceType = typeof(TService);
        var types = from type in assembly.GetTypes()
                    where serviceType.IsAssignableFrom(type)
                            && !type.IsAbstract
                            && !type.IsInterface
                            && filter(type)
                    select type;
#endif

        foreach (var type in types) {
#if WinRT
            container.RegisterSingleton(typeof(TService), null, type.GetType());
#else
            container.RegisterSingleton(typeof(TService), null, type);
#endif
        }

        return container;
    }

    /// <summary>
    /// Get's the name of the assembly.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <returns>The assembly's name.</returns>
    public static string GetAssemblyName(this Assembly assembly)
    {
        return assembly.FullName.Remove(assembly.FullName.IndexOf(","));
    }

    /// <summary>
    /// Gets all the attributes of a particular type.
    /// </summary>
    /// <typeparam name="T">The type of attributes to get.</typeparam>
    /// <param name="member">The member to inspect for attributes.</param>
    /// <param name="inherit">Whether or not to search for inherited attributes.</param>
    /// <returns>The list of attributes found.</returns>
    public static IEnumerable<T> GetAttributes<T>(this MemberInfo member, bool inherit)
    {
#if WinRT
            return member.GetCustomAttributes(inherit).OfType<T>();
#else
        return Attribute.GetCustomAttributes(member, inherit).OfType<T>();
#endif
    }

    /// <summary>
    /// Applies the action to each element in the list.
    /// </summary>
    /// <typeparam name="T">The enumerable item's type.</typeparam>
    /// <param name="enumerable">The elements to enumerate.</param>
    /// <param name="action">The action to apply to each item in the list.</param>
    public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
        }
    }

    /// <summary>
    /// Converts an expression into a <see cref="MemberInfo"/>.
    /// </summary>
    /// <param name="expression">The expression to convert.</param>
    /// <returns>The member info.</returns>
    public static MemberInfo GetMemberInfo(this Expression expression)
    {
        var lambda = (LambdaExpression)expression;

        MemberExpression memberExpression;
        if (lambda.Body is UnaryExpression)
        {
            var unaryExpression = (UnaryExpression)lambda.Body;
            memberExpression = (MemberExpression)unaryExpression.Operand;
        }
        else
        {
            memberExpression = (MemberExpression)lambda.Body;
        }

        return memberExpression.Member;
    }

#if WP71
		//Method missing in WP71 Linq

		/// <summary>
		/// Merges two sequences by using the specified predicate function.
		/// </summary>
		/// <typeparam name="TFirst">The type of the elements of the first input sequence.</typeparam>
		/// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
		/// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
		/// <param name="first">The first sequence to merge.</param>
		/// <param name="second">The second sequence to merge.</param>
		/// <param name="resultSelector"> A function that specifies how to merge the elements from the two sequences.</param>
		/// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that contains merged elements of two input sequences.</returns>
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector){
			if (first == null){
				throw new ArgumentNullException("first");
            }

			if (second == null) {
				throw new ArgumentNullException("second");
            }

			if (resultSelector == null) {
				throw new ArgumentNullException("resultSelector");
            }

			var enumFirst = first.GetEnumerator();
			var enumSecond = second.GetEnumerator();

			while (enumFirst.MoveNext() && enumSecond.MoveNext()) {
				yield return resultSelector(enumFirst.Current, enumSecond.Current);
			}
		}
#endif
}
