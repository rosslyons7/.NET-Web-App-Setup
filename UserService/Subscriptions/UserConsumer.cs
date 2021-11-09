using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Requests;
using UserService.Services;

namespace UserService.Subscriptions {
    public class UserConsumer : ConsumerBase {

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        protected async override Task<bool> HandleMessage(string message, string routingKey) {
            using var scope = _serviceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            
            switch(routingKey){
                case "user.create.*":
                    try {
                        return HandleResponse(await userService.CreateUser(JsonConvert.DeserializeObject<CreateUserRequest>(message)));
                    }
                    catch (Exception e) {
                        _logger.LogError($"UserService/UserConsumer/CreateUser -- ${e.Message}");
                        return false;
                    }
                case "user.delete.*":
                    try {
                        return HandleResponse(await userService.DeleteUser(Guid.Parse(message)));
                    }
                    catch (Exception e) {
                        _logger.LogError($"UserService/UserConsumer/DeleteUser -- ${e.Message}");
                        return false;
                    }
            }
            
            return true;
        }

        private bool HandleResponse(int response) =>
            response > 0;


        public UserConsumer(string exchange, string queue, string routingKey, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IConfiguration config)
            : base(exchange, queue, routingKey, loggerFactory, config) {

            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<UserConsumer>();
        }
    }
}
