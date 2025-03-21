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
[assembly: MelonLoader.MelonOptionalDependencies("Microsoft.CSharp")]

namespace HasteMultiplayer
{
    public class HasteMultiplayerClass : MelonMod
    {
        // Defines WebSocket properties
        private static WebSocket ws;
        private static string clientId = Guid.NewGuid().ToString();
        public class RecievedData
        {
            public string id;
            public Vector3 PositionValue;
            public Quaternion RotationValue;
        }

        // Connect to server
        public override void OnInitializeMelon()
        {

            MelonLogger.Msg("HasteMultiplayer mod initialized!");
            StartWebSocket();
        }

        // WebSocket setup and connection
        private void StartWebSocket()
        {
            // Connect to localhost on port 50000
            ws = new WebSocket("ws://localhost:50000");

            // WebSocket event handlers
            ws.OnOpen += (sender, e) =>
            {
                MelonLogger.Msg("Connected to WebSocket server.");
                SendTransformData();
            };

            ws.OnMessage += (sender, e) =>
            {
                HandleServerMessage(e.Data);
            };

            ws.OnClose += (sender, e) =>
            {
                MelonLogger.Msg("Connection closed.");
            };

            ws.OnError += (sender, e) =>
            {
                MelonLogger.Error("WebSocket error: " + e.Message);
            };

            ws.Connect(); // Connect to WebSocket
        }

        // Called every frame
        public override void OnUpdate()
        {
            SendTransformData(); // Send player data to the server every frame

            if (GameObject.Find("Player"))
            {
                // Dev note : For debugging purposes, gives the player an infinite supply of energy
                PlayerCharacter Player = GameObject.Find("Player").GetComponent<PlayerCharacter>();
                if (Player != null)
                {
                    Player.AddEnergy(1000, 0);  // Add energy to the player
                }

                // Temporary model used as placeholder for the other client's player
                GameObject Rock = GameObject.Find("Mo_SnowWorld_IceSpike_5_AutoLOD");
                Rock.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }

        // Send player data to the server
        private void SendTransformData()
        {
            // Check if there is player
            if (PlayerCharacter.localPlayer != null)
            {
                Vector3 PlayerPosition = PlayerCharacter.localPlayer.transform.position;

                PlayerVisualRotation playerVisual = GameObject.FindObjectOfType<PlayerVisualRotation>();
                Quaternion PlayerRotation = playerVisual.visual.localRotation;

                // Pack transform data
                var TransformData = new
                {
                    id = clientId,
                    PositionValue = new { PlayerPosition.x, PlayerPosition.y, PlayerPosition.z },
                    RotationValue = new { PlayerRotation.x, PlayerRotation.y, PlayerRotation.z, PlayerRotation.w }
                };

                string jsonData = JsonConvert.SerializeObject(TransformData);
                ws.Send(jsonData);
                //MelonLogger.Msg($"Sent Transform Data: {jsonData}");
            }
        }

        // Handle received data from the server
        private void HandleServerMessage(string data)
        {
            // Dev note : Temporary solution, the rock model won't be used in the end and can only be used in the DemoHub Scene.
            GameObject Rock = GameObject.Find("Mo_SnowWorld_IceSpike_5_AutoLOD");
            RecievedData serverData = JsonConvert.DeserializeObject<RecievedData>(data);
            if (serverData.id != clientId)
            {
                MelonLogger.Msg(serverData.PositionValue);
                MelonLogger.Msg(serverData.RotationValue);
                Rock.transform.position = serverData.PositionValue;
                Rock.transform.rotation = serverData.RotationValue;
            }
        }
    }
}
