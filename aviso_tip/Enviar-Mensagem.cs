using MySql.Data.MySqlClient;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aviso_tip
{
    internal class Enviar_Mensagem
    {
        //Variáveis para mandar a mensagem
        private string mensagem = "A Sua Fibra tem como missão a satisfação  do nosso cliente. Por vocês  nos dedicamos 24h e buscamos nos aperfeiçoar  dia após dia para satisfazê-los.  Em agradecimento  a confiança depositada nos nossos serviços juntamente com nossos super parceiros :  @acaidoninja  @pizzariamanago  @originaltechoficial @kmcarcentroautomotivo   Faremos um show de prêmios pelo dia do cliente.";
        private string template = "aviso_generico";
        private string img = "https://i.postimg.cc/HsrfVFPJ/71f5c1ee-c2d9-4004-a998-99fbff425d05.jpg";


        public void Enviador()
        {
            string cs = @"Data Source=190.8.17.4,1433;Initial Catalog=SuaFibra;User ID=checklist;Password=fij@2919;";
            var query = "select razao,telefone_celular from ClienteIxcInfo3";
            SqlConnection conn = new SqlConnection(cs);
            SqlDataAdapter data = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            data.Fill(dt);
                
            foreach(DataRow row in dt.Rows)
            {
                var NumeroDb = row["telefone_celular"].ToString();
                var NomeDb = row["razao"].ToString();
                var numero = NumeroDb.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "");

                
                //Executa o envio para somente 1 numero
                acao("62984889704", template, NomeDb, mensagem, img);

                //Executa o envio para todos os numeros
               // acao(numero, template, NomeDb, mensagem, img);
                Console.WriteLine(NomeDb);
            }
       
        }         
        public void acao(string numero,string template,string var1, string var2,string img)
        {

            var client = new RestClient("https://waba.360dialog.io/v1/messages");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("D360-Api-Key", "lGo0HKIs9cuAVqKma77EQtiTAK");
            request.AddHeader("Content-Type", "application/json");
            var body = "{\"to\":\"55"+numero+"\",\"type\":\"template\",\"template\":{\"namespace\":\"9778e7d8_76c6_42f4_9ad6_5d8a0c34655a\",\"language\":{\"policy\":\"deterministic\",\"code\":\"pt_BR\"},\"name\":\""+ template + "\",\"components\":[{\"type\":\"header\",\"parameters\":[{\"type\":\"image\",\"image\":{\"link\":\""+img+"\"}}]},{\"type\":\"body\",\"parameters\":[{\"type\":\"text\",\"text\":\""+var1+"\"},{\"type\":\"text\",\"text\":\""+var2+"\"}]}]}}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            if(response.IsSuccessful == true)
            {
                _ = new LogWritter("Foi enviado uma mensagem  {"+var2+"}   para o numero " + numero + " com o nome " + var1 + " utilizando o template " + this.template + " com sucesso!");

            }
            else
            {
                _ = new LogWritter("Não foi enviado a mensagem   {"+var2+"}   para o numero " + numero + " com o nome " + var1 + " utilizando o template " + this.template + " sem sucesso!");

            }


        }

    }
}
