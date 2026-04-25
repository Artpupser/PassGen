# 🤝 Contributing to PassGen

Thank you for your interest in contributing to **PassGen** — a secure, cross-platform CLI password generator built with .NET!

Whether you're fixing a bug, proposing a new feature, improving documentation, or adding a new password model — all contributions are welcome.

---

## Table of Contents

- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [Project Structure](#project-structure)
- [How to Contribute](#how-to-contribute)
- [Coding Guidelines](#coding-guidelines)
- [Commit Messages](#commit-messages)
- [Reporting Bugs](#reporting-bugs)
- [Suggesting Features](#suggesting-features)
- [Security Issues](#security-issues)

---

## Getting Started

1. **Fork** the repository on GitHub
2. **Clone** your fork locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/PassGen.git
   cd PassGen
   ```
3. Create a new branch for your changes:
   ```bash
   git checkout -b feature/my-feature
   ```

---

## Development Setup

### Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Any IDE: [Rider](https://www.jetbrains.com/rider/), [Visual Studio](https://visualstudio.microsoft.com/), or [VS Code](https://code.visualstudio.com/)

### Install dependencies

```bash
cd src/PassGen
dotnet restore
```

### Build & Run

```bash
dotnet build
dotnet run
```

### Platform-specific builds

```bash
# Linux
dotnet publish -c Release -r linux-x64 --self-contained

# Windows
dotnet publish -c Release -r win-x64 --self-contained

# macOS
dotnet publish -c Release -r osx-x64 --self-contained
```

---

## Project Structure

```
PassGen/
├── .github/                  # GitHub templates and workflows
├── assets/                   # Images and media for README
├── src/
│   └── PassGen/              # Main application source
│       ├── Models/           # Password generation models (Alpha, Beta, Argon...)
│       ├── Services/         # Core services (CLI, clipboard, QR, config...)
│       └── ...
├── CODE_OF_CONDUCT.md
├── CONTRIBUTING.md
├── LICENSE.md
├── README.md
├── SECURITY.md
└── PassGen.slnx
```

---

## How to Contribute

### 1. Adding a new password model

- Create a new class under `src/PassGen/Models/`
- Implement the shared generation interface used by existing models (Alpha, Beta, Argon)
- Register it with the CLI commander
- Add it to `model --list` output
- Update `README.md` with usage info

### 2. Fixing a bug

- Open an Issue first (if one doesn't exist yet) to describe the bug
- Reference the Issue number in your branch name: `fix/issue-42-clipboard-crash`

### 3. Improving documentation

- Docs live in `README.md`, `RELEASE.md`, and inline XML comments in the source
- Keep language clear and concise; match the existing tone

---

## Coding Guidelines

- Follow standard **C# conventions** (PascalCase for types/methods, camelCase for locals)
- Keep methods small and focused — single responsibility
- Avoid hardcoded strings; use constants or config where applicable
- Do not introduce unnecessary dependencies
- **No AI-generated code** — this project follows a No AI policy (see the badge in README)
- Format your code according to `.editorconfig` settings in the repo root

---

## Commit Messages

Use clear, descriptive commit messages in the imperative mood:

```
feat: add Argon2id model support
fix: resolve clipboard crash on Wayland
docs: update Linux installation steps
refactor: extract password validator to separate service
chore: bump QRCoder to 1.5.0
```

Prefixes: `feat`, `fix`, `docs`, `refactor`, `test`, `chore`, `style`

---

## Reporting Bugs

Please open a [GitHub Issue](https://github.com/Artpupser/PassGen/issues) with:

- Your OS and version
- .NET SDK version (`dotnet --version`)
- Steps to reproduce
- Expected vs actual behavior
- Any relevant error output or screenshots

---

## Suggesting Features

Open a [GitHub Discussion](https://github.com/Artpupser/PassGen/discussions) or Issue tagged `enhancement`. Describe:

- What the feature does
- Why it's useful for PassGen users
- Any implementation ideas you have

---

## Security Issues

**Do not open a public Issue for security vulnerabilities.**
Please refer to [SECURITY.md](./SECURITY.md) for the responsible disclosure process.

---

## Code of Conduct

By participating, you agree to follow our [Code of Conduct](./CODE_OF_CONDUCT.md).

---

Thanks for helping make PassGen better! 🔑
