using Microsoft.Extensions.VectorData;

namespace Catalog.Domain;

public class ProductVector
{
    [VectorStoreKey]
    public int Id { get; set; }

    [VectorStoreData]
    public string Name { get; set; } = default!;

    [VectorStoreData]
    public string Description { get; set; } = default!;

    [VectorStoreData]
    public decimal Price { get; set; }

    [VectorStoreData]
    public string ImageUrl { get; set; } = default!;

    [VectorStoreVector(Dimensions: 384, DistanceFunction = DistanceFunction.CosineSimilarity)]
    public ReadOnlyMemory<float> Vector { get; set; }
}
