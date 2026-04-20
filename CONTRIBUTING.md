# Contributing

Thank you for your interest in contributing to Here.Sdk.Premium.Common!

## Workflow

1. Fork the repository and clone it locally.
2. Create a branch from `develop`: `feat/<slug>`, `fix/<slug>`, `chore/<slug>`, etc.
3. Follow the [OpenSpec workflow](openspec/README.md) for non-trivial API changes.
4. Run `./build/verify.sh` before opening a PR.
5. Commit using [Conventional Commits](https://www.conventionalcommits.org/).
6. Open a PR targeting `develop`.

## Code Standards

- Clean Code: functions ≤ 20 lines, ≤ 3 parameters, single purpose
- Clean Architecture: zero runtime dependencies in `src/`
- Coverage: ≥ 90% line + branch per file; 100% on new public members
- English (en-US) only in identifiers, docs, comments
- XML `<summary>` on every public type and member

## Tests

```sh
dotnet test tests/Here.Sdk.Premium.Common.UnitTests
```

## License

By contributing you agree that your contribution will be licensed under the MIT License.
