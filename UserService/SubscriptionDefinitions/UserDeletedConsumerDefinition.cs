using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Subscriptions;

namespace UserService.SubscriptionDefinitions {
    public class UserDeletedConsumerDefinition : ConsumerDefinition<UserDeletedConsumer> {

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UserDeletedConsumer> consumerConfigurator) {

            endpointConfigurator.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(3)));
        }
    }
}
