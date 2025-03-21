using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using WebSocketSharp;
using MelonLoader;
using HarmonyLib;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using MelonLoader.Utils;
using Landfall.Haste;
[assembly: MelonLoader.MelonOptionalDependencies("Microsoft.CSharp")]

namespace HasteMultiplayer
{
    public class ConfigData // Websocket network configuration
    {
        public string server_address { get; set; }
        public int server_port { get; set; }
    }
    public class HasteMultiplayerClass : MelonMod // Defines WebSocket properties
    {
        private static WebSocket ws;
        private static string clientId = Guid.NewGuid().ToString();
        public float lastLogTime = Time.time;
        public int CurrentSeed = 0;
        public int CurrentLevelID = 0;
        public int LastShardIdValue = 0;
        private static string configPath = Path.Combine(MelonEnvironment.ModsDirectory, "HasteMultiplayerConfig.json");
        public static ConfigData Config { get; private set; }

        public class RecievedData // Server data management
        {
            public string id;
            public int currentSeed;
            public int currentLevelID;
            public Vector3 PositionValue;
            public Quaternion RotationValue;
        }
        public override void OnInitializeMelon() // Connect to server
        {

            MelonLogger.Msg("HasteMultiplayer mod initialized!");
            LoadConfig();
            StartWebSocket();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName) //Instantiate player model on scene load
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            if (GameObject.Find("Player"))
            {
                Transform PlayerModel = GameObject.Instantiate(GameObject.Find("Courier_Retake")).transform;
                PlayerModel.name = "NetworkedPlayer";
                GameObject.Destroy(PlayerModel.transform.Find("Courier/CameraJoint").gameObject);
                GameObject.Destroy(PlayerModel.transform.Find("Courier/SFX").gameObject);
            }
        }

        public override void OnUpdate() // Called every frame
        {

            SendTransformData(); // Send player data to the server every frame

            if (GameObject.Find("Player"))
            {
                if (SimpleRunHandler.currentShardID != LastShardIdValue) // Sets seed to global seed on leaving/entering DemoHub
                {
                    SimpleRunHandler.currentSeed = CurrentSeed;
                    SimpleRunHandler.currentLevelID = CurrentLevelID;
                    LastShardIdValue = SimpleRunHandler.currentShardID;
                }
            }
        }

        private void LoadConfig() // Loads HasteMultiplayerConfig.json data
        {
            try
            {
                if (!File.Exists(configPath)) // Create default config if missing
                {
                    Config = new ConfigData { server_address = "localhost", server_port = 50000 };
                    File.WriteAllText(configPath, JsonConvert.SerializeObject(Config, Formatting.Indented));
                    MelonLogger.Msg("Created default config file.");
                }
                else // Read config
                {
                    string json = File.ReadAllText(configPath);
                    Config = JsonConvert.DeserializeObject<ConfigData>(json);
                    MelonLogger.Msg($"Config file loaded successfully. Loading WebSocket on ws://{Config.server_address}:{Config.server_port}");
                }
            }
            catch (Exception e) // Could not read config file
            {
                MelonLogger.Error($"Error loading config file: {e.Message}");
                Config = new ConfigData { server_address = "localhost", server_port = 50000 };
            }
        }

        private void StartWebSocket() // WebSocket setup and connection
        {
            ws = new WebSocket($"ws://{Config.server_address}:{Config.server_port}");

            ws.OnOpen += (sender, e) =>
            {
                MelonLogger.Msg($"Connected to WebSocket server on ws://{Config.server_address}:{Config.server_port}");
                SendTransformData();
            };

            ws.OnMessage += (sender, e) =>
            {
                RecieveServerData(e.Data);
            };

            ws.OnClose += (sender, e) =>
            {
                MelonLogger.Msg("Connection closed. And error might have occured.");
            };

            ws.OnError += (sender, e) =>
            {
                if (Time.time - lastLogTime >= 2f) // Check if 2 seconds passed
                {
                    MelonLogger.Error("WebSocket error: " + e.Message);
                    lastLogTime = Time.time;
                }
            };

            ws.Connect(); // Connect to WebSocket
        }

        private void SendTransformData() // Sends player data to the server
        {
            // Check if there is player
            if (PlayerCharacter.localPlayer != null)
            {
                Vector3 PlayerPosition = PlayerCharacter.localPlayer.transform.position;
                PlayerVisualRotation playerVisual = GameObject.FindObjectOfType<PlayerVisualRotation>();
                Quaternion PlayerRotation = playerVisual.visual.localRotation;

                var TransformData = new
                {
                    id = clientId,
                    PositionValue = new { PlayerPosition.x, PlayerPosition.y, PlayerPosition.z },
                    RotationValue = new { PlayerRotation.x, PlayerRotation.y, PlayerRotation.z, PlayerRotation.w }
                };

                string jsonData = JsonConvert.SerializeObject(TransformData);
                ws.Send(jsonData);
            }
        }

        private void RecieveServerData(string data) // Handles received data from the server
        {
            GameObject NetworkedPlayer = GameObject.Find("NetworkedPlayer");

            RecievedData serverData = JsonConvert.DeserializeObject<RecievedData>(data);
            if (serverData.id != clientId) // Dev note : set condition to true for debugging on a single instance
            {
                CurrentSeed = serverData.currentSeed;
                CurrentLevelID = serverData.currentLevelID;

                NetworkedPlayer.transform.position = serverData.PositionValue;
                NetworkedPlayer.transform.rotation = serverData.RotationValue;
                NetworkedPlayer.transform.localScale = new Vector3(4, 4, 4);
            }
        }
    }
}