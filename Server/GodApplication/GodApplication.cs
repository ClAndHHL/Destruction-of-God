using GodCommon;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using GodServer.Handlers;
using log4net;
using log4net.Config;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer
{
    class GodApplication : ApplicationBase
    {
        private static GodApplication _instance;
        private static readonly ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();
        public Dictionary<byte, HandlerBase> handlers = new Dictionary<byte, HandlerBase>();
        public List<ClientPeer> peerListForTeam = new List<ClientPeer>();  //需要组队的List

        public static GodApplication Instance
        {
            get
            {
                return _instance;
            }
        }

        public GodApplication()
        {
            _instance = this;
            RegisterHandler();
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new ClientPeer(initRequest.Protocol, initRequest.PhotonPeer);
        }

        protected override void Setup()
        {
            ExitGames.Logging.LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(ApplicationRootPath, "log");
            GlobalContext.Properties["LogFileName"] = "D" + ApplicationName;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(BinaryPath, "log4net.config")));

            log.Debug("Application Setup");
        }

        protected override void TearDown()
        {
            log.Debug("Application TearDown");
        }

        void RegisterHandler()  //注册Handler
        {
            handlers.Add((byte)OperationCode.Login, new LoginHandler());
            handlers.Add((byte)OperationCode.Register, new RegisterHandler());
            handlers.Add((byte)OperationCode.ServerList, new ServerProperyHandler());
            handlers.Add((byte)OperationCode.Role, new RoleHandler());
            handlers.Add((byte)OperationCode.TaskDB, new TaskDBHandler());
            handlers.Add((byte)OperationCode.InventoryDB, new InventoryDBHandler());
            handlers.Add((byte)OperationCode.SkillDB, new SkillDBHandler());
            handlers.Add((byte)OperationCode.Fight, new FightHandler());
            handlers.Add((byte)OperationCode.Enemy, new EnemyHandler());
            handlers.Add((byte)OperationCode.Boss, new BossHandler());
        }
    }
}
