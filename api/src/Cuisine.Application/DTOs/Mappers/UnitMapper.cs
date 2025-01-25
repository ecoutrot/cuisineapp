using Cuisine.Domain.Entities;

namespace Cuisine.Application.DTOs.Mappers;

public static class UnitMapper
{
    public static UnitDTO ToUnitDTO(this Unit unit)
    {
        return new UnitDTO
        {
            Id = unit.Id,
            Name = unit.Name
        };
    }

    public static Unit ToEntity(this UnitDTO unitDTO)
    {
        return new Unit
        {
            Id = unitDTO.Id,
            Name = unitDTO.Name,
        };
    }

    public static Unit ToNewEntity(this NewUnitDTO newUnitDTO)
    {
        return new Unit
        {
            Id = Guid.NewGuid(),
            Name = newUnitDTO.Name,
        };
    }
}