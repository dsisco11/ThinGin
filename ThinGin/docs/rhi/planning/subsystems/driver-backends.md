# Driver Backends

## Scope

Backend drivers implement `IRHIDriver` for each native API. They translate RHI concepts into API-specific commands and own device and context lifetime.

## Responsibilities

- Device and context creation, teardown, and capability reporting.
- Native resource creation and destruction.
- Submission of command lists and management of queues or contexts.
- Translation of pipeline state and descriptors to backend bindings.
- Validation and debug markers where the API allows.

## Architectural decisions

- One driver instance per device.
- Backends own native handles and resource tables; RHI resources are thin handles.
- The RHI defines logical resource states; backends map them to API semantics.
- The initial backend target is OpenGL, with room for Vulkan or D3D12.
- The API exposes graphics and async-compute contexts; backends without async compute map both to one context and report capability flags.
- Backends own device/context lifecycle and provide per-thread contexts.
- Backend debug layers integrate with the layered validation policy.
- Backends report structured capabilities and limits to gate optional features.

## Deliverables

- OpenGL driver that supports buffers, textures, shaders, and drawing.
- Capability reporting and feature flags.
- A backend interface that does not leak API-specific types into the RHI frontend.
