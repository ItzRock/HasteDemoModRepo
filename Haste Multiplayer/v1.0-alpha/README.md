# Haste Multiplayer
> Uses [MelonLoader](https://github.com/LavaGang/MelonLoader)

## Version notes
> v1.0-alpha

> Dev note : this is the very first mod's release. It is very unstable, and only works in the DemoHub scene. You should not install it if you wish to play the game, as this version is dedicated to testing. You can only play on your local network on ports 50000 (localhost:50000).

## Installation
- If MelonLoader isn't installed, install it [here](https://github.com/LavaGang/MelonLoader).
- If Python isn't installed, install it [here](https://www.python.org/).
- Download the `HasteMultiplayerServer.py` server. You can put it anywhere on your disk.
- Download and put both mod `HasteMultiplayer.dll` and dependency `websocket-sharp.dll` in the `Mods` folder in your game files.

## Setup the client-Server connection
- Make sure both clients are on the same local network.
- Launch the `HasteMultiplayerServer.py` server. You should see `WebSocket Server started on ws://localhost:50000` printed in the console.
- Launch the game.

You should see the other client's player as a rock, moving on your screen.

## Changelog
- Added Multiplayer
> Dev note : You have infinite energy

## To-do
- Use the player's model instead of a rock
- Maintain the websocket connection when loading a new scene
- Syncronize both client's map
- Add an custom adress and port selection system
