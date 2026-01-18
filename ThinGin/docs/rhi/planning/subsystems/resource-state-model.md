# Resource State Model (Explicit Access and Pipeline)

## Purpose

Define the explicit resource state model used by the RHI for correctness, validation, and backend portability.

## Model overview

- Access is represented as a bitmask of usage states (SRV, UAV, RTV, DSV, Copy, Present, etc.).
- Pipeline scope is explicit (graphics, async compute) and part of transition intent.
- Texture state is tracked per subresource (mip, array slice, plane).
- Transitions are explicit and describe AccessBefore, AccessAfter, and pipeline scopes.
- The RHI validates state usage and hazards; backends may treat some barriers as logical.

## Core concepts

- Access mask
  - Encodes read, write, and read-write modes.
  - Includes specialized states (indirect args, shading rate, BVH, resolve).
- Pipeline mask
  - Identifies which pipeline owns the transition (graphics, async compute).
- Subresource range
  - Mip index, array slice, and plane slice; whole-resource is the default range.
- Transition info
  - Resource reference + subresource range + AccessBefore/AccessAfter + flags.
- Transition object
  - Aggregates one or more transition infos and optional aliasing infos.

## Transition flags and aliasing

- Discard and clear hints for transient or fully overwritten resources.
- Aliasing information for transient resources that reuse memory.
- Optional "no split" or "no fence" hints for optimized barrier placement.

## Validation and tracking

- The RHI maintains per-pipeline tracked access for each resource or subresource.
- Validation rejects invalid access mixes (read-only exclusive with writable, etc.).
- Validation is required regardless of backend, even if a backend uses logical barriers.

## Initial state

- Resource descriptors carry an explicit initial access state.
- If unspecified, the RHI chooses a default based on usage flags.

## Minimal interface sketch (conceptual)

RHIResourceTransition
- Data: src pipelines, dst pipelines, transition infos, aliasing infos, transition flags.
- Role: immutable description of state changes, created by the RHI and consumed by the command context.

IRHICommandContext
- BeginTransitions(transitions)
- EndTransitions(transitions)
- Transition(infos, src pipelines, dst pipelines, flags)
- SetTrackedAccess(infos) for validation-only updates when no barrier is required.

## Backend notes

- OpenGL may implement transitions as validation plus ordering, since native barriers are limited.
- Explicit APIs map access and pipeline masks to native barrier structures.
