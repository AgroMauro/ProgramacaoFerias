using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Portal.ViewModels.Administracao;
using Portal.ViewModels.Configuracoes;
using Portal.ViewModels.Ofertas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Services
{
    public class OfertasService
    {
        #region Variaveis
        private static string uri = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ApiBaseURL")["Atento.Horizon.API"];
        private static string pathArquivo = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ArquivoCSV")["Path"];
        #endregion
      
        public async Task<string> SalvarOfertas([FromBody] string jsonOfertas)
        {
            string Metodo = string.Concat("/Ofertas/SalvarConfiguracaoOfertas");
            string Url = string.Concat(uri, Metodo);
            string Retorno = string.Empty;

            var httpContent = new StringContent(jsonOfertas, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {

                using (var response = await client.PostAsync(Url, httpContent))
                {
                    Retorno = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : response.ReasonPhrase.ToString();
                }
            }

            return Retorno;
        }

        public List<OfertaViewModel> GetOfertas(int idOperacao)
        {
            string Metodo = string.Concat("/Ofertas/GetOfertas/", idOperacao.ToString());
            string Url = string.Concat(uri, Metodo);
            string Retorno = string.Empty;
            List<OfertaViewModel> result = new List<OfertaViewModel>();

            using (var client = new HttpClient())
            {

                using (var response = client.GetAsync(Url).GetAwaiter().GetResult())
                {
                    Retorno = response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().GetAwaiter().GetResult() : response.ReasonPhrase.ToString();
                }
            }

            result = JsonConvert.DeserializeObject<List<OfertaViewModel>>(Retorno);

            return result;
        }
    }
}
