using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Visib.Services;

namespace Visib.Api.Services
{
    public class SmsService : ISmsService
    {
        public async Task Send(string number, string message)
        {
            try
            {
                var client = new ReluzCapWebServiceSoapClient(new BasicHttpsBinding(), new EndpointAddress(new Uri("https://webservices2.twwwireless.com.br/reluzcap/wsreluzcap.asmx")));
                await client.OpenAsync();
                await client.EnviaSMSAsync(new EnviaSMSRequest
                {
                    NumUsu = "BIZMIX",
                    Senha = "Haverim3$",
                    SeuNum = "1234",
                    Celular = number,
                    Mensagem = message
                });
                await client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}