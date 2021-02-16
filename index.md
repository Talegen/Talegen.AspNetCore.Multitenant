# Talegen ASP.net Core Multitenancy Middleware library

## Introduction

The Talegen ASP.net Core Multitenancy Middleware library is an open-source middleware library, maintained by [Talegen](https://talegen.com) in GitHub, providing a quick and easy way to implement several tenant identification strategies seamlessly into an ASP.net Core Web Application.

### Multitenancy Strategies

There are typically three overall multi-tenant strategies when building SaaS applications. 

#### One Instance Per Tenant

The first is to have a single instance of an application for every tenant. This is the simplest strategy in that each tenant has their own instance of the application and resources assigned. 

The cons to using the approach are that costs increase as each tenant uses their own resources and thus cloud compute service costs can grow quickly. This strategy can also add complexity when deploying updates and new releases across all tenants.

#### Multiple Tenants, One Instance

The other two strategies use a single instance of the application require a tenant identifier to determine which tenant the web request is for. One using a single database for all tenants (sharding data), and the other using a single application with a different database for each tenant.

*The most common and suggested strategy is to have a single database per tenant.*

## Determining The Tenant Identifier

One problem typically found when building a multi-tenant application, is how the server knows which tenant is being requested.

This is where this Middleware library is designed to assist the developer by removing the complexities of tenant identification, storage, and retrieval.

There are three typical strategies in determining the tenant identifier and this middleware supports all three.

The first is through the host name. Your single web application can be multi-homed with several unique sub-domains, each unique to the tenant it represents, and that host name can be used as the tenant identifier for tenant information lookups. This strategy is pretty easy to implement, but comes with some cons. The first is the need to create a sub-domain in the DNS for every tenant. If the web application address changes, this can be problematic. When setting up CORS, redirects in IdPs, this becomes a can of worms and doesn't scale well for support.

The second is through a custom HTTP request header. We use `x-tenant` as our custom parameter. This allows the web request to have the tenant identifier specified right in the header of the request. The problem here, is there's no easy way to add this by default on the first request, and must be made through Ajax calls only.

The third approach is through the request route. The first slug (/path/) of the route itself will now contain the tenant identifier. This allows the end user to specify and bookmark the tenant identifier directly in the web address of the application. The con here is this can add complexity to addresses and break bookmarks if the identifier is changed.

Recent best practices suggest a hybrid approach, where the initial request for the application may contain the tenant identifier in the route, and all subsequent Ajax calls will contain the identifier in the header. Our middleware's Route strategy method supports this hybrid approach by first checking for the identifier in the `x-tenant` header and then attempting to extract the identifier from the first part of the route (e.g. `https://myapp.com/Talegen/SomeCall`)

## Middleware Library

Talegen's Multitenant Middleware Library provides several out-of-the-box components to help the developer implement three main components to determining the tenant, storing tenant information, and providing a backing source for tenant information. 

### Tenant Resolution Strategies

Tenant resolution strategies are defined in the `Talegen.AspNetCore.Multitenant.Strategies` namespace. The following strategies are provided for the developer:

| Strategy                       | Class Name                     |
| ------------------------------ | ------------------------------ |
| Derived from Host Name         | `TenantHostResolverStrategy`   |
| Derived from `x-tenant` Header | `TenantHeaderResolverStrategy` |
| Derived from Route             | `TenantRouteResolverStrategy`  |

To implement a strategy, the developer will call the extension method `WithResolutionStrategy<TTenant>` during the `ConfigServices` call in `Startup`.

```c#
// add the multi-tenant capabilities. Using the rout strategy, using a memory store, 
// and an example memory source backer.
services.AddMultiTenancy<SimpleTenant>()
    .WithResolutionStrategy<TenantRouteResolverStrategy>()
    .WithStore<TenantMemoryStore<SimpleTenant>>()
    .WithSource<ExampleMemorySource<SimpleTenant>>();
```

In the example provided, the `AddMultiTenancy<TTenant>` call returns a `TenantBuilder` object, allowing the additional extension methods for defining the strategy, storage, and source middleware.

## More Information

Check out the [Source Documentation Reference](https://talegen.github.io/Talegen.AspNetCore.Multitenant/ref/Talegen.AspNetCore.Multitenant.html)

Check out the [example projects in GitHub](https://github.com/Talegen/Talegen.AspNetCore.Multitenant/tree/main/examples/).

## Available Library Platforms

[.NET – Nuget.org](https://www.nuget.org/packages/Talegen.AspNetCore.Multitenant/)