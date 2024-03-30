extern alias OTAPI;
using TShockAPI;
using OTAPI.Terraria;
using TerrariaApi.Server;
using OTAPI::Terraria.Net.Sockets;
using System.Net.Sockets;
using System.Net;
using OTAPI.Terraria.Localization;
using OTAPI.Terraria.Net;
using OTAPI.Terraria.Net.Sockets;

namespace Testplugin2;

[ApiVersion(2, 1)]
public class Testplugin2 : TerrariaPlugin
{
    //private static MainForm? MyForm;
    private static Task? FormTask;
    public Command AddCommand;
    public Testplugin2(Main game) : base(game)
    {
        //AddCommand = new Command(Cmd, "myform");
    }

    public override void Initialize()
    {
        Commands.ChatCommands.Add(AddCommand);
        //ServerApi.Hooks.ServerLeave.Register(this, OnServerLeave);
        ServerApi.Hooks.GamePostInitialize.Register(this, OnGamePostInitialize);
    }

    private void OnGamePostInitialize(EventArgs args)
    {
        typeof(OTAPI.OTAPI.Hooks.Netplay).GetField("CreateTcpListener", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .SetValue(null, new EventHandler<OTAPI.OTAPI.Hooks.Netplay.CreateTcpListenerEventArgs>((sender, e) => new LinuxTcpSocket()));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Commands.ChatCommands.Remove(AddCommand);
            ServerApi.Hooks.GamePostInitialize.Deregister(this, OnGamePostInitialize);
            //ServerApi.Hooks.ServerLeave.Deregister(this, OnServerLeave);
            //MyForm?.Dispose();
        }
    }

    //private void OnServerLeave(LeaveEventArgs args)
    //{
    //    Console.WriteLine(args.Who);
    //}
    //private void Cmd(CommandArgs args)
    //{
    //    if(Environment.OSVersion.Platform != PlatformID.Win32NT)
    //    {
    //        args.Player.SendInfoMessage("非Windows系统不可使用");
    //        return;
    //    }
    //    RunForm(args);
    //}
    //private static void RunForm(CommandArgs args)
    //{
    //    if (args.Parameters.Count == 0)
    //    {
    //        if (MyForm is null || MyForm.IsDisposed)
    //        {
    //            MyForm = new MainForm();
    //        }
    //        if (FormTask is null || FormTask.IsCompleted)
    //        {
    //            FormTask = Task.Run(() =>
    //            {
    //                Application.EnableVisualStyles();
    //                Application.SetCompatibleTextRenderingDefault(false);
    //                Application.Run(MyForm);
    //            });
    //        }
    //        else
    //        {
    //            MyForm.Show();
    //        }
    //    }
    //    else if (args.Parameters[0] == "dis")
    //    {
    //        MyForm?.Dispose();
    //    }
    //}
}

public class LinuxTcpSocket : ISocket
{
    public byte[] _packetBuffer = new byte[1024];

    public int _packetBufferLength;

    public List<object> _callbackBuffer = new List<object>();

    public int _messagesInQueue;

    public TcpClient _connection;

    public TcpListener _listener;

    public SocketConnectionAccepted _listenerCallback;

    public RemoteAddress _remoteAddress;

    public bool _isListening;

    public int MessagesInQueue => this._messagesInQueue;

    public LinuxTcpSocket()
    {
        this._connection = new TcpClient();
        this._connection.NoDelay = true;
    }

    public LinuxTcpSocket(TcpClient tcpClient)
    {
        this._connection = tcpClient;
        this._connection.NoDelay = true;
        IPEndPoint iPEndPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
        this._remoteAddress = new TcpAddress(iPEndPoint.Address, iPEndPoint.Port);
    }

    void ISocket.Close()
    {
        this._remoteAddress = null;
        this._connection.Close();
    }

    bool ISocket.IsConnected()
    {
        if (this._connection != null && this._connection.Client != null)
        {
            return this._connection.Connected;
        }
        return false;
    }

    void ISocket.Connect(RemoteAddress address)
    {
        TcpAddress tcpAddress = (TcpAddress)address;
        this._connection.Connect(tcpAddress.Address, tcpAddress.Port);
        this._remoteAddress = address;
    }

    private void ReadCallback(IAsyncResult result)
    {
        Tuple<SocketReceiveCallback, object> tuple = (Tuple<SocketReceiveCallback, object>)result.AsyncState;
        try
        {
            tuple.Item1(tuple.Item2, this._connection.GetStream().EndRead(result));
        }
        catch (InvalidOperationException)
        {
            ((ISocket)this).Close();
        }
        catch (Exception ex2)
        {
            TShock.Log.Error(ex2.ToString());
        }
    }

    private void SendCallback(IAsyncResult result)
    {
        object[] obj = (object[])result.AsyncState;
        LegacyNetBufferPool.ReturnBuffer((byte[])obj[1]);
        Tuple<SocketSendCallback, object> tuple = (Tuple<SocketSendCallback, object>)obj[0];
        try
        {
            this._connection.GetStream().EndWrite(result);
            tuple.Item1(tuple.Item2);
        }
        catch (Exception)
        {
            ((ISocket)this).Close();
        }
    }

    void ISocket.SendQueuedPackets()
    {
    }

    void ISocket.AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state)
    {
        byte[] array = LegacyNetBufferPool.RequestBuffer(data, offset, size);
        this._connection.GetStream().BeginWrite(array, 0, size, SendCallback, new object[2]
        {
            new Tuple<SocketSendCallback, object>(callback, state),
            array
        });
    }

    void ISocket.AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state)
    {
        this._connection.GetStream().BeginRead(data, offset, size, ReadCallback, new Tuple<SocketReceiveCallback, object>(callback, state));
    }

    bool ISocket.IsDataAvailable()
    {
        return this._connection.GetStream().DataAvailable;
    }

    RemoteAddress ISocket.GetRemoteAddress()
    {
        return this._remoteAddress;
    }

    bool ISocket.StartListening(SocketConnectionAccepted callback)
    {
        IPAddress address = IPAddress.Any;
        if (OTAPI.Terraria.Program.LaunchParameters.TryGetValue("-ip", out var value) && !IPAddress.TryParse(value, out address))
        {
            address = IPAddress.Any;
        }
        this._isListening = true;
        this._listenerCallback = callback;
        if (this._listener == null)
        {
            this._listener = new TcpListener(address, Netplay.ListenPort);
        }
        try
        {
            this._listener.Start();
        }
        catch (Exception)
        {
            return false;
        }
        ThreadPool.QueueUserWorkItem(ListenLoop);
        return true;
    }

    void ISocket.StopListening()
    {
        this._isListening = false;
    }

    private void ListenLoop(object unused)
    {
        while (this._isListening && !Netplay.Disconnect)
        {
            try
            {
                ISocket socket = new LinuxTcpSocket(this._listener.AcceptTcpClient());
                Console.WriteLine(Language.GetTextValue("Net.ClientConnecting", socket.GetRemoteAddress()));
                this._listenerCallback(socket);
            }
            catch (Exception)
            {
            }
        }
        this._listener.Stop();
        Netplay.IsListening = false;
    }
}
