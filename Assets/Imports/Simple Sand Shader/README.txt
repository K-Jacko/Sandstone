# SimpleSandShader

A simple shader that creates a sand wave around a spheric object.

## Installation

1. On the Unity Editor go to Assets > ImportPackage > Custom Package and chose the 'SimpleSandShader.unitypackage' that you just downloaded.
2. On the Import Unity Package window click Import.
3. Open the SimpleShaderScene.unity that you will find in the path 'Assets/SimpleSandShader'
4. Go to Layers > Edit Layers..., click the icon between the cogwheel and the info icon and select the 'SimpleSandShaderLayers'.
5. Go to Edit > Project Settings > Physics, click the icon between the cogwheel and the info icon and select the 'SimpleSandShaderPhysics'.


## Usage

The Sand.shader has multiple properties.

Four of them are related to the TrackedEntity which is the object surrounded by sand. Those properties are hidden in the inspector and are automatically 
updated every frame by the Sand.cs script. Those are:

- TrackedEntityPosition: The position of the tracked object.
- TrackedEntityVelocity: The velocity of the tracked object.
- TrackedEntityRadius: The radius of the tracked object. This one can be changed on the TrackedEntity.cs.
- TrackedEntityPercentUndergroud The percentage of the tracked object that is underground.

Properties unrelated to the TrackedEntity:

- MainText: The sand's texture.
- HeightMap: The noise's texture.
- TrackedEntityMaxVelocity: That maximum velocity that the system will track. A bigger TrackedEntity velocity will have no further effect on the final shape 
of the wave.
- TrackedEntityVelocityContribution: How much the velocity contributes to the final wave shape. Setting it to 0 will make velocity have no impact whatsoever.
- UndulationInnerWidth: The width of the inner undulation. That is the distance from the TrackedEntity's edge to the wave crest.
- UndulationOuterWidth: The width of the outter undulation. That is the distance from the wave's crest to its external trough.
- UndulationMinHeight: The minimum wave's height.
- UndulationMaxHeight: The maximum wave's height.
- Tesselation: Subdivisions on the sand mesh. This allows the terrain to deform and adapt better to the expected shapes. Keep it as low as posible as it impacts
performance.

## License

MIT License
https://opensource.org/licenses/MIT