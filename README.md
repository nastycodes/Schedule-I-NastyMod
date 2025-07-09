<div align="center">

# NastyMod v2
### *Schedule I All-In-One Mod Menu*

[![Version](https://img.shields.io/badge/Version-2.0.0-brightgreen?style=for-the-badge&logo=github)](https://github.com/nasty-codes/nastymod-v2)
[![Game](https://img.shields.io/badge/Game-Schedule%20I-blue?style=for-the-badge&logo=steam)](https://store.steampowered.com/app/3164500/Schedule_I/)
[![Framework](https://img.shields.io/badge/Framework-MelonLoader-orange?style=for-the-badge&logo=unity)](https://melonwiki.xyz)
[![Platform](https://img.shields.io/badge/Platform-Windows-lightblue?style=for-the-badge&logo=windows)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/License-Custom-red?style=for-the-badge)](LICENSE)

---

</div>

## 🚀 Overview

**NastyMod v2** is an all-in-one mod menu for the game **Schedule I** that provides extensive customization and enhancement options. Built using MelonLoader, this mod offers a wide range of features including player modifications, world manipulation, item spawning, teleportation, employee management, and various quality-of-life improvements.

<div align="center">

### 🎯 Key Features

<table>
<tr>
<td align="center" width="33%">
<strong>🛡️ Player Enhancements</strong><br>
<sub>Infinite health/stamina, speed modifications, noclip, and financial management</sub>
</td>
<td align="center" width="33%">
<strong>🌍 World Modifications</strong><br>
<sub>Time control, ESP systems, and environmental tweaks</sub>
</td>
<td align="center" width="33%">
<strong>📦 Item Spawning</strong><br>
<sub>Comprehensive item spawner with filtering and quality selection</sub>
</td>
</tr>
<tr>
<td align="center" width="33%">
<strong>🎯 Teleportation System</strong><br>
<sub>Quick travel to various locations</sub>
</td>
<td align="center" width="33%">
<strong>👥 Employee Management</strong><br>
<sub>Control and optimize your workforce</sub>
</td>
<td align="center" width="33%">
<strong>⚙️ Miscellaneous Tools</strong><br>
<sub>Various game mechanics improvements</sub>
</td>
</tr>
</table>

</div>

<div align="center">

---

## 📥 Installation Guide

</div>

### 📋 Prerequisites

<div align="center">

| Requirement | Status | Download |
|-------------|--------|----------|
| 🎮 **Schedule I** | Required | [![Steam](https://img.shields.io/badge/Steam-000000?style=flat&logo=steam&logoColor=white)](https://store.steampowered.com/app/3164500/Schedule_I/) |
| 🔧 **MelonLoader** | Required | [![GitHub](https://img.shields.io/badge/GitHub-100000?style=flat&logo=github&logoColor=white)](https://github.com/LavaGang/MelonLoader) |
| 💻 **Visual Studio** | Optional | [![Microsoft](https://img.shields.io/badge/Microsoft-0078D4?style=flat&logo=microsoft&logoColor=white)](https://visualstudio.microsoft.com) |

</div>

### 🚀 Installation Steps

<details>
<summary><strong>🎯 Option 1: Binary Installation (Recommended)</strong></summary>

```bash
# Step-by-step installation
1️⃣ Download the latest release from the releases page
2️⃣ Extract the NastyMod v2.dll file
3️⃣ Navigate to your Schedule I installation directory
4️⃣ Go to the Mods folder (created by MelonLoader)
5️⃣ Copy NastyMod v2.dll into the Mods folder
6️⃣ Launch Schedule I
```

</details>

<details>
<summary><strong>🛠️ Option 2: Build from Source</strong></summary>

```csharp
// Build requirements
1️⃣ Clone or download this repository
2️⃣ Open NastyMod v2.sln in Visual Studio
3️⃣ Ensure all dependencies are properly referenced:
   - MelonLoader.dll
   - Assembly-CSharp.dll (from Schedule I)
   - Il2Cpp assemblies (from Schedule I)
4️⃣ Build the project in Release mode (x64)
5️⃣ Copy the generated NastyMod v2.dll from bin\x64\Release\ to your Schedule I Mods folder
```

</details>

### ✅ Verification

<div align="center">

> 🎮 **Launch Schedule I**  
> 👀 **Look for MelonLoader console output confirming "NastyMod v2" has loaded**  
> ⌨️ **Press `F11` or `Right Alt` in-game to open the mod menu**

</div>

<div align="center">

---

## 🎮 Usage Guide

</div>

### 🔑 Opening the Menu

<div align="center">

| Hotkey | Function |
|--------|----------|
| `F11` | 🎯 Primary Menu Toggle |
| `Right Alt` | 🔄 Alternative Toggle |

</div>

### 🧭 Menu Navigation

<div align="center">

**The mod menu features six main tabs:**

| Tab | Icon | Description |
|-----|------|-------------|
| **PLAYER** | 👤 | Character modifications |
| **WORLD** | 🌍 | World and environment settings |
| **SPAWNER** | 📦 | Item spawning tools |
| **MISC** | ⚙️ | Miscellaneous features |
| **TELEPORT** | 🎯 | Location teleportation |
| **EMPLOYEES** | 👥 | Workforce management |

</div>

<div align="center">

---

## 📖 Feature Documentation

</div>

### 👤 1. Player Tab

#### 🛡️ Health & Stamina
<table>
<tr>
<td><strong>🔥 Infinite Health</strong></td>
<td>Makes player invulnerable to damage</td>
</tr>
<tr>
<td><strong>⚡ Infinite Stamina</strong></td>
<td>Removes stamina consumption</td>
</tr>
<tr>
<td><strong>👮 Never Wanted</strong></td>
<td>Prevents police attention</td>
</tr>
</table>

#### 🏃 Movement Enhancements
<table>
<tr>
<td><strong>👻 NoClip</strong></td>
<td>Walk through walls and objects</td>
</tr>
<tr>
<td><strong>🏃 Move Speed Multiplier</strong></td>
<td>Adjust walking speed (default: 1.0x)</td>
</tr>
<tr>
<td><strong>🐌 Crouch Speed Multiplier</strong></td>
<td>Adjust crouching speed (default: 0.6x)</td>
</tr>
<tr>
<td><strong>🦘 Jump Multiplier</strong></td>
<td>Modify jump height/power (default: 1.0x)</td>
</tr>
</table>

#### 💰 Financial Management
<table>
<tr>
<td><strong>⭐ Add Experience</strong></td>
<td>Gain XP instantly (default: 1000)</td>
</tr>
<tr>
<td><strong>💵 Add Cash</strong></td>
<td>Add cash to inventory (default: $1000)</td>
</tr>
<tr>
<td><strong>🏦 Add Bank Balance</strong></td>
<td>Add money to bank account (default: $1000)</td>
</tr>
</table>

### 🌍 2. World Tab

#### 👁️ ESP (Wallhack) Systems
<table>
<tr>
<td><strong>🧑 NPC ESP</strong></td>
<td>See NPCs through walls</td>
</tr>
<tr>
<td><strong>👥 Player ESP</strong></td>
<td>See other players through walls (multiplayer)</td>
</tr>
<tr>
<td><strong>📏 ESP Range</strong></td>
<td>Adjust visibility range for ESP features</td>
</tr>
</table>

#### ⏰ Time Control
<table>
<tr>
<td><strong>⚡ Time Scale</strong></td>
<td>Speed up or slow down game time</td>
</tr>
<tr>
<td><strong>🕐 Set Specific Time</strong></td>
<td>Jump to any time of day</td>
</tr>
<tr>
<td><strong>⏸️ Freeze Time</strong></td>
<td>Stop time progression</td>
</tr>
</table>

### 📦 3. Spawner Tab

#### 🎁 Item Spawning
<table>
<tr>
<td><strong>📂 Category Filter</strong></td>
<td>Browse items by category</td>
</tr>
<tr>
<td><strong>⭐ Quality Selection</strong></td>
<td>Choose item quality/condition</td>
</tr>
<tr>
<td><strong>📊 Stack Size</strong></td>
<td>Set quantity for spawned items</td>
</tr>
<tr>
<td><strong>🔍 Search Filter</strong></td>
<td>Find specific items quickly</td>
</tr>
</table>

#### 📋 Supported Categories
- 💊 Drugs and substances
- 🔧 Equipment and tools
- 📦 Containers and storage
- 🧪 Raw materials
- ✨ Processed goods

### ⚙️ 4. Miscellaneous Tab

#### 📊 Stack Management
<table>
<tr>
<td><strong>📈 Use Custom Stack Size</strong></td>
<td>Override default stack limits</td>
</tr>
<tr>
<td><strong>🔢 Stack Size Value</strong></td>
<td>Set custom stack amounts</td>
</tr>
</table>

#### 💼 Business Operations
<table>
<tr>
<td><strong>🎯 Deal Success Chance</strong></td>
<td>Modify drug deal success rates</td>
</tr>
<tr>
<td><strong>⚡ Instant Dead Drops</strong></td>
<td>Complete dead drops immediately</td>
</tr>
<tr>
<td><strong>💰 Instant Laundering</strong></td>
<td>Speed up money laundering</td>
</tr>
<tr>
<td><strong>🧪 Instant Mixing</strong></td>
<td>Accelerate drug mixing processes</td>
</tr>
</table>

#### 🔧 Equipment Upgrades
<table>
<tr>
<td><strong>🗑️ Trash Grabber Capacity</strong></td>
<td>Increase collection capacity</td>
</tr>
<tr>
<td><strong>🌱 Plant Growth Speed</strong></td>
<td>Accelerate plant growing</td>
</tr>
</table>

### 🎯 5. Teleport Tab

#### 🏠 Location Categories
<table>
<tr>
<td><strong>🏘️ Safe Houses</strong></td>
<td>Player properties and hideouts</td>
</tr>
<tr>
<td><strong>🏭 Business Locations</strong></td>
<td>Drug labs, shops, and operations</td>
</tr>
<tr>
<td><strong>🌆 Public Places</strong></td>
<td>Streets, parks, and common areas</td>
</tr>
<tr>
<td><strong>🗺️ Special Locations</strong></td>
<td>Hidden or unique spots</td>
</tr>
</table>

#### 🚀 Quick Travel
- 🔍 Filter locations by name
- ⚡ Instant teleportation
- ⭐ Favorite location saving

### 👥 6. Employees Tab

#### 👔 Workforce Management
<table>
<tr>
<td><strong>🏠 Property Selection</strong></td>
<td>Choose which property to manage</td>
</tr>
<tr>
<td><strong>👥 Employee Types</strong></td>
<td>Botanist, Cleaner, Packager, Chemist</td>
</tr>
<tr>
<td><strong>⚡ Speed Modifications</strong></td>
<td>Adjust work completion times</td>
</tr>
<tr>
<td><strong>🔍 Employee Filtering</strong></td>
<td>Find specific workers</td>
</tr>
</table>

#### 📈 Productivity Enhancements
- 🌱 Soil Pour Time adjustment
- 💧 Water Pour Time modification  
- 🧪 Additive Pour Time control
- 🌿 Seed Sowing Speed
- 🌾 Harvest Time optimization

<div align="center">

---

## ⌨️ Controls & Hotkeys

</div>

### 🖱️ Menu Controls

<div align="center">

| Action | Input | Description |
|--------|-------|-------------|
| 🔄 **Toggle Menu** | `F11` or `Right Alt` | Show/hide mod interface |
| 🖱️ **Navigate** | `Mouse` | Interact with buttons and menus |
| 📌 **Move Window** | `Left Click & Drag` | Reposition menu window |
| 📏 **Resize** | `Drag Corners` | Adjust menu dimensions |

</div>

### 🎮 In-Game Features

<div align="center">

> ✨ **Most features are toggle-based and work automatically once enabled**  
> 💾 **Settings are saved between game sessions**  
> 🎛️ **Real-time adjustment sliders for numeric values**

</div>

<div align="center">

---

## ⚙️ Configuration

</div>

### 💾 Settings Persistence

The mod automatically saves your preferences to `Properties\Settings.settings`:

- 📐 Menu position and size
- 🔘 Feature toggle states  
- 🔢 Numeric values (speeds, amounts, etc.)
- 📂 Selected categories and filters

### 🎨 Customization Options

<table align="center">
<tr>
<td><strong>📏 Menu Width/Height</strong></td>
<td>Adjustable interface size</td>
</tr>
<tr>
<td><strong>📋 Tab Layout</strong></td>
<td>Organized feature grouping</td>
</tr>
<tr>
<td><strong>🎨 Color Scheme</strong></td>
<td>Professional dark theme with blue accents</td>
</tr>
</table>

<div align="center">

---

## 🔧 Technical Information

</div>

### 📦 Dependencies

<div align="center">

| Component | Purpose | Version |
|-----------|---------|---------|
| 🔧 **MelonLoader** | Mod loading framework | Latest |
| 🔨 **Harmony** | Runtime method patching | 2.x+ |
| 🔗 **Il2Cpp Interop** | Game integration layer | Latest |
| 🎮 **Schedule I** | Target game assemblies | Current |

</div>

### 🖥️ Compatibility

<div align="center">

| Specification | Requirement |
|---------------|-------------|
| 🎮 **Game Version** | Schedule I (latest) |
| 💻 **Platform** | Windows (x64) |
| ⚙️ **Framework** | .NET Framework 4.7.2+ |
| 🔧 **MelonLoader** | 0.6.1+ |

</div>

### ⚡ Performance Impact

<div align="center">

> 🟢 **Minimal CPU usage during normal gameplay**  
> 🟡 **ESP features may impact framerate when many objects are visible**  
> 🔵 **Menu rendering only active when interface is open**

</div>

<div align="center">

---

## ⚠️ Safety & Legal Notice

</div>

### 🛡️ Use Responsibly

<div align="center">

| ⚠️ Important Notice |
|---------------------|
| 🎮 This mod is intended for **single-player use only** |
| 🚫 Using mods in multiplayer may result in server bans |
| 💾 Always backup your save files before using mods |

</div>

### ⚖️ Legal Disclaimer

<div align="center">

> 📜 **This software is provided "as-is" without warranty**  
> ⚠️ **Use at your own risk**  
> 🏢 **Not affiliated with Schedule I developers**  
> 🔍 **Reverse engineering for personal use only**

</div>

<div align="center">

---

## 🔧 Troubleshooting

</div>

### 🚨 Common Issues

<details>
<summary><strong>❌ Mod Not Loading</strong></summary>

```bash
# Troubleshooting Steps:
✅ Verify MelonLoader is properly installed
🔍 Check console for error messages  
📦 Ensure all dependencies are present
📁 Confirm mod file is in correct Mods folder
```

</details>

<details>
<summary><strong>🔍 Menu Not Appearing</strong></summary>

```bash
# Solutions:
⌨️ Try both hotkeys (F11 and Right Alt)
📊 Check if mod loaded successfully in console
🔄 Restart game if needed
```

</details>

<details>
<summary><strong>⚡ Performance Issues</strong></summary>

```bash
# Optimization:
👁️ Disable ESP features if experiencing lag
📏 Reduce ESP range value
❌ Close menu when not in use
```

</details>

<details>
<summary><strong>💾 Save Game Corruption</strong></summary>

```bash
# Prevention:
📁 Always backup saves before modding
🚫 Disable mods before major game updates
⚠️ Some features may affect save file integrity
```

</details>

### 🆘 Getting Help

<div align="center">

| Issue Type | Solution |
|------------|----------|
| 🐛 **Bug Reports** | Check console output for specific error messages |
| 🔄 **Compatibility** | Verify game and mod versions are compatible |
| 📝 **Feature Requests** | Report with detailed reproduction steps |

</div>

<div align="center">

---

## 🏆 Credits & Attribution

</div>

### 🔧 Third-Party Libraries

<div align="center">

| Library | Purpose | License |
|---------|---------|---------|
| 🔧 **MelonLoader** | Mod framework | MIT |
| 🔨 **Harmony** | Runtime patching | MIT |
| 🔗 **Il2Cpp Interop** | Game integration | MIT |

</div>

### 🙏 Special Thanks

<div align="center">

> 🎮 **Schedule I development team** - For creating an amazing game  
> 💬 **Discord community** - For feedback and suggestions  
> 🧪 **Beta testers and contributors** - For quality assurance

</div>

<div align="center">

---

## 📅 Version History

</div>

### 🚀 v2.0.0 (Current)

<div align="center">

**✨ Major Release - Complete Overhaul ✨**

</div>

<table>
<tr>
<td><strong>🎨 Complete UI Rewrite</strong></td>
<td>Modern, responsive interface design</td>
</tr>
<tr>
<td><strong>👥 Employee Management</strong></td>
<td>Advanced workforce control system</td>
</tr>
<tr>
<td><strong>🎯 Enhanced Teleportation</strong></td>
<td>Improved location system with categories</td>
</tr>
<tr>
<td><strong>⚡ Performance Boost</strong></td>
<td>Optimized code and reduced overhead</td>
</tr>
<tr>
<td><strong>💾 Better Persistence</strong></td>
<td>Improved settings save/load system</td>
</tr>
</table>

### 📜 Previous Versions

<details>
<summary><strong>📋 Legacy Releases</strong></summary>

- **v1.x**: Initial release series
- **Features**: Legacy feature set
- **Status**: Basic mod functionality

</details>

<div align="center">

---

## 🌟 Support the Project

</div>

<div align="center">

[![GitHub Stars](https://img.shields.io/github/stars/nasty-codes/nastymod-v2?style=for-the-badge&logo=github)](https://github.com/nasty-codes-software/Schedule-I-NastyMod)
[![GitHub Forks](https://img.shields.io/github/forks/nasty-codes/nastymod-v2?style=for-the-badge&logo=github)](https://github.com/nasty-codes-software/Schedule-I-NastyMod)
[![GitHub Issues](https://img.shields.io/github/issues/nasty-codes/nastymod-v2?style=for-the-badge&logo=github)](https://github.com/nasty-codes-software/Schedule-I-NastyMod/issues)

**⭐ If you enjoy this mod, please consider giving it a star!**

[🐛 Report Bug](https://github.com/nasty-codes-software/Schedule-I-NastyMod/issues) •
[💡 Request Feature](https://github.com/nasty-codes-software/Schedule-I-NastyMod/issues) •
[💬 Join Discord](https://discord.gg/CfZXMqtfvv)

---

*This README covers the complete feature set of NastyMod v2. For technical support or feature requests, visit the project repository or contact the development team.*

**Made with ❤️ by nasty.codes Software**

</div>
