using FitLog.Application.UseCases;
using FitLog.Application.UseCases.Auth;
using FitLog.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.UseCaseSubscribers
{
    public class LoginUseCaseSubscriber : IUseCaseSubscriber<LoginUseCase, LoginData, Tokens>
    {
        public int Order => 0;

        public void OnUseCaseExecuted(UseCaseSubscriberData<LoginData, Tokens> data)
        {
            Console.WriteLine("Login use case was executed successfully.");
        }
    }
}
