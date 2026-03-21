# DotNetBuildingBlocks.Abstractions.UnitTests

This test project verifies the compile-level contract usage for `DotNetBuildingBlocks.Abstractions`.

## Test scope

The abstractions package is intentionally very small and interface-only.

Because of that, the tests focus on:
- contract implementation usage
- interface composition
- expected property exposure
- minimal behavior that can be verified without fake logic

## Notes

These tests should stay lightweight and should not become artificial.
The goal is confidence, not fake coverage.
