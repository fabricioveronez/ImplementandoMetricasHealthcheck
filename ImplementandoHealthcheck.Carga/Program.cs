using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace ImplementandoHealthcheck.Carga
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("http://localhost:8080/api");

            for (int i = 0; i < 2000; i++)
            {
                var resGetAll = cliente.GetStringAsync("http://localhost:8080/api/produto").Result;
                var resGetOne = cliente.GetStringAsync("http://localhost:8080/api/produto/1").Result;
                var resPost = cliente.PostAsync("http://localhost:8080/api/produto", new StringContent(JsonConvert.SerializeObject(new { nome = "Celular", descricao = "Celular muito bom." }))).Result;
                var resPut = cliente.PutAsync("http://localhost:8080/api/produto/1", new StringContent(JsonConvert.SerializeObject(new { nome = "Celular", descricao = "Celular muito bom." }))).Result;
                var resDelete= cliente.DeleteAsync("http://localhost:8080/api/produto/1").Result;
            }
        }
    }
}
