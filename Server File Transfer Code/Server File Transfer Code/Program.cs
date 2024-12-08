using System.Net.Sockets;
using System.Net;

try
{
    int port = 9000;
    TcpListener listener = new TcpListener(IPAddress.Any, port);
    listener.Start();
    Console.WriteLine("sunucu bağlandı ve port dinleniyor");

    while (true)
    {
        using (TcpClient client = listener.AcceptTcpClient())
        {
            Console.WriteLine("bir client bağlandı");
            using (NetworkStream ns = client.GetStream())
            {
               
                byte[] fileNameLengthBuffer = new byte[4];
                ns.Read(fileNameLengthBuffer, 0, fileNameLengthBuffer.Length);
                int fileNameLength = BitConverter.ToInt32(fileNameLengthBuffer, 0);

                byte[] fileNameBuffer = new byte[fileNameLength];
                ns.Read(fileNameBuffer, 0, fileNameBuffer.Length);
                string fileName = System.Text.Encoding.UTF8.GetString(fileNameBuffer);

                
                using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    ns.CopyTo(stream);
                    Console.WriteLine("Dosya başarıyla kaydedildi");
                }
            }
        }
        

    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}