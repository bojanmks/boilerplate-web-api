﻿using WebApi.Common.Enums.Auth;

namespace WebApi.Implementation.ApplicationUsers
{
    public class AnonymousUser : ApplicationUser
    {
        public override int? Id => null;
        public override string Email => null;
        public override UserRole Role => UserRole.Anonymous;
    }
}
