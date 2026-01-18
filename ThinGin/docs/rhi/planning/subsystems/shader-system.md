# Shader System

## Scope

The shader system covers compilation, reflection, caching, and libraries.

## Responsibilities

- Define supported shader source languages and profiles.
- Provide reflection data for resources and inputs.
- Build a stable library and cache keyed by hashes.
- Integrate shader metadata with pipeline state creation.

## Architectural decisions

- Choose a canonical source language for authoring.
- Reflection is mandatory for binding validation and descriptor layout.
- Shader libraries are versioned and backend-aware.

## Deliverables

- Shader compilation pipeline specification.
- Reflection schema and binding metadata.
- Library and cache strategy aligned with the pipeline cache.
