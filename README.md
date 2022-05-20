# ThinGin
### Purpose
ThinGin (Thin rendering Engine) is intended to provide an efficient abstraction layer overtop the most common rendering APIs (Vulkan, DirectX, OpenGL).

### Abstraction layer
A Rendering API asbstraction layer provides conveniant abstractions for the fundamental concepts which all graphics APIs support.   
Concepts such as: Meshes, Shaders, GBuffers, Textures, Compute Resources, Etc.   

### Refactor Status
The current system which exists on the main branch is merely a quick test implementation, its purpose was to explore what some of the requirements would be for the final system.   
There are a few things which have little purpose being included in the final system and which only introduce bloat.   
Such as: The world object system, VirtualConsole system, GameObject system, etc. Before the initial release, these aspects will (likely) be moved into their own project if they are kept at all.   
As such, ThinGin is undergoing a major rewrite to move to a much more elegant abstraction via an RHI (Render Hardware Interface) system.   
This rewrite is happening over on the RHI_Refactor branch.   
There is no ETA.

### Examples
The included example project showcases creating and rendering mesh objects.
The camera can be controlled to navigate and look around the scene.

##### Camera Controls
Movement = WASD / Spacebar   
Rotation = LeftClick & Drag   


### Usage
// TODO
