using System;
using System.Web.Mvc;
using System.Web.Routing;

public static class RouteExtensions
{
	///<summary>
	/// Get the name of the route
	///</summary>
	///<param name="route"></param>
	///<returns></returns>
	public static string GetRouteName(this Route route)
	{
		return route == null ? null : route.DataTokens.GetRouteName();
	}

	///<summary>
	/// Get the name of the route
	///</summary>
	///<param name="routeData"></param>
	///<returns></returns>
	public static string GetRouteName(this RouteData routeData)
	{
		return routeData == null ? null : routeData.DataTokens.GetRouteName();
	}

	///<summary>
	/// Get the name of the route
	///</summary>
	///<param name="routeValues"></param>
	///<returns></returns>
	/// <example>
	/// 	<code>
	///			var route = routes.Map("rName", "url");
	///			route.GetRouteName();
	///		</code>
	/// </example>
	public static string GetRouteName(this RouteValueDictionary routeValues)
	{
		if (routeValues == null)
			return null;
		object routeName = null;
		routeValues.TryGetValue("__RouteName", out routeName);
		return routeName as string;
	}

	///<summary>
	/// Set the name of a route
	///</summary>
	///<param name="route">The route</param>
	///<param name="routeName">the route name</param>
	///<returns></returns>
	///<exception cref="ArgumentNullException"></exception>
	/// <example>
	/// 	<code>
	///		routes.MapRoute("rName", "{controller}/{action}").SetRouteName("rName");
	///		</code>
	/// </example>
	public static Route SetRouteName(this Route route, string routeName)
	{
		if (route == null)
			throw new ArgumentNullException("route");

		if (route.DataTokens == null)
			route.DataTokens = new RouteValueDictionary();

		route.DataTokens["__RouteName"] = routeName;
		return route;
	}

	///<summary>
	/// Create routes for which I can retrieve the route name
	///</summary>
	///<param name="routes"></param>
	///<param name="name"></param>
	///<param name="url"></param>
	///<param name="defaults"></param>
	///<param name="constraints"></param>
	///<param name="namespaces"></param>
	///<returns></returns>
	/// <example>
	/// 	<code>
	/// 		var route = routes.Map("rName", "url");
	///			route.GetRouteName();
	///			
	///			//within a controller
	///			string routeName = RouteData.GetRouteName();
	/// 	</code>
	/// </example>
	public static Route Map(this RouteCollection routes, string name,
			string url, object defaults = null, object constraints = null, string[] namespaces = null)
	{
		return routes.MapRoute(name, url, defaults, constraints, namespaces).SetRouteName(name);
	}
}
