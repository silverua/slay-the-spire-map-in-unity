# Slay the Spire Map in Unity

Demo video:

[![FEATURES](https://img.youtube.com/vi/gHPh3trkJWc/0.jpg)](https://youtu.be/gHPh3trkJWc)

Longer video with some how-to info:

[![HOW-TO](https://img.youtube.com/vi/P9ogBkLWmPQ/0.jpg)](https://youtu.be/P9ogBkLWmPQ)

This repo contains an implementation of the Slay the Spire Map in Unity3d. 
Key features:
- Pick any orientation (left to right, right to left, top to bottom, bottom to top)
- Map appearance and player position get saved between game sessions
- Node position randomization
- Cross nodes elimination
- Scrolling map content for long maps

Free resources and assets used in the project:
- [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
- Icons from [Game-icons.net](https://game-icons.net). You can find a full list of credits in the folder with icons
- [Newtonsoft.JSON for Unity](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347) 

December 13 2022 Update:
Thanks to [hojjatjafary](https://github.com/hojjatjafary)
- Better algorithm for generating paths
- New algorithm uses a for loop instead of a while loop to generate paths, which makes it more precise and eliminates the need of limiting max amount of attempts
- New algorithm respects precise numbers of starting nodes and pre-boss nodes
- Use the new field `extraPaths` in MapConfig to generate more paths and create more connections between the nodes

February 25 2023 Update:
Got several requests to make this map work with Unity UI and scroll rects. 
In this update: 
- New test scene with the Unity UI version of the map: Assets/Scenes/SampleSceneUI.unity
- New UI-specific prefabs: Assets/Prefabs/MapObjectsUI Variant.prefab, Assets/Prefabs/UINode.prefab, Assets/Prefabs/UILine.prefab
- UI setup requires 2 scroll rects created for horizontal, vertical maps. Check out the SampleSceneUI for an example of this.
