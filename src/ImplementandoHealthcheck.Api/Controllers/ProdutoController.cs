using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImplementandoHealthcheck.Api.Models;
using System.Threading;

namespace ImplementandoHealthcheck.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProdutoController : Controller
    {
        // GET api/produtos
        [HttpGet("")]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return new List<Produto>() { new Produto() { Nome = "Geladeira", Descricao = "Uma geladeira muito eficiente." }, new Produto() { Nome = "Microondas", Descricao = "Esse Microondas é muito bom." } };
        }

        // GET api/produtos/5
        [HttpGet("{id}")]
        public ActionResult<Produto> GetById(int id)
        {           
            return new Produto() { Nome = "Geladeira", Descricao = "Uma geladeira muito eficiente." };
        }

        // POST api/produtos
        [HttpPost("")]
        public void Post([FromBody] Produto produto) { }

        // PUT api/produtos/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Produto produto) { }

        // DELETE api/produtos/5
        [HttpDelete("{id}")]
        public void DeleteById(int id) 
        {
            if (DateTime.Now.Minute % 5 == 0)
            {
                throw new Exception("Teste de erro do endpoint.");
            }
        }
    }
}