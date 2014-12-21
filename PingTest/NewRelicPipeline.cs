using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Bootstrapper;
using Nancy.Routing;

namespace PingTest
{
    using NewRelicAgent = NewRelic.Api.Agent.NewRelic; // protip: don't give class and namespace the same name. it's awkward.
    public class NewRelicPipeline : IApplicationStartup
    {
        private readonly IRouteResolver routeResolver;

        public NewRelicPipeline(IRouteResolver routeResolver)
        {
            this.routeResolver = routeResolver;
        }

        public void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(
            context =>
            {
                var route = routeResolver.Resolve(context);

                if (route == null || route.Route == null || route.Route.Description == null) // probably not necessary but don't want the chance of losing visibility on anything
                {
                    NewRelicAgent.SetTransactionName(
                        context.Request.Method,
                        context.Request.Url.ToString());
                }
                else
                {
                    NewRelicAgent.SetTransactionName(
                        route.Route.Description.Method,
                        route.Route.Description.Path);
                }
                return null;
            });
            pipelines.OnError.AddItemToEndOfPipeline(
                (context, ex) =>
                {
                    NewRelicAgent.NoticeError(
                        ex);
                    return null;
                });
        }
    }
}