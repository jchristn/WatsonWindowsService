using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace WatsonWindowsService
{
    public partial class WatsonService : ServiceBase
    {
        #region Public-Members

        #endregion

        #region Private-Members

        private WatsonTcpServer _Server;
        private string _Logfile = "./watson.log";
        private readonly object _Lock = new object();

        #endregion

        #region Constructors-and-Factories

        public WatsonService()
        {
            InitializeComponent();
        }

        #endregion

        #region Public-Methods

        protected override void OnStart(string[] args)
        {
            _Server = new WatsonTcpServer("127.0.0.1", 8000);
            _Server.Events.ClientConnected += ClientConnected;
            _Server.Events.ClientDisconnected += ClientDisconnected;
            _Server.Events.MessageReceived += MessageReceived;

            _Server.Start();

            Log("WatsonTcp server started");
        }

        protected override void OnStop()
        {
            if (_Server != null)
            {
                _Server.Stop();
            }

            Log("WatsonTcp server stopped");
        }

        #endregion

        #region Private-Methods

        private void Log(string msg)
        {
            if (String.IsNullOrEmpty(msg)) return;

            if (!msg.EndsWith(Environment.NewLine)) msg += Environment.NewLine;

            lock (_Lock)
            {
                File.AppendAllText(_Logfile, msg);
            }
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Log("Message received from " + e.IpPort + ": " + Encoding.UTF8.GetString(e.Data));

            _Server.Send(e.IpPort, "Echo: " + Encoding.UTF8.GetString(e.Data));
        }

        private void ClientDisconnected(object sender, DisconnectionEventArgs e)
        {
            Log("Client " + e.IpPort + " disconnected");
        }

        private void ClientConnected(object sender, ConnectionEventArgs e)
        {
            Log("Client " + e.IpPort + " connected");
        }

        #endregion
    }
}
