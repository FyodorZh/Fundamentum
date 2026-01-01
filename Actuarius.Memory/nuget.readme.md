# Actuarius.Memory

Actuarius.Memory is a lightweight, thread-safe memory rental and pooling library for .NET (netstandard2.0). It provides high-performance reusable pools for byte arrays and generic objects to reduce allocations and GC pressure in high-throughput applications.

## Key features
- Concurrent pools for byte arrays and generic typed arrays
- Separate pools for small and big objects with configurable capacities
- Collectable resource pool for pooled disposable/collectable objects
- Centralized `IMemoryRental` API and a shared `MemoryRental.Shared` singleton
- Array pool sizing strategy tuned by buffer size

## Core API
- `IMemoryRental` — exposes pools: `ByteArraysPool`, `CollectablePool`, `SmallObjectsPool`, `BigObjectsPool`, and `GetArrayPool<T>()`
- `MemoryRental.Shared` — global default rental instance
- `GetArrayPool<T>()` — obtain a typed array pool with size-based capacity policy

## Design goals
- Minimize allocations and GC overhead
- Safe concurrent access from multiple threads
- Simple, composable API that integrates with existing pooling abstractions