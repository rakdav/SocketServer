using System.Net;
using System.Net.Sockets;
using System.Text;

IPAddress ipAddr = IPAddress.Parse("192.168.113.117");
IPEndPoint iPEndPoint=new IPEndPoint(ipAddr, 11000);
Socket sListener=
    new Socket(AddressFamily.InterNetwork, 
    SocketType.Stream, ProtocolType.Tcp);
try
{
    sListener.Bind(iPEndPoint);
    sListener.Listen(10);
    while(true)
    {
        Console.WriteLine("Ожидаем соединение через порт "
            +iPEndPoint);
        Socket handler = sListener.Accept();
        string data = null;
        byte[] buffer = new byte[1024];
        int bytesRead = handler.Receive(buffer);
        data+=Encoding.Unicode.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Получeнный текст:"+data+"\n\n");
        //ответ сервера
        string reply = "Спасибо за запрос в " + 
            data.Length.ToString() + " символов";
        byte[] msg = Encoding.Unicode.GetBytes(reply);
        handler.Send(msg);
        if(data.IndexOf("<TheEnd>")>-1)
        {
            Console.WriteLine("Сервер завершил соединение с клиентом");
            break;
        }
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
