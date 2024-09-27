namespace Aiglusoft.IAM.Application.Queries
{
  public class ODataQueryOptions<TEntity>
  {

    /// <summary>
    /// Gets or sets the filter expression (e.g., "Title == 'Act1'").
    /// </summary>
    public string Filter { get; set; }

    /// <summary>
    /// Gets or sets the order by expression (e.g., "CreatedAt desc").
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of items to return (default 10, max 100).
    /// </summary>
    public int Top { get; set; } = 10;

    /// <summary>
    /// Gets or sets the number of items to skip (default 0).
    /// </summary>
    public int Skip { get; set; } = 0;

    /// <summary>
    /// Gets or sets the fields to select (e.g., "Title, Description").
    /// </summary>
    public string Select { get; set; }
  }
}
