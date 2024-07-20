using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain.Model.CodeValidators
{
    public interface ICodeValidatorRepository : IRepository<CodeValidator>
    {
        Task<CodeValidator> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task SaveAsync(CodeValidator codeValidator, CancellationToken cancellationToken = default);
        Task<CodeValidator> FindByCodeAndTargetAsync(string code, string target, CancellationToken cancellationToken = default);
        Task InvalidatePreviousCodesAsync(string target, CancellationToken cancellationToken = default);
    }

}
