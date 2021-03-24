using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;

namespace DistribuitedLibrary.Gatewayy.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController:ControllerBase {
        public BooksController() {
        }

        [HttpGet("{ibsn}")]
        public async Task<string> Get(long ibsn) {
            BooksService _books = new BooksService();

        string path =  _books.CreateFile();
            try {
                var factory = new ConnectionFactory {
                    HostName = "localhost",
                    Port = 5672
                };
                using (var connection = factory.CreateConnection()) {
                    using (var channel = connection.CreateModel()) {
                        channel.QueueDeclare("send-ibsn", false, false, false, null);
                        var body = Encoding.UTF8.GetBytes(ibsn.ToString());
                        channel.BasicPublish("", "send-ibsn", null, body);
                    }
                }
                SendFilename(path);
            } catch (Exception ex) {
                return "No Existe El Indice";
            }
            return path;
        }

        public void SendFilename(string filename) {
            try {
                var factory = new ConnectionFactory {
                    HostName = "localhost",
                    Port = 5672
                };
                using (var connection = factory.CreateConnection()) {
                    using (var channel = connection.CreateModel()) {
                        channel.QueueDeclare("send-archivo", false, false, false, null);
                        var body = Encoding.UTF8.GetBytes(filename);
                        channel.BasicPublish("", "send-archivo", null, body);
                    }
                }
            } catch (Exception ex) {
            }
        }
    }
}
