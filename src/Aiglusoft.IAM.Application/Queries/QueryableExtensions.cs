using System.Linq.Expressions;
using System.Reflection;

namespace Aiglusoft.IAM.Application.Queries
{
  public static class QueryableExtensions
  {
    // Extension pour ToListAsync avec réflexion
    public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> source, CancellationToken cancellation = default)
    {
      // Recherche si une méthode ToListAsync existe via réflexion
      var toListAsyncMethod = GetMethod<T>("ToListAsync", source);

      // Si la méthode existe, on l'invoque via réflexion
      if (toListAsyncMethod != null)
      {
        var result = toListAsyncMethod.Invoke(null, new object[] { source });
        if (result is Task<List<T>> task)
        {
          return await task;
        }
      }

      // Si aucune méthode ToListAsync n'est trouvée, on utilise l'implémentation personnalisée
      return await CustomToListAsync(source);
    }

    // Extension pour CountAsync avec réflexion
    public static async Task<int> CountAsync<T>(this IQueryable<T> source, CancellationToken cancellation = default)
    {
      // Recherche si une méthode CountAsync existe via réflexion
      var countAsyncMethod = GetMethod<T>("CountAsync", source);

      // Si la méthode existe, on l'invoque via réflexion
      if (countAsyncMethod != null)
      {
        var result = countAsyncMethod.Invoke(null, new object[] { source });
        if (result is Task<int> task)
        {
          return await task;
        }
      }

      // Si aucune méthode CountAsync n'est trouvée, on utilise l'implémentation personnalisée
      return await CustomCountAsync(source);
    }

    // Méthode générique pour rechercher une méthode asynchrone via réflexion
    private static MethodInfo GetMethod<T>(string methodName, IQueryable<T> source)
    {
      var queryableType = typeof(IQueryable<T>);
      var methodInfo = queryableType
          .Assembly
          .GetTypes()
          .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
          .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(IQueryable<T>));

      return methodInfo;
    }

    // Implémentation personnalisée de ToListAsync pour simuler une exécution asynchrone
    private static Task<List<T>> CustomToListAsync<T>(IQueryable<T> source)
    {
      return Task.Run(() => source.ToList());
    }

    // Implémentation personnalisée de CountAsync pour simuler une exécution asynchrone
    private static Task<int> CustomCountAsync<T>(IQueryable<T> source)
    {
      return Task.Run(() => source.Count());
    }

    public static async Task<ODataResult<T>> ToODataResultAsync<T>(
            this IQueryable<T> source,
            int pageNumber,
            int pageSize,
            string baseUrl,
            CancellationToken cancellationToken = default)
    {
      // Vérifier et limiter la taille de la page
      pageSize = pageSize > 100 ? 100 : (pageSize < 1 ? 10 : pageSize);

      // Nombre total d'éléments avant pagination
      var totalItems = await source.CountAsync(cancellationToken);

      // Appliquer la pagination
      var items = await source.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync(cancellationToken);

      // Construire le lien vers la page suivante si nécessaire
      string nextLink = null;
      if ((pageNumber * pageSize) < totalItems)
      {
        nextLink = $"{baseUrl}?$skip={(pageNumber * pageSize)}&$top={pageSize}";
      }

      // Construire et retourner le résultat sous format OData
      return new ODataResult<T>
      {
        //Context = $"{baseUrl}/$metadata#Items",   // Contexte des métadonnées
        Value = items,                           // Les éléments paginés
        Count = totalItems,                      // Le nombre total d'éléments
        NextLink = nextLink                      // Lien vers la page suivante, s'il y en a une
      };
    }

    // Méthode pour appliquer Include via la réflexion
    public static IQueryable<T> ApplyInclude<T>(this IQueryable<T> query, string navigationProperty)
    {
      // Vérifier si la méthode Include existe
      var includeMethod = query.GetType()
          .GetMethods(BindingFlags.Static | BindingFlags.Public)
          .FirstOrDefault(m => m.Name == "Include" && m.GetParameters().Length == 2);

      if (includeMethod == null)
      {
        throw new InvalidOperationException("Include method not found.");
      }

      // Spécifier le type générique pour Include<Act>
      var genericMethod = includeMethod.MakeGenericMethod(typeof(T), typeof(object));

      // Invoquer Include dynamiquement sur la propriété de navigation
      var result = genericMethod.Invoke(null, new object[] { query, navigationProperty });

      return (IQueryable<T>)result;
    }


    // Méthode générique pour invoquer FirstOrDefaultAsync avec une expression de filtrage
    public static async Task<T> ApplyFirstOrDefaultAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
      // Récupérer la méthode FirstOrDefaultAsync via réflexion
      var firstOrDefaultMethod = query.GetType()
          .GetMethods(BindingFlags.Static | BindingFlags.Public)
          .FirstOrDefault(m => m.Name == "FirstOrDefaultAsync" && m.GetParameters().Length == 2);

      if (firstOrDefaultMethod == null)
      {
        throw new InvalidOperationException("FirstOrDefaultAsync method not found.");
      }

      // Spécifier le type générique pour FirstOrDefaultAsync<T>
      var genericMethod = firstOrDefaultMethod.MakeGenericMethod(typeof(T));

      // Appliquer le Where avec l'expression lambda
      var filteredQuery = query.Where(predicate);

      // Invoquer FirstOrDefaultAsync avec la requête filtrée
      var task = (Task<T>)genericMethod.Invoke(null, new object[] { filteredQuery, cancellationToken });

      return await task;
    }
  }
}
