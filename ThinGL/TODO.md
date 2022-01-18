- Move all resource decoders to a unified container within engine instance
- Refactor texture system to remove need for end users to utilize the texture factory
	- Users should just be able to create a new instance of the texture class.
	- Texture class should wrap some type of texture handle reference object which can be either a raw/proxy/or shared texture backing object.

- Implement VarDataBuffer which is basically a UniformBuffer object for storing global shader var values, all of the graphics (dx, opengl, vulkan) apis have an equivalent for this concept.

- Overhaul the ShaderVar/VariableBlock system



====== [v2.0 REFACTOR] ======
SEE: https://www.youtube.com/watch?v=qx1c190aGhs

- Move render engine crap into RHI implementations
	RHI = RenderHardwareImplementation

- Frames collected into RHICommandList instances that essentially contain which shaders to bind and which verticies to draw for it
	(But its more complex, review the video linked)

