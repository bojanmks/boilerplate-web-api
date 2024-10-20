using WebApi.Application.UseCases;
using WebApi.Application.UseCases.Auth;
using WebApi.Common.DTO.Auth;

namespace WebApi.Implementation.UseCaseSubscribers
{
    public class LoginUseCaseSubscriber : IUseCaseSubscriber<LoginUseCase, LoginData, Tokens>
    {
        public Task OnUseCaseExecuted(UseCaseSubscriberData<LoginData, Tokens> data)
        {
            Console.WriteLine("Login use case was executed successfully.");
            return Task.CompletedTask;
        }
    }
}
