using Aiglusoft.IAM.Domain.Model;
using Aiglusoft.IAM.Domain.Model.CodeValidators;
using Aiglusoft.IAM.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;


namespace Aiglusoft.IAM.Infrastructure.Repositories
{
    public class CodeValidatorRepository : ICodeValidatorRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public CodeValidatorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CodeValidator> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.CodeValidators.FirstOrDefaultAsync(e=>e.Id == id, cancellationToken);
        }

        public async Task SaveAsync(CodeValidator codeValidator, CancellationToken cancellationToken = default)
        {
            var existingEntity = await _context.CodeValidators.FirstOrDefaultAsync(e => e.Id == codeValidator.Id);
            if (existingEntity == null)
            {
                _context.Add(codeValidator);
            }
            else
            {
                _context.Update(codeValidator);
            }

            await UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<CodeValidator> FindByCodeAndTargetAsync(string code, string target, CancellationToken cancellationToken = default)
            => await _context.CodeValidators.FirstOrDefaultAsync(cv => cv.Code == code && cv.Target == target, cancellationToken);

        public async Task InvalidatePreviousCodesAsync(string target, CancellationToken cancellationToken = default)
        {
            var activeCodes = await _context.CodeValidators.Where(cv => cv.Target == target && cv.Status == CodeStatus.Active)
                                          .ToListAsync();

            foreach (var code in activeCodes)
            {
                code.MarkAsCancelled();
                _context.Update(code);
            }
        }
    }

}
