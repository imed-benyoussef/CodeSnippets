using Aiglusoft.IAM.Application.Queries;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;

namespace Aiglusoft.IAM.Application.Services
{
  public class ODataQueryService<TEntity> : IODataQueryService<TEntity>
    where TEntity : class
  {
    public ODataQueryService() { }

    public async Task<ODataResult<object>> ExecuteQueryAsync(IQueryable<TEntity> query, ODataQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
    {
      // Appliquer les options OData sur la requête existante
      query = PrepareQuery(query, queryOptions);

      // Compter le total des éléments avant pagination
      var totalItems = await query.CountAsync(cancellationToken);

      // Appliquer la pagination
      query = ApplyPaging(query, queryOptions);

      // Sélection dynamique des champs
      var resultQuery = ApplySelect(query, queryOptions.Select);

      // Exécuter la requête
      var items = await resultQuery.ToDynamicListAsync(cancellationToken);

      // Retourner le résultat OData
      return CreateODataResult(items, totalItems);
    }

    private IQueryable<TEntity> PrepareQuery(IQueryable<TEntity> query, ODataQueryOptions<TEntity> queryOptions)
    {
      // Appliquer les filtres dynamiques et l'ordre sur la requête existante
      query = ApplyDynamicFilter(query, queryOptions.Filter);
      query = ApplyDynamicOrder(query, queryOptions.OrderBy);

      return query;
    }

    private IQueryable<TEntity> ApplyDynamicFilter(IQueryable<TEntity> query, string filter)
    {
      if (string.IsNullOrEmpty(filter)) return query;

      try
      {
        return query.Where(filter);
      }
      catch (ParseException ex)
      {
        throw new ArgumentException($"Erreur dans le filtre : {ex.Message}");
      }
    }

    private IQueryable<TEntity> ApplyDynamicOrder(IQueryable<TEntity> query, string orderBy)
    {
      if (string.IsNullOrEmpty(orderBy)) return query;

      try
      {
        return query.OrderBy(orderBy);
      }
      catch (ParseException ex)
      {
        throw new ArgumentException($"Erreur dans le tri : {ex.Message}");
      }
    }

    private IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, ODataQueryOptions<TEntity> queryOptions)
    {
      return query.Skip(queryOptions.Skip).Take(queryOptions.Top);
    }

    private IQueryable ApplySelect(IQueryable<TEntity> query, string select)
    {
      if (string.IsNullOrEmpty(select)) return query;

      var fields = string.Join(",", select.Split(',').Select(f => f.Trim()));
      try
      {
        return query.Select($"new({fields})");
      }
      catch (ParseException ex)
      {
        throw new ArgumentException($"Erreur dans la sélection des champs : {ex.Message}");
      }
    }


    private ODataResult<dynamic> CreateODataResult(dynamic items, int totalItems)
    {
      return new ODataResult<dynamic>
      {
        Value = items,
        Count = totalItems
      };
    }

    public string GenerateNextLink(Uri uri, ODataQueryOptions<TEntity> queryOptions, int skip)
    {
      // Obtenir les paramètres de requête existants de l'URI
      var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);

      // Mettre à jour ou ajouter le paramètre $skip avec la nouvelle valeur
      queryParameters["$skip"] = skip.ToString();

      // Ajouter ou conserver les autres paramètres OData (si présents)
      if (!string.IsNullOrEmpty(queryOptions.Filter))
      {
        queryParameters["$filter"] = queryOptions.Filter;
      }

      if (!string.IsNullOrEmpty(queryOptions.OrderBy))
      {
        queryParameters["$orderby"] = queryOptions.OrderBy;
      }

      if (!string.IsNullOrEmpty(queryOptions.Select))
      {
        queryParameters["$select"] = queryOptions.Select;
      }

      // Reconstruire l'URI avec les paramètres de requête mis à jour
      var uriBuilder = new UriBuilder(uri)
      {
        Query = queryParameters.ToString() // Mettre à jour la chaîne de requête avec les nouveaux paramètres
      };

      // Retourner l'URI complète sous forme de chaîne
      return uriBuilder.ToString();
    }

  }

}
