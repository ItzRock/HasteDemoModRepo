
# Haste Broken Worlds Demo Mod Repository

> [!NOTE]  
> This for the Demo of Haste. These mods were made before the game launched and probably will not be maintained.

## How do I download a single mod?
Navigate to the mod which you'd like to download and click on the mod's `.dll` file and on the right side of your screen there should be a download icon. If the mod includes assetbundles or other dlls then it may be easier to download each individuially or the whole repository depending how you'd like to do it.

## How do I install these mods?
These mods mainly use BepInEx, unless specifically stated in their READMEs. The steps go:
- Install [BepInEx 5](https://github.com/BepInEx/BepInEx/releases/latest) (not 6) for your current platform (drag and drop zip contents into `C:\Program Files (x86)\Steam\steamapps\common\Haste Broken Worlds Demo`, or wherever else you have Haste installed).
- Launch the game once to generate BepInEx's files. You should see folders such as `config` created inside `Haste Broken Worlds Demo\BepInEx`.
- Create a `plugins` folder inside the `BepInEx` folder if it does not already exist.
- Drag and drop the DLL file(s) from any BepInEx mod into that plugins folder.
- Launch the game, and the mods should load.
### Video Guide
https://github.com/user-attachments/assets/bf38c947-ce3b-45b8-9619-3d8c396e16a9

## How do I add my mod?

 1. Fork the repository. (If you press the `.` key you can enter a VS code editor on your web browser and update from there.)
 2. Create a folder in the base directory with your mod name.
 3. Create a readme in your folder.
 4. Create a folder for each version and optionally a folder for a latest version so it would look something like

```
ExampleMod/
├─ v1.2/
│  ├─ example_mod.dll
├─ v1.1/
│  ├─ example_mod.dll
├─ v1.0/
│  ├─ example_mod.dll
├─ latest/ (optional)
│  ├─ example_mod.dll
├─ README.md

```

5. Create a pull request. (optionally ping me in the Haste server @anthony.stai)

## Some brief guidelines.
* Mods uploaded should be BepInEx (unless special permission to use melonloader by Anthony) and `Assembly-CSharp.dll` modifications are absolutely not okay.
* No obfuscated mods.
* Please be sure you aren't using other people's copyrighted assets without permission.
If your unsure please join the official Haste: Broken Worlds Discord server down below.

## Need more help?
Come join us in the `#haste-modding` channel in the official [Haste: Broken Worlds community discord server](https://discord.gg/hastebrokenworlds). We're happy to help!
