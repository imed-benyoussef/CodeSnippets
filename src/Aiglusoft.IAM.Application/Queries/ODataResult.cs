namespace Aiglusoft.IAM.Application.Queries
{
  public class ODataResult<T>
  {
    public IEnumerable<T> Value { get; set; }           // Les éléments retournés
    public int? Count { get; set; }              // Le nombre total d'éléments (facultatif)
    public string NextLink { get; set; }         // Lien vers la page suivante (facultatif)
  }
}
