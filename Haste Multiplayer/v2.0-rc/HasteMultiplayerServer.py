import asyncio
import websockets
import json
import random

clients = {}  # Stores clients' WebSocket connection and transform data
currentSeed = 0
currentLevelID = 0
server_port = str(input("Enter server port (default is 50000) : "))
if server_port == "": server_port = "50000"

async def UpdateSeed():
    # Update global shared seed
    global currentSeed
    global currentLevelID
    while True:
        currentSeed = random.randint(1000, 1000000)
        currentLevelID = random.randint(1000, 1000000)
        #print(f" currentSeed was set to : {currentSeed}, currentLevelID was set to : {currentLevelID}")
        await asyncio.sleep(10)

async def handle_client(websocket):
    global clients
    try: 
        #Receive data from clients
        data = await websocket.recv()
        data = json.loads(data)
        client_id = data["id"]
        clients[client_id] = {"websocket": websocket, "transform": {}}
        
        print(f"New client connected: {client_id}")

        async for message in websocket:
            data = json.loads(message) 

            # Store the latest position and rotation of all clients
            clients[client_id]["transform"] = {
                "PositionValue": data["PositionValue"],
                "RotationValue": data["RotationValue"]  
            }

            # Broadcast to all clients
            await broadcast(client_id, data)

    except websockets.exceptions.ConnectionClosed:
        print(f"A client disconnected.")
        clients.pop(client_id, None)

async def broadcast(sender_id, data):
    # Distributes latest positions and rotations of all clients back
    global currentSeed
    global currentLevelID
    message = json.dumps({
        "id": sender_id,
        "currentSeed": currentSeed,
        "currentLevelID": currentLevelID,
        "PositionValue": data["PositionValue"],
        "RotationValue": data["RotationValue"]
    })

    disconnected_clients = []
    for client_id, client_info in clients.items():
        try:
            await client_info["websocket"].send(message)
        except websockets.exceptions.ConnectionClosed:
            disconnected_clients.append(client_id)

    # Removes disconnected clients on the list
    for client_id in disconnected_clients:
        print(f"Client disconnected and was removed from clients list : {client_id}")
        clients.pop(client_id, None)

async def main():
    # Run Server
    async with websockets.serve(handle_client, "0.0.0.0", server_port):
        print(f"WebSocket Server started on ws://0.0.0.0:{server_port}")
        asyncio.create_task(UpdateSeed())
        await asyncio.Future()

asyncio.run(main())
