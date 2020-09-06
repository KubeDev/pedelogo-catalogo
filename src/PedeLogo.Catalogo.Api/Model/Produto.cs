using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PedeLogo.Catalogo.Api.Model
{
    public class Produto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
        public string Categoria { get; set; }
    }
}