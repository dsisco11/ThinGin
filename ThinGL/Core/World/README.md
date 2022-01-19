

- EngineObjectBase: low level engine related stuff for objects, contains the stuff that should not be messed with by end users
- EngineObject: base class for all things that can be and have other things attached to them as children, this forms the basis of all things in the engine.
	- Also has a globally unique ID number


- WorldComponent: world-space object which exists within the world and has a physical transform/location but nothing else, including anything related to rendering or physics.
