namespace Cuisine.Application.DTOs.Mappers;
using Cuisine.Domain.Models;

public static class TokenMapper
    {
        public static TokenDTO ToDTO(this Token token)
        {
            return new TokenDTO
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                UserId = token.UserId,
            };
        }

        public static Token ToEntity(this TokenDTO tokenDTO)
        {
            return new Token
            {
                AccessToken = tokenDTO.AccessToken,
                RefreshToken = tokenDTO.RefreshToken,
                UserId = tokenDTO.UserId,
            };
        }
    }
