using MassTransit;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Services;

namespace UserService.Subscriptions {
    public class UserDeletedConsumer : IConsumer<UserDeleted>{
        private readonly IUserService _userService;

        public async Task Consume(ConsumeContext<UserDeleted> context) {

            try {
                await _userService.DeleteUser(context.Message);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public UserDeletedConsumer(IUserService userService) {

            _userService = userService;
        }
    }
}
