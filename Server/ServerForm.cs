using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ServerForm
{
    public partial class ServerForm : Form
    {
        private static Socket Server;
        private const int PortNumber = 2000;
        private static bool ServerStarted = false;
        private List<Socket> Connections = new List<Socket>();

        public ServerForm()
        {
            InitializeComponent();
            SetSendButton(enabled: false);
        }

        private void StartServer()
        {
            try
            {
                Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Server.Bind(new IPEndPoint(IPAddress.Any, PortNumber));
                Server.Listen(10);

                SetToggleConnectionButtonText("Stop Server");
                ServerStarted = true;
                SetSendButton(enabled: true);
                AddMessage("Server has been started");

                Server.BeginAccept(new AsyncCallback(AcceptCallback), Server);
            }
            catch(Exception)
            {
                AddMessage("Could not start server");
            }
        }

        private void StopServer()
        {
            foreach(Socket connection in Connections) connection.Close();

            ClearMessageList();
            Server.Close();
            SetToggleConnectionButtonText("Start Server");
            ServerStarted = false;
            SetSendButton(enabled: false);
            AddMessage("Server has been stopped");
        }

        public void AcceptCallback(IAsyncResult asyncResult)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client = Server.EndAccept(asyncResult);

                lock(Connections)
                {
                    Connections.Add(client);
                }

                AddMessage("Connected to a new client");

                StateObject state = new StateObject();
                state.client = client;
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                Server.BeginAccept(new AsyncCallback(AcceptCallback), Server);
            }
            catch(Exception)
            {
                client.Close();

                lock(Connections)
                {
                    Connections.Remove(client);
                }
            }
        }

        public void ReceiveCallback(IAsyncResult asyncResult)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                StateObject state = (StateObject)asyncResult.AsyncState;
                client = state.client;
                int bytesRead = client.EndReceive(asyncResult);
                string message = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                if (bytesRead > 0)
                {
                    AddMessage(message);
                    foreach (Socket receiverClient in Connections) Send(receiverClient, message);
                }
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch(Exception)
            {
                AddMessage("Disconnected from client");

                client.Close();

                lock (Connections)
                {
                    Connections.Remove(client);
                }
            }
        }

        private void Send(Socket client, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);

            lock(client)
            {
                client.Send(data);
            }
        }

        delegate void EmptyControlCallback();
        delegate void TextControlCallback(string value);
        delegate void BooleanControlCallback(bool value);

        private void AddMessage(string message)
        {
            if (messageList.InvokeRequired)
            {
                TextControlCallback callback = new TextControlCallback(AddMessage);
                Invoke(callback, new object[] { message });
            }
            else
            {
                messageList.Items.Add(message);
            }
        }

        private void SetToggleConnectionButtonText(string text)
        {
            if (toggleServerButton.InvokeRequired)
            {
                TextControlCallback callback = new TextControlCallback(SetToggleConnectionButtonText);
                Invoke(callback, new object[] { text });
            }
            else
            {
                toggleServerButton.Text = text;
            }
        }

        private void ClearMessageBox()
        {
            if (messageBox.InvokeRequired)
            {
                EmptyControlCallback callback = new EmptyControlCallback(ClearMessageBox);
                Invoke(callback);
            }
            else
            {
                messageBox.Text = String.Empty;
            }
        }

        private void SetSendButton(bool enabled)
        {
            if (sendButton.InvokeRequired)
            {
                BooleanControlCallback callback = new BooleanControlCallback(SetSendButton);
                Invoke(callback, new object[] { enabled });
            }
            else
            {
                sendButton.Enabled = enabled;
            }
        }

        private void ClearMessageList()
        {
            if (messageList.InvokeRequired)
            {
                EmptyControlCallback callback = new EmptyControlCallback(ClearMessageList);
                Invoke(callback);
            }
            else
            {
                messageList.Items.Clear();
            }
        }

        private void SendButtonOnClick(object sender, EventArgs e)
        {
            string message = messageBox.Text;
            ClearMessageBox();
            AddMessage(message);

            foreach (Socket client in Connections) Send(client, message);
        }

        private void ToggleServerButtonOnClick(object sender, EventArgs e)
        {
            if (ServerStarted) StopServer();
            else StartServer();
        }

        private void MessageBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendButtonOnClick(sender, e);
                e.Handled = true;
            }
        }
    }

    public class StateObject
    {
        public Socket client = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder data = new StringBuilder();
    }
}
