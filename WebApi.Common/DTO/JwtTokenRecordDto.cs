﻿using WebApi.Common.DTO.Abstraction;

namespace WebApi.Common.DTO
{
    public class JwtTokenRecordDto : IdentifyableDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiry { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public int UserId { get; set; }
    }
}
