using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Subscriptions;
using GreenPipes;
namespace UserService.SubscriptionDefinitions {
    public class UserCreatedConsumerDefinition : ConsumerDefinition<UserCreatedConsumer>{


        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UserCreatedConsumer> consumerConfigurator) {

            endpointConfigurator.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(3)));
        }
    }
}
