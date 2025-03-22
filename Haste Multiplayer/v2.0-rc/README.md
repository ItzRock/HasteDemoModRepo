# Haste Multiplayer
> Uses [MelonLoader](https://github.com/LavaGang/MelonLoader)

## Version notes
> v2.0-rc

> Dev note : this is the release candidate for the release version. Here be dragons ! 

## Installation
- If MelonLoader isn't installed, install it [here](https://github.com/LavaGang/MelonLoader).
- If Python isn't installed, install it [here](https://www.python.org/). You will also need Python's [websocket library](https://pypi.org/project/websockets/).
- The host downloads the `HasteMultiplayerServer.py` server. You can put it anywhere on your disk.
- Download and put both mod `HasteMultiplayer.dll` and dependency `websocket-sharp.dll` in the `Mods` folder in your game files.

## Setup the client-Server connection
- Set your port when launching the server. Those ports must be port forwarded (by default, the config choses 50000).
- Launch the game once. When in the main menu, close the game.
- Set the server adress and port in the `HasteMultiplayerConfig.json` file located in the same directory as the mod.
- The host launches the `HasteMultiplayerServer.py` server. You should see `WebSocket Server started on ws://0.0.0.0:<port>` printed in the console.
- Launch the game.

You should see the other client's player, moving on your screen.

## Changelog
- Used the player's model instead of a rock
- Maintained the websocket connection when loading a new scene
- Syncronized both client's map
- Added an custom adress and port selection system

## To-do
- Set an animation player for the Networked Player Mesh
