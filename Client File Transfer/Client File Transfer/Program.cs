using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_File_Transfer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string serverIp = "45.94.170.239";
            int port = 9000; 
            string filePath = "C:\\Users\\Developer\\Desktop\\Gonderilecek Dosya";



            try
            {
                if (Directory.Exists(filePath))
                {
                    Console.WriteLine($"klasör doğru {filePath}");
                    foreach (var file in Directory.GetFiles(filePath))
                    {
                        using (TcpClient client = new TcpClient(serverIp, port))
                        {
                            Console.WriteLine("Sunucuya bağlanıldı");
                            using (NetworkStream stream = client.GetStream())
                            {
                                string fileName = Path.GetFileName(file);
                                byte[] fileNameBuffer = System.Text.Encoding.UTF8.GetBytes(fileName);
                                byte[] fileNameLengthBuffer = BitConverter.GetBytes(fileNameBuffer.Length);

                                stream.Write(fileNameLengthBuffer, 0, fileNameLengthBuffer.Length);
                                stream.Write(fileNameBuffer, 0, fileNameBuffer.Length);


                                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                                {
                                    fs.CopyTo(stream);
                                    Console.WriteLine($"Dosya başarıyla gönderild {file}");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Böyle klasör yok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();


        }
    }
}
