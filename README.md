# PassGen v1.1

## ⚡ Logs

### version 1.1

- **QR-Code Generation**: Easily generate a QR code for your password checks.
- **Clipboard Integration**: Automatically copy generated passwords to your clipboard for convenience.

### version 1.0

- **Not added 🤐**

## 🛠️ Installation Instructions
### Linux 🐧

|    Name    |                              Icon                               |          Command          |
|:----------:|:---------------------------------------------------------------:|:-------------------------:|
|   ubuntu   |   ![Ubuntu](https://skillicons.dev/icons?i=ubuntu&theme=dark)   | sudo apt-get install xsel |
|   debian   |   ![Debian](https://skillicons.dev/icons?i=debian&theme=dark)   | sudo apt-get install xsel |
|    mint    |     ![Mint](https://skillicons.dev/icons?i=mint&theme=dark)     | sudo apt-get install xsel |
| Arch Linux |  ![Arch Linux](https://skillicons.dev/icons?i=arch&theme=dark)  |    sudo pacman -S xsel    |

```bash
git clone https://github.com/Artpupser/PassGen.git

cd ./PassGen/PassGen/

dotnet restore
dotnet publish -c Release -r linux-x64 --self-contained   

./bin/Release/net9.0/linux-x64/publish/PassGen   
```
### Windows 🖥️
```bash
git clone https://github.com/Artpupser/PassGen.git

cd ./PassGen/PassGen/

dotnet restore
dotnet publish -c Release -r win-x64 --self-contained

./bin/Release/net9.0/win-x64/publish/PassGen.exe
```

## 📦 Dependencies
Ensure you have the following dependencies installed:
- **[QRCoder](https://github.com/codebude/QRCoder)**: For generating QR codes.
- **[TextCopy](https://github.com/CopyText/TextCopy)**: For clipboard operations.
