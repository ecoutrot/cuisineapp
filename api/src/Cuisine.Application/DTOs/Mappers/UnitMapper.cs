using Cuisine.Domain.Entities;

namespace Cuisine.Application.DTOs.Mappers;

public static class UnitMapper
{
    public static UnitDTO ToUnitDTO(this Unit unit)
    {
        return new UnitDTO
        (
            unit.Id,
            unit.Name
        );
    }

    public static Unit ToEntity(this UnitDTO unitDTO)
    {
        return new Unit
        {
            Id = unitDTO.Id,
            Name = unitDTO.Name,
        };
    }
}