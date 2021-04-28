﻿using LeagueBroadcast.Common.Data.Config;
using LeagueBroadcast.OperatingSystem;
using System;

namespace LeagueBroadcast.Common.Controllers
{
    class ConfigController
    {
        private static ConfigController _instance;
        public static ConfigController Instance => GetInstance();

        public static ChampSelect.Data.Config.PickBanConfig PickBan = new();

        public static ComponentConfig Component = new();
        public ConfigController()
        {
            Log.Info("Starting Config Controller");
            var controller = JSONConfigProvider.Instance;

            controller.ReadConfig(PickBan);
            controller.ReadConfig(Component);

            if(PickBan.FileVersion == null || Component.FileVersion == null)
            {
                Log.Warn("Config load failed");
                throw new Exception("Config load failed");
            }

            App.Instance.Exit += OnClose;
        }

        private static ConfigController GetInstance()
        {
            if (_instance == null)
                _instance = new();
            return _instance;
        }

        public static void UpdateConfigFile(JSONConfig config)
        {
            JSONConfigProvider.Instance.WriteConfig(config);
        }

        private void OnClose(object sender, EventArgs e)
        {
            Log.Info("Saving all configs to file");
            var controller = JSONConfigProvider.Instance;

            controller.WriteConfig(PickBan);
            controller.WriteConfig(Component);
            Log.Info("Configs saved");
        }
    }
}
