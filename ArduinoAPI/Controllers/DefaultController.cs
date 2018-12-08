using ArduinoAPI.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace ArduinoAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ApiVersionRoutePrefix("notification", 1)]
    public class DefaultController : ApiController
    {
        [HttpGet]
        [Route("main")]
        public IHttpActionResult Notification()
        {
            try
            {
                //Pegar a data e hora atual (Horário de Brasília), para enviar dentro da notificação.
                var dateCurrent = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
                string date = string.Format("{0:d/M/yyyy} às {0:HH:mm:ss.fff}", dateCurrent);
                long ordenation = long.Parse(dateCurrent.ToString("yyyyMMddHHmmssfff"));

                //Dados da notificação, definindo também o nome do tópico (GENERAL), prioridade, e tempo de vida da notificação.
                var data = new
                {
                    to = "/topics/GENERAL",
                    data = new
                    {
                        date,
                        ordenation
                    },
                    priority = "high",
                    content_available = true,
                    time_to_live = 3
                };

                //Definindo os dados para a requisição ao Firebase Cloud Messaging
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", "AIzaSyD5v2jQvz8FapflwPBKMCO9Dz7tnwCyZio"));
                tRequest.Headers.Add(string.Format("Sender: id={0}", "250247043186"));

                //Serializando os dados da notificação
                var json = new JavaScriptSerializer().Serialize(data);
                var byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.ContentLength = byteArray.Length;

                //Enviando o objeto serializado ao Firebase Cloud Messaging
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);

                //Recebendo a resposta do Firebase Cloud Messaging
                WebResponse tResponse = tRequest.GetResponse();
                Stream dataStreamResponse = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStreamResponse);
                string sResponseFromServer = tReader.ReadToEnd();

                //Finalizando a requisição e as streans abertas
                dataStream.Close();
                tReader.Close();
                dataStreamResponse.Close();
                tResponse.Close();
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                });
            }

            return Ok(new
            {
                Sucesso = true,
                Mensagem = "Notificação enviada com sucesso!"
            });

        }

    }
}