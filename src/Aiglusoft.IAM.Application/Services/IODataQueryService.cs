using Aiglusoft.IAM.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Application.Services
{
  public interface IODataQueryService<TEntity>
    where TEntity : class
  {
    /// <summary>
    /// Exécute une requête OData sur la source IQueryable fournie.
    /// </summary>
    /// <param name="query">La requête IQueryable sur laquelle appliquer les options OData.</param>
    /// <param name="queryOptions">Les options OData incluant le filtrage, tri, pagination, etc.</param>
    /// <param name="cancellationToken">Le jeton d'annulation pour gérer les requêtes asynchrones.</param>
    /// <returns>Un résultat OData contenant les éléments, le compte total et un lien suivant si applicable.</returns>
    Task<ODataResult<object>> ExecuteQueryAsync(IQueryable<TEntity> query, ODataQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken);
    string GenerateNextLink(Uri uri, ODataQueryOptions<TEntity> queryOptions, int skip);
  }

}
