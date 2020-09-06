using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using PedeLogo.Catalogo.Api.Model;

namespace PedeLogo.Catalogo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ILogger<ProdutoController> _logger;
        private readonly IMongoCollection<Produto> _collection;

        public ProdutoController(ILogger<ProdutoController> logger, IMongoDatabase mongoDB)
        {
            this._logger = logger;
            this._collection = mongoDB.GetCollection<Produto>("Produto");
        }
        
        [HttpGet]
        public IEnumerable<Produto> Get()
        {
            return this._collection.Find(new BsonDocument()).ToList();
        }

        [HttpGet("{id}", Name = "GetProduto")]
        public Produto Get(string id)
        {
            ObjectId objID;
            if (!ObjectId.TryParse(id, out objID))
            {
                throw new Exception("Erro ao converter.");
            }

            return this._collection.Find(p => p.Id.Equals((id))).FirstOrDefault();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Produto produto)
        {
            this._collection.InsertOne(produto);
            return Ok();
        }

        [HttpPut("{id}")]
        public void Put([FromRoute] string id, [FromBody] Produto produto)
        {
            ObjectId objID;
            if (!ObjectId.TryParse(id, out objID))
            {
               throw new Exception("Id errado");
            }
            
            this._collection.FindOneAndReplace(obj => obj.Id.Equals(produto.Id), produto);
        }
        
        [HttpDelete]
        public void Delete(string id)
        {
            this._collection.FindOneAndDelete(obj => obj.Id.Equals(id));
        }
    }
}