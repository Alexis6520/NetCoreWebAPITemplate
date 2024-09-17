using Application.Services.Abstractions;
using Application.Services.DTOs.Donuts;

namespace Application.Services.Queries.Donuts
{
    /// <summary>
    /// Consulta para enlistar todas las donas
    /// </summary>
    public class GetDonutListQuery : IRequest<List<DonutDTO>>
    {
    }
}
