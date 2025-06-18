using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;

namespace Catalog.Application.UseCases;

public class ProductAIService(
    ProductDbContext dbContext,
    IChatClient chatClient,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
    VectorStoreCollection<int, ProductVector> productVectorCollection)
{
    public async Task<string> SupportAsync(string query)
    {
        var systemPrompt = """
        You are a useful assistant. 
        You always reply with a short and funny message. 
        If you do not know an answer, you say 'I don't know that.' 
        You only answer questions related to outdoor camping products. 
        For any other type of questions, explain to the user that you only answer outdoor camping products questions.
        At the end, Offer one of our products: Hiking Poles-$24, Outdoor Rain Jacket-$12, Outdoor Backpack-$32, Camping Tent-$22
        Do not store memory of the chat conversation.
        """;

        var chatHistory = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, systemPrompt),
            new ChatMessage(ChatRole.User, query),
        };

        var chatResponse = await chatClient.GetResponseAsync(chatHistory);

        return chatResponse.Text;
    }

    public async Task<List<Product>> SearchProductsAsync(string query)
    {
        //use emedding generator
        //use in memory vector store
        //semantic search

        if (!await productVectorCollection.CollectionExistsAsync())
        {
            await InitEmbeddingsAsync();
        }

        var queryEmbedding = await embeddingGenerator.GenerateVectorAsync(query);

        var vectorSearchOptions = new VectorSearchOptions<ProductVector> { VectorProperty = pv => pv.Vector };

        var results = productVectorCollection.SearchAsync(queryEmbedding, top: 3, vectorSearchOptions);

        List<Product> products = [];
        await foreach (var resultItem in results)
        {
            products.Add(new Product
            {
                Id = resultItem.Record.Id,
                Name = resultItem.Record.Name,
                Description = resultItem.Record.Description,
                Price = resultItem.Record.Price,
                ImageUrl = resultItem.Record.ImageUrl
            });
        }

        return products;
    }

    private async Task InitEmbeddingsAsync()
    {
        await productVectorCollection.EnsureCollectionExistsAsync();

        // for testing purpose
        var products = await dbContext.Products.ToListAsync();
        foreach (var product in products)
        {
            var productInfo = $"[{product.Name}] is a product that costs [{product.Price}] and is described as [{product.Description}]";

            var productVector = new ProductVector
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Vector = await embeddingGenerator.GenerateVectorAsync(productInfo)
            };

            await productVectorCollection.UpsertAsync(productVector);
        }
    }
}
