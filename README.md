<div align="center">

# 🔑 PassGen

![Dotnet](https://img.shields.io/badge/.NET-black?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-black.svg?style=for-the-badge&logo=csharp&logoColor=white)
![CLI](https://img.shields.io/badge/CLI-black?style=for-the-badge&logo=gnu-bash&logoColor=white)
![Linux](https://img.shields.io/badge/Linux-black?style=for-the-badge&logo=linux&logoColor=white)
![License](https://img.shields.io/badge/MIT-black?style=for-the-badge)
![No AI](https://custom-icon-badges.demolab.com/badge/No%20AI-black?style=for-the-badge&logo=non-ai&logoColor=white)

![.NET](https://img.shields.io/badge/.NET-10.0-blue?style=for-the-badge)
![.Version](https://img.shields.io/github/v/release/Artpupser/PassGen?style=for-the-badge)


#### [PassGen](https://github.com/Artpupser/PassGen) is password generator in terminal 🎯

<img src="https://github.com/Artpupser/PassGen/blob/main/assets/banner.jpg" style="border-radius: 20px; max-height: 500px">

</div>

---
## 📎 Navigation

- [✨ Features](#-features)
- [🧵 Usage](#-usage)
- [👀 Preview](#-usage)
- [🦾️ Installation](#-installation)
    - [Linux 🐧](#linux-)
    - [Windows 🖥️](#windows-)
- [📦 Dependencies](#-dependencies)
- [🗃️ Devlog](#devlog)
- [⚖️️ License](#-license)


## ✨️ Features
| 🏆 Feature                  | 📝 Description                                                                |
| --------------------------- | ----------------------------------------------------------------------------- |
| Secure Password Generation  | Creates strong, random passwords that are hard to guess or crack.             |
| Copy to Clipboard           | Quickly copies the generated password for easy use.                           |
| QrCode generation | Simplify transport password to mobile.                   |
| Cross-Platform Support      | Works smoothly across different operating systems.                            |
| Lightweight                 | Fast startup and minimal resource usage.                                      |
| User-Friendly CLI interface | Simple design that makes password generation easy for everyone.               |

## 🧵 Usage

Use your keyboard to interact with the app

1. Run `/set --list` to view all available models.
2. Run `/gen` to start the generation process with the selected model.
3. Fill in the generation form.
4. Receive your password receipt.

## 👀 Preview

## 🦾 Installation

### Linux 🐧

|                              Unix-like                              |              Command               |
|:-------------------------------------------------------------------:|:---------------------------------:|
|     ![Ubuntu](https://skillicons.dev/icons?i=ubuntu&theme=dark)      |      sudo apt-get install xsel     |
|     ![Debian](https://skillicons.dev/icons?i=debian&theme=dark)      |      sudo apt-get install xsel     |
|      ![Mint](https://skillicons.dev/icons?i=mint&theme=dark)         |      sudo apt-get install xsel     |
|   ![Arch Linux](https://skillicons.dev/icons?i=arch&theme=dark)      |        sudo pacman -S xsel         |
|     ![RedHat](https://skillicons.dev/icons?i=redhat&theme=dark)      |        sudo yum install xsel       |
|       ![Kali](https://skillicons.dev/icons?i=kali&theme=dark)        |      sudo apt-get install xsel     |

```bash
git clone https://github.com/Artpupser/PassGen.git

cd ./PassGen/PassGen/

dotnet restore
dotnet publish -c Release -r linux-x64 --self-contained

./bin/Release/net10.0/linux-x64/publish/PassGen
```

### Windows 🖥️

|                             macOS                             |
|:-------------------------------------------------------------:|
| ![Windows](https://skillicons.dev/icons?i=windows&theme=dark) |


```bash
git clone https://github.com/Artpupser/PassGen.git

cd ./PassGen/PassGen/

dotnet restore
dotnet publish -c Release -r win-x64 --self-contained

./bin/Release/net10.0/win-x64/publish/PassGen.exe
```

### MacOS 💻

|            macOS             |           Command            |
|:---------------------------:|:----------------------------:|
| ![Apple](https://skillicons.dev/icons?i=apple&theme=dark) | brew install xsel |

```bash
git clone https://github.com/Artpupser/PassGen.git

cd ./PassGen/PassGen/

dotnet restore
dotnet publish -c Release -r osx-x64 --self-contained

./bin/Release/net10.0/osx-x64/publish/PassGen
```

## 📦 Dependencies

- [Microsoft.Extensions.Configuration](https://github.com/dotnet/runtime)
- [Microsoft.Extensions.Configuration.EnvironmentVariables](https://github.com/dotnet/runtime)
- [Microsoft.Extensions.Configuration.Json](https://github.com/dotnet/runtime)
- [Microsoft.Extensions.DependencyInjection](https://github.com/dotnet/runtime)
- [PupaLib.Core](https://github.com/Artpupser/PupaLib.Core)
- [PupaLib.FileIO](https://github.com/Artpupser/PupaLib.FileIO)
- [QRCoder](https://github.com/codebude/QRCoder)
- [TextCopy](https://github.com/CopyText/TextCopy)
- [dotenv.net](https://github.com/tonerdo/dotnet-env)
- [Konscious.Security.Cryptography.Argon2](https://github.com/kmaragon/Konscious.Security.Cryptography)

## 🗃️ Devlog

### 2.0.0
- Changed .NET version net8.0 -> net10.0.
- rebuilding rendering system, added: commander, configuration, dependency injection, graphics and console input service.
- User-friendly cli system.
- More information with generation password.
- Qr-code for copy paste password on phone with minimal effort.
- All oldest <alpha, beta> generators supports in new version.

### 1.2.0

- Added new password core: "Beta" - A more secure password generation method that requires a keyword and length input. It generates complex passwords using a unique algorithm, but needs more data for recovery.
- Regeneration Feature: Users can recreate passwords using the same keyword and creation date, ensuring easy recovery.
- Changed file readme.md: Added a "navigation" section for better user guidance.

### 1.1.0
- QR-Code Generation: Easily generate a QR code for your password checks.
- Clipboard Integration: Automatically copy generated passwords to your clipboard for convenience.

### 1.0.0
- Start console-app: Created a console application project.
- Added first password core: Alpha - is first password generation method in this project.

## ⚖️ License

This project is licensed under the MIT License.