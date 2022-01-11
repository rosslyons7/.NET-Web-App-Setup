using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Messages;
using Newtonsoft.Json;
using UserService.Services;

namespace UserService.Subscriptions {
    public class UserCreatedConsumer : IConsumer<UserCreated> {
        private readonly IUserService _userService;

        public async Task Consume(ConsumeContext<UserCreated> context) {

            try {
                await _userService.CreateUser(context.Message);
            }catch(Exception e) {
                Console.WriteLine(e.Message);
                throw;
            }
            
        }


        public UserCreatedConsumer(IUserService userService) {

            _userService = userService;
        }
    }
}
