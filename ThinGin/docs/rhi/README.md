# ThinGin RHI architecture analysis

These documents summarize the current state of ThinGin's Render Hardware Interface (RHI) effort, with a focus on building a C#-native, driver-centric RHI architecture.

## Scope

- **Included:** Code under `ThinGin/Core/RenderHardware` and adjacent rendering-facing code that interacts with it.
- **Also covered:** The existing OpenGL backend (`ThinGin.OpenGL`) and how it relates to (or diverges from) the newer RHI work.
- **Not included:** Non-rendering subsystems (console, world, math, etc.), except where they are directly referenced by the rendering/RHI layer.

## Documents

- [Implementation Status](implementation-status.md)
  - What exists today: abstractions, resource types, command-list scaffolding, partial OpenGL RHI resource classes.
  - Where the architecture is currently split (legacy engine/provider system vs newer RHI core).
  - Notable incomplete areas detected in code.

- [Gaps and Roadmap](gaps-and-roadmap.md)
  - What remains to implement to reach the target driver-centric RHI.
  - Recommended sequencing/milestones to reduce rework.
  - Concrete "next 2-4 weeks" tasks vs longer-term items.

- [Planning Hierarchy](planning/README.md)
  - Architecture-first plan and subsystem breakdown.

## Quick take

- There is a **new RHI core surface area** in `ThinGin/Core/RenderHardware` that already defines many modern RHI concepts (resource types, pipeline init structs, command list types, fences, etc.).
- There is also a **legacy (older) engine/provider model** in `ThinGin.OpenGL/Common` that binds OpenGL directly and powers the existing examples.
- The new RHI is **not yet fully wired into the runtime**, and many pieces are currently **stubs / placeholders**.
