# 🔑 PassGen - this is password generator in terminal

![Linux](https://img.shields.io/badge/Linux-FCC624?style=for-the-badge&logo=linux&logoColor=black)
![Bash](https://img.shields.io/badge/bash-%23121011.svg?style=for-the-badge&logo=gnu-bash&logoColor=white)
![Windows Terminal](https://img.shields.io/badge/Windows%20Terminal-%234D4D4D.svg?style=for-the-badge&logo=windows-terminal&logoColor=white)
![C#](https://img.shields.io/badge/CSHARP-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)

- #### PassGen is a simple terminal tool for creating strong passwords.
- #### Just enter a keyword and choose a length, and it generates secure passwords for you.
- #### Perfect for anyone wanting to keep their online accounts safe! 🔒✨

---

## 📎 Navigation

- [⚡ Logs](#logs)
	- [Version 1.2](#version-1.2)
	- [Version 1.1](#version-1.1)
	- [Version 1.0](#version-1.0)
- [🛠️ Installation Instructions](#installation-instructions)
	- [Linux 🐧](#linux1)
	- [Windows 🖥️](#windows1️)
- [📦 Dependencies](#dependencies)

<h2 id="logs">⚡ Logs</h2>

| Версия                          | Изменения                                                                                                                                                                                                                                                                                                                                                                                                                                               |
|---------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| <p id='version-1.2'>**1.2**</p> | - **Added new password core**: **"Beta"** - A more secure password generation method that requires a keyword and length input. It generates complex passwords using a unique algorithm, but needs more data for recovery. <br> - **Regeneration Feature**: Users can recreate passwords using the same keyword and creation date, ensuring easy recovery. <br> - **Changed file readme.md**: Added a **"navigation"** section for better user guidance. |
| <p id='version-1.1'>**1.1**</p> | - **QR-Code Generation**: Easily generate a QR code for your password checks. <br> - **Clipboard Integration**: Automatically copy generated passwords to your clipboard for convenience.                                                                                                                                                                                                                                                               |
| <p id='version-1.0'>**1.0**</p> | - **Start console-app**: Created a console application project. <br> - **Added first password core**: **Alpha** - is first password generation method in this project.                                                                                                                                                                                                                                                                                  |

<h2 id="installation-instructions">🛠️ Installation Instructions</h2>
<h3 id="linux1">Linux 🐧</h3>

|    Name    |                             Icon                              |          Command          |
|:----------:|:-------------------------------------------------------------:|:-------------------------:|
|   ubuntu   |  ![Ubuntu](https://skillicons.dev/icons?i=ubuntu&theme=dark)  | sudo apt-get install xsel |
|   debian   |  ![Debian](https://skillicons.dev/icons?i=debian&theme=dark)  | sudo apt-get install xsel |
|    mint    |    ![Mint](https://skillicons.dev/icons?i=mint&theme=dark)    | sudo apt-get install xsel |
| Arch Linux | ![Arch Linux](https://skillicons.dev/icons?i=arch&theme=dark) |    sudo pacman -S xsel    |

```bash
git clone https://github.com/Artpupser/PassGen.git

cd ./PassGen/PassGen/

dotnet restore
dotnet publish -c Release -r linux-x64 --self-contained

./bin/Release/net9.0/linux-x64/publish/PassGen
```

<h3 id="windows1️">Windows 🖥️</h3>

```bash
git clone https://github.com/Artpupser/PassGen.git

cd ./PassGen/PassGen/

dotnet restore
dotnet publish -c Release -r win-x64 --self-contained

./bin/Release/net9.0/win-x64/publish/PassGen.exe
```

<h2 id="dependencies">📦 Dependencies</h2>

Ensure you have the following dependencies installed:

- **[QRCoder](https://github.com/codebude/QRCoder)**: For generating QR codes.
- **[TextCopy](https://github.com/CopyText/TextCopy)**: For clipboard operations.
