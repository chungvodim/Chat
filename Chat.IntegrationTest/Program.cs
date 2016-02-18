using jabber;
using jabber.client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.IntegrationTest
{
    class Program
    {
        static ManualResetEvent done = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            JID jid = new JID("long", "localhost", "Jabber.Net");
            JabberClient client = new JabberClient();

            client.User = jid.User;
            client.Password = "111111";
            client.Server = jid.Server;
            client.Resource = jid.Resource;
            client.Port = 5222;
            client.AutoReconnect = 1;
            client.KeepAlive = 300F;
            client.AutoLogin = true;
            client.AutoPresence = true;
            client.AutoRoster = true;

            client.SSL = false;
            client.PlaintextAuth = true;
            client.AutoStartTLS = false;
            client.AutoStartCompression = false;

            client.OnConnect += client_OnConnect;
            client.OnInvalidCertificate += client_OnInvalidCertificate;
            client.OnError += new bedrock.ExceptionHandler(client_OnError);
            client.OnIQ += client_OnIQ;
            client.OnProtocol += client_OnProtocol;
            client.OnMessage += client_OnMessage;
            client.OnReadText += new bedrock.TextHandler(client_OnReadText);
            client.OnWriteText += new bedrock.TextHandler(client_OnWriteText);
            client.OnDisconnect += client_OnDisconnect;
            client.OnPresence += client_OnPresence;
            client.OnAuthError += client_OnAuthError;
            client.OnAuthenticate += new bedrock.ObjectHandler(client_OnAuthenticate);
            client.OnLoginRequired += new bedrock.ObjectHandler(client_OnLoginRequired);

            //RosterManager rm = new RosterManager();
            //rm.Stream = client;
            //rm.AutoSubscribe = true;
            //rm.AutoAllow = jabber.client.AutoSubscriptionHanding.AllowAll;
            //rm.OnRosterBegin += new bedrock.ObjectHandler(rm_OnRosterBegin);
            //rm.OnRosterEnd += new bedrock.ObjectHandler(rm_OnRosterEnd);
            //rm.OnRosterItem += new RosterItemHandler(rm_OnRosterItem);

            //PresenceManager pm = new PresenceManager();
            //pm.Stream = client;

            

            client.Connect();
            //client.Login();
            //done.WaitOne();
            //client.Presence(jabber.protocol.client.PresenceType.available, "dang ranh", "sieu ranh", 1);
            while (!client.IsAuthenticated)
            {
                Thread.Sleep(1000);
                Console.Write(".");
            }
            while (true)
            {
                var content = Console.ReadLine();
                jabber.protocol.client.Message msg = new jabber.protocol.client.Message(client.Document);
                msg.From = "long";
                msg.Type = jabber.protocol.client.MessageType.chat;
                msg.To = "admin@sdev";
                msg.Body = content;
                client.Write(msg);
                Console.WriteLine("is authenticated: {0}", client.IsAuthenticated);
            }
        }

        private static void client_OnLoginRequired(object sender)
        {
            Console.WriteLine("LoginRequired");
        }

        static void client_OnWriteText(object sender, string txt)
        {
            Console.WriteLine("WriteText: " + txt);
        }

        static void client_OnAuthError(object sender, System.Xml.XmlElement rp)
        {
            Console.WriteLine("Authentication Error: " + rp.Name);
        }

        private static void rm_OnRosterItem(object sender, jabber.protocol.iq.Item ri)
        {
            Console.WriteLine("OnRosterItem");
        }

        private static void rm_OnRosterEnd(object sender)
        {
            Console.WriteLine("OnRosterEnd");
        }

        private static void rm_OnRosterBegin(object sender)
        {
            Console.WriteLine("OnRosterBegin");
        }

        static void client_OnPresence(object sender, jabber.protocol.client.Presence pres)
        {
            Console.WriteLine("Presence: " + pres.Status);
        }

        static void client_OnDisconnect(object sender)
        {
            Console.WriteLine("Disconnected");
        }

        static void client_OnReadText(object sender, string txt)
        {
            Console.WriteLine("Received a message: " + txt);
        }

        static void client_OnMessage(object sender, jabber.protocol.client.Message msg)
        {
            Console.WriteLine("Received a message: " + msg.Body);
        }

        private static void client_OnError(object sender, Exception ex)
        {
            Console.WriteLine("Error");
        }

        private static void client_OnProtocol(object sender, System.Xml.XmlElement rp)
        {
            Console.WriteLine("Protocol: " + rp.ToString());
        }

        private static void client_OnIQ(object sender, jabber.protocol.client.IQ iq)
        {
            Console.WriteLine("OnIQ: " + iq.ToString());
        }

        private static bool client_OnInvalidCertificate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine("InvalidCertificate");
            return false;
        }

        private static void client_OnAuthenticate(object sender)
        {
            Console.WriteLine("Authenticating");
        }

        private static void client_OnConnect(object sender, jabber.connection.StanzaStream stream)
        {
            Console.WriteLine("Connecting to server");
        }
    }
}
