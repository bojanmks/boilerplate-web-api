﻿using WebApi.Application.UseCases.Attributes;
using WebApi.Common.DTO;
using WebApi.Common.Enums;

namespace WebApi.Application.UseCases.Auth
{
    [AllowForRoles(UserRole.Anonymous)]
    public class LoginUseCase : UseCase<LoginData, Tokens>
    {
        public LoginUseCase() : base(null) { }

        public LoginUseCase(LoginData data) : base(data)
        {
        }

        public override string Id => "Login";
    }
}
