- Move all resource decoders to a unified container within engine instance
- Refactor texture system to remove need for end users to utilize the texture factory
	- Users should just be able to create a new instance of the texture class.
	- Texture class should wrap some type of texture handle reference object which can be either a raw/proxy/or shared texture backing object.

- Implement VarDataBuffer which is basically a UniformBuffer object for storing global shader var values, all of the graphics (dx, opengl, vulkan) apis have an equivalent for this concept.

- Overhaul the ShaderVar/VariableBlock system



====== [v0.5 REFACTOR] ======
SEE: https://www.youtube.com/watch?v=qx1c190aGhs

- Move render engine crap into RHI implementations
	RHI = RenderHardwareImplementation

- Frames collected into RHICommandList instances that essentially contain which shaders to bind and which verticies to draw for it
	(But its more complex, review the video linked)

- Move engine object life handling to seperate, dedicated object
- MeshRenderer needs to be something else, maybe the RHI?
- Meshes need to also know what shader they are using and what set of shader vars too.

- Replace Pixel Buffer Objects with a unified GByteBuffer object


- Move EngineInstance GFX handling stuff into its render manager
- Add defaults for Shader objects
- Create RHI command system
- need vertex factorys
- need to move things like mesh and camera into component system



===========================================
			NEW RHI SYSTEM
===========================================

Okay so in the new RHI based system the general idea is that everything is abstracted into layers like so:

[Engine Component] 
	Uses RHI objects to facilitate specific engine level components and features.
[RHI Abstract Object] 
	Represents the general idea of objects and capabilities of rendering hardware. Eg; the general concept of a pixel shader, or a buffer holding vertex/index data.
[Driver Implementation]
	Facilitates the actual implementation of objects and capabilities provided by the RHI using the specific feature set of the underlying graphics driver.


	
===========================================
				DIFFERENCES
===========================================

- We dont use BulkDataInterface objects, we use DMAInterface objects which are given a kind of resource context object which describes what it is being used for (buffer, texture, etc)


