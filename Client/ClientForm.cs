using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class ClientForm : Form
    {
        private static Socket Server;
        private const string HostName = "localhost";
        private const int PortNumber = 2000;

        public ClientForm()
        {
            InitializeComponent();
            Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SetSendButton(enabled: false);
        }

        private void ConnectToServer()
        {
            try
            {
                Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPHostEntry ipHostEntry = Dns.GetHostEntry(HostName);
                IPAddress ipAddress = ipHostEntry.AddressList[1];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, PortNumber);
                Server.Connect(ipEndPoint);

                AddMessage("Connected to server");

                StateObject state = new StateObject();
                state.client = Server;

                Server.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);

                SetToggleConnectionButtonText("Disconnect");
                SetSendButton(enabled: true);
            }
            catch(Exception)
            {
                AddMessage("Unable to connect to server");
            }
        }

        private void DisconnectFromServer()
        {
            Server.Close();

            ClearMessageList();
            AddMessage("Disconnected from server");
            SetToggleConnectionButtonText("Connect");
            SetSendButton(enabled: false);
        }

        public void ReceiveCallback(IAsyncResult asyncResult)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                StateObject state = (StateObject)asyncResult.AsyncState;
                client = state.client;
                if (client.Connected)
                {
                    int bytesRead = client.EndReceive(asyncResult);
                    if (bytesRead > 0) AddMessage(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
            }
            catch(Exception)
            {
                DisconnectFromServer();
            }
        }

        delegate void TextControlCallback(string value);
        delegate void BooleanControlCallback(bool value);
        delegate void EmptyControlCallback();

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
            if (toggleConnectionButton.InvokeRequired)
            {
                TextControlCallback callback = new TextControlCallback(SetToggleConnectionButtonText);
                Invoke(callback, new object[] { text });
            }
            else
            {
                toggleConnectionButton.Text = text;
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

        private void SendButtonOnClick(object sender, EventArgs e)
        {
            string message = messageBox.Text;
            ClearMessageBox();
            Server.Send(Encoding.ASCII.GetBytes(message));
        }

        private void MessageBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendButtonOnClick(sender, e);
                e.Handled = true;
            }
        }

        private void ToggleConnectionButtonOnClick(object sender, EventArgs e)
        {
            if (Server.Connected) DisconnectFromServer();
            else ConnectToServer();
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
