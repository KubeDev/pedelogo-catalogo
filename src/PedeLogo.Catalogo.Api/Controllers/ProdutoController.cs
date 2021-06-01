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
            var resultado = this._collection.Find(new BsonDocument()).ToList();
            this._logger.LogInformation("Entrou no Get All");
            return resultado;
        }

        [HttpGet("{id}", Name = "GetProduto")]
        public Produto Get(string id)
        {
            this._logger.LogInformation("Entrou no Get By Id");
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
            this._logger.LogInformation("Entrou no Post");
            this._collection.InsertOne(produto);
            return Ok();
        }

        [HttpPut("{id}")]
        public void Put([FromRoute] string id, [FromBody] Produto produto)
        {
            this._logger.LogInformation("Entrou no Put");
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
            this._logger.LogInformation("Entrou no Delete");
            this._collection.FindOneAndDelete(obj => obj.Id.Equals(id));
        }
    }
}