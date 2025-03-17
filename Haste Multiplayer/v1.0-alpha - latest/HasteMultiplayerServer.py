import asyncio
import websockets
import json

clients = {}  # Stores clients' WebSocket connection and transform data

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
        print(f"Client {client_id} disconnected.")
        clients.pop(client_id, None)

async def broadcast(sender_id, data):
    # Distributes latest positions and rotations of all clients back
    message = json.dumps({
        "id": sender_id,
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
    async with websockets.serve(handle_client, "localhost", 50000):
        print("WebSocket Server started on ws://localhost:50000")
        await asyncio.Future()

asyncio.run(main())
