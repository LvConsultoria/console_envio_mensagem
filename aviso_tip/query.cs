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
    internal class query
    {

        public void PuxaDados()
        {
            string cs = @"Data Source=190.8.17.4,1433;Initial Catalog=SuaFibra;User ID=checklist;Password=fij@2919;";

            List<IxcModel> IXC = new List<IxcModel>();

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(" select razao,telefone_celular from tempdb ", conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        IxcModel model = new IxcModel()
                        {
                            //Id = sdr.GetString("id"),
                            Razao = sdr.GetString("razao"),
                            //Cnpj_cpf = sdr.GetString("cnpj_cpf"),
                            Numero = sdr.GetString("telefone_celular")



                        };

                        //IXC.Add(new IxcModel { Id = model.Id,Razao = model.Razao,Cnpj_cpf = model.Cnpj_cpf,Numero = model.Numero});
                        //PublicVariable.ListaIxc.Add(new IxcModel { Id = model.Id, Razao = model.Razao, Cnpj_cpf = model.Cnpj_cpf, Numero = model.Numero });


                        var numero = model.Numero.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "");

                        string msg = "todo cliente Sua Fibra tem automaticamente o plano mínimo de *600 megas* por R$99,90. Sua Fibra entrega o que promete e é 100% homologada pela Anatel 👷‍♂️. Como você sabe nosso suporte é 24 horas pelo whatsapp sem robô 🤖 , sem plano de fidelidade e fique com a gente porque ama 🫶 e não por obrigação 🔒 não aceite enganações. A MELHOR INTERNET SUA FIBRA, INTERNET COMO DEVE SER 🎉";
                        string img = "https://i.postimg.cc/W4S6xCBX/template-war.png";
                        query q = new query();
                        q.diadospais(numero,model.Razao.ToString());






                        _ = new LogWritter(numero + " " + model.Razao);
                        Console.WriteLine(model.Razao);
                        

                    };
                };
                conn.Close();
            }
            
        }
        public void PuxaDados1()
        {
            string cs = @"Server=190.8.16.11;Database=ixcprovedor;Uid=root;Pwd=fij@2919;Connection Timeout=30;Allow Zero Datetime=True";

            List<IxcModel> IXC = new List<IxcModel>();

            using (MySqlConnection conn = new MySqlConnection(cs))
            {
                MySqlDataAdapter dt = new MySqlDataAdapter(" SELECT * FROM ixcprovedor.cliente_contrato where status_internet = 'FA'  order by id_cliente desc", conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                DataTable ds = new DataTable();
                conn.Open();
                dt.Fill(ds);
                var teste = ds.Rows.Count;
               foreach(DataRow item in ds.Rows)
                {

                    MySqlDataAdapter dt1 = new MySqlDataAdapter("select razao,telefone_celular,fn.data_vencimento from ixcprovedor.cliente c inner join ixcprovedor.fn_areceber fn on fn.id_cliente = c.id where c.id = '" + item["id_cliente"].ToString() +"' and fn.status = 'A' order by data_vencimento asc limit 1", conn);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    DataTable ds1 = new DataTable();
                    dt1.Fill(ds1);
                    var teste2 = ds1.Rows.Count;

                    foreach(DataRow item1 in ds1.Rows)
                    {

                        try
                        {
                            var numero = item1["telefone_celular"].ToString().Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "");

                            query q = new query();
                            q.atraso("62994234769", item1["razao"].ToString(), DateTime.Parse(item1["data_vencimento"].ToString()).ToString("dd/MM/yyyy"));
                        }
                        catch
                        {

                        }
                    }
                }
              
                
                conn.Close();
            }

        }


        public void envia(string numero,string nome,string login,string senha,string pin)
        {

            var client = new RestClient("https://waba.360dialog.io/v1/messages");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("D360-Api-Key", "lGo0HKIs9cuAVqKma77EQtiTAK");
            request.AddHeader("Content-Type", "application/json");
            var body = "{\"to\":\"55"+numero+"\",\"type\":\"template\",\"template\":{\"namespace\":\"9778e7d8_76c6_42f4_9ad6_5d8a0c34655a\",\"language\":{\"policy\":\"deterministic\",\"code\":\"pt_BR\"},\"name\":\"aviso_tip1\",\"components\":[{\"type\":\"header\",\"parameters\":[{\"type\":\"image\",\"image\":{\"link\":\"https://i.postimg.cc/7h93LWP6/Logo-SS.jpg\"}}]},{\"type\":\"body\",\"parameters\":[{\"type\":\"text\",\"text\":\""+nome+"\"},{\"type\":\"text\",\"text\":\""+login+"\"},{\"type\":\"text\",\"text\":\""+senha+"\"},{\"type\":\"text\",\"text\":\""+pin+"\"}]}]}}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            Console.WriteLine(response.StatusCode);
        }


        public void enviasorteio(string numero, string nome)
        {

            var client = new RestClient("https://waba.360dialog.io/v1/messages");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("D360-Api-Key", "lGo0HKIs9cuAVqKma77EQtiTAK");
            request.AddHeader("Content-Type", "application/json");
            var body = "{\"to\":\"55"+numero+ "\",\"type\":\"template\",\"template\":{\"namespace\":\"9778e7d8_76c6_42f4_9ad6_5d8a0c34655a\",\"language\":{\"policy\":\"deterministic\",\"code\":\"pt_BR\"},\"name\":\"aviso_sorteio\",\"components\":[{\"type\":\"header\",\"parameters\":[{\"type\":\"image\",\"image\":{\"link\":\"https://i.postimg.cc/cLQFXMQh/8704d889-0992-4b10-91e1-78a564589533.jpg\"}}]},{\"type\":\"body\",\"parameters\":[{\"type\":\"text\",\"text\":\"" + nome +"\"}]}]}}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }



    
        public void acao(string numero,string nome,string msg,string img)
        {

            var client = new RestClient("https://waba.360dialog.io/v1/messages");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("D360-Api-Key", "lGo0HKIs9cuAVqKma77EQtiTAK");
            request.AddHeader("Content-Type", "application/json");
            var body = "{\"to\":\"55"+numero+"\",\"type\":\"template\",\"template\":{\"namespace\":\"9778e7d8_76c6_42f4_9ad6_5d8a0c34655a\",\"language\":{\"policy\":\"deterministic\",\"code\":\"pt_BR\"},\"name\":\"alerta_aviso\",\"components\":[{\"type\":\"header\",\"parameters\":[{\"type\":\"image\",\"image\":{\"link\":\""+img+"\"}}]},{\"type\":\"body\",\"parameters\":[{\"type\":\"text\",\"text\":\""+nome+"\"},{\"type\":\"text\",\"text\":\""+msg+"\"}]}]}}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
        public void atraso(string numero,string nome,string data)
        {
            var client = new RestClient("https://waba.360dialog.io/v1/messages");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("D360-Api-Key", "lGo0HKIs9cuAVqKma77EQtiTAK");
            request.AddHeader("Content-Type", "application/json");
            var body = "{\"to\":\"55"+ numero + "\",\"type\":\"template\",\"template\":{\"namespace\":\"9778e7d8_76c6_42f4_9ad6_5d8a0c34655a\",\"language\":{\"policy\":\"deterministic\",\"code\":\"pt_BR\"},\"name\":\"aviso_atraso\",\"components\":[{\"type\":\"header\",\"parameters\":[{\"type\":\"image\",\"image\":{\"link\":\"https://i.postimg.cc/wx5k70Kk/cobran-a-amigavel.jpg\"}}]},{\"type\":\"body\",\"parameters\":[{\"type\":\"text\",\"text\":\""+nome+"\"},{\"type\":\"text\",\"text\":\""+data+"\"}]}]}}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public void diadospais(string numero, string nome)
        {
            var client = new RestClient("https://waba.360dialog.io/v1/messages");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("D360-Api-Key", "lGo0HKIs9cuAVqKma77EQtiTAK");
            request.AddHeader("Content-Type", "application/json");
            var body = "{\"to\":\"55" + numero + "\",\"type\":\"template\",\"template\":{\"namespace\":\"9778e7d8_76c6_42f4_9ad6_5d8a0c34655a\",\"language\":{\"policy\":\"deterministic\",\"code\":\"pt_BR\"},\"name\":\"aviso_diasdospais\",\"components\":[{\"type\":\"header\",\"parameters\":[{\"type\":\"image\",\"image\":{\"link\":\"https://i.postimg.cc/HWPpNJhh/img2.jpg\"}}]},{\"type\":\"body\",\"parameters\":[{\"type\":\"text\",\"text\":\"" + nome + "\"}]}]}}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

    }
}
