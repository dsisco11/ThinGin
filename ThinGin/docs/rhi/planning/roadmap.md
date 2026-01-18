# Roadmap

This roadmap is milestone-based and architecture focused. Each phase should end with a stable RHI boundary and a working vertical slice.

## Phase 0: Architecture alignment

- Decide the definitive RHI boundary and command model.
- Confirm resource descriptor and state model.
- Choose the initial backend (OpenGL) and define driver responsibilities.
- Confirm clean-slate replacement with no compatibility adapters.

## Phase 1: Minimal RHI vertical slice

- Backend: OpenGL driver that can create buffers, shaders, and issue a draw.
- Command model: record and replay command list with explicit submission (single-threaded).
- Resources: buffers, minimal textures, and basic samplers.
- Outcome: draw a simple triangle through the RHI.

## Phase 2: Textured mesh and basic pipeline state

- Graphics pipeline state object or equivalent state bundle.
- Shader reflection and binding layout for textures and samplers.
- Resource views and basic descriptor binding.
- Outcome: draw a textured mesh through the RHI.

## Phase 3: Render targets and depth

- Render target and depth resources, clear, load, and store semantics.
- Framebuffer or render pass abstraction with backend translation.
- Presentation path with swapchain or viewport scheduling.
- Outcome: render to offscreen and present reliably.

## Phase 4: Synchronization and resource states

- Logical resource state model and transition commands.
- GPU and CPU sync primitives with validation hooks.
- Async uploads via staging or DMA concepts.
- Outcome: safe multi-pass rendering with explicit barriers.

## Phase 5: Full RHI feature set

- Shader libraries, pipeline cache, and stable binding model.
- Descriptor allocation strategy for scale.
- Multi-threaded command recording and optional RHI thread integration.
- Additional backends beyond OpenGL.

## Continuous quality goals

- Validation, debug markers, and error reporting at all phases.
- Clear capability reporting and feature gating.
- Documentation kept in sync with architectural decisions.
