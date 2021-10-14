using MyFinances.Core.Dtos;
using MyFinances.Core.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFinances.Services
{
    public interface IOperationService
    {
        Task<DataResponse<int>> AddAsync(OperationDto item);
        Task<Response> UpdateAsync(OperationDto item);
        Task<Response> DeleteAsync(int id);
        Task<DataResponse<OperationDto>> GetAsync(int id);
        Task<DataResponse<IEnumerable<OperationDto>>> GetAsync();
        Task<DataResponse<IEnumerable<OperationDto>>> GetPaginatedAsync(int numberPage = 1);
    }
}
