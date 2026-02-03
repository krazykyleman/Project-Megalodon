# 💰 Tap Tycoon - Unity Clicker Game

A monetizable idle clicker game built with Unity and C#, featuring Unity Ads integration for revenue generation.

## 🎮 Game Features

### Core Gameplay
- **Tap to Earn**: Tap the diamond button to earn coins
- **10 Unique Upgrades**: From Auto Tappers to interdimensional Portals
- **Idle Progression**: Upgrades generate coins automatically
- **Auto-Save**: Game state persists using PlayerPrefs
- **Smooth Animations**: Satisfying tap feedback with custom animations
- **Number Formatting**: Clean display for large numbers (K, M, B, T)

### Upgrades
1. 👆 **Auto Tapper** - Taps automatically (0.1/sec)
2. 👵 **Grandma** - A nice grandma to tap for you (1.0/sec)
3. 🌾 **Farm** - Grows coins naturally (8.0/sec)
4. ⛏️ **Mine** - Extracts precious coins (47.0/sec)
5. 🏭 **Factory** - Mass produces coins (260.0/sec)
6. 🏦 **Bank** - Generates coin interest (1,400/sec)
7. ⛩️ **Temple** - Summons coin spirits (7,800/sec)
8. 🧙 **Wizard Tower** - Creates coins with magic (44,000/sec)
9. 🚀 **Spaceship** - Mines asteroids for coins (260,000/sec)
10. 🌀 **Portal** - Brings coins from other dimensions (1.6M/sec)

### Monetization Features
- **Banner Ads**: Persistent ad at the bottom of the screen
- **Interstitial Ads**: Shows every 50 taps
- **Rewarded Video Ads**: Players watch ads for 500 bonus coins
- **Unity Ads Integration**: Ready for production deployment

## 🚀 Getting Started

### Prerequisites
- Unity 2022.3 LTS or later
- Unity Ads package (included in project)
- TextMeshPro package (included in project)

### Opening the Project

1. Clone the repository
2. Open Unity Hub
3. Click "Add" and select the project folder
4. Open the project with Unity 2022.3 LTS or later
5. Wait for Unity to import all packages

### Project Structure

```
Assets/
├── Scripts/
│   ├── GameManager.cs         # Core game logic and state
│   ├── Upgrade.cs              # Upgrade data class
│   ├── SaveSystem.cs           # Save/load functionality
│   ├── AdManager.cs            # Unity Ads integration
│   ├── UIManager.cs            # UI controller
│   └── UpgradeButton.cs        # Individual upgrade UI
└── Scenes/
    └── MainScene.unity         # Main game scene (to be created)
```

## 🎨 Setting Up the Scene

### Creating the UI

Since Unity scenes can't be created via text files, you'll need to set up the UI in the Unity Editor:

1. **Create Main Canvas**
   - Right-click in Hierarchy → UI → Canvas
   - Set Canvas Scaler to "Scale With Screen Size"
   - Reference Resolution: 1080x1920

2. **Add Coin Display (Top)**
   - Create Panel for header
   - Add TextMeshProUGUI for coins (large, bold)
   - Add TextMeshProUGUI for coins/sec

3. **Add Tap Button (Center)**
   - Create Button
   - Set size to 400x400
   - Add TextMeshProUGUI with "💎\nTap!" text
   - Assign to UIManager's tapButton field

4. **Add Watch Ad Button**
   - Create Button below tap button
   - Add text: "Watch Ad +500 Coins!"
   - Assign to UIManager's watchAdButton field

5. **Add Upgrades Scroll View (Bottom Half)**
   - Create Scroll View
   - Set Content to use Vertical Layout Group
   - Create upgrade button prefab:
     - Panel with Image background
     - TextMeshProUGUI for name (with icon)
     - TextMeshProUGUI for description
     - TextMeshProUGUI for cost
     - TextMeshProUGUI for owned count
     - TextMeshProUGUI for production
     - Button component
     - UpgradeButton script
   - Assign prefab to UIManager's upgradeItemPrefab field

6. **Create GameObjects**
   - Create empty GameObject named "GameManager"
   - Add GameManager script
   - Create empty GameObject named "AdManager"
   - Add AdManager script

7. **Assign References**
   - Select Canvas and add UIManager script
   - Assign all UI references in the inspector

## 💰 Monetization Setup

### Setting Up Unity Ads

1. **Create Unity Gaming Services Account**
   - Go to https://dashboard.unity3d.com
   - Create a new project or link existing one
   - Enable Unity Ads in the Services tab

2. **Get Your Game IDs**
   - In Unity Dashboard, go to Monetization
   - Copy your Android Game ID
   - Copy your iOS Game ID (if publishing to iOS)

3. **Configure AdManager**
   - Open AdManager.cs in Unity Inspector
   - Paste your Android Game ID in `androidGameId` field
   - Paste your iOS Game ID in `iosGameId` field
   - Set `testMode` to `true` for testing
   - **Important**: Set `testMode` to `false` before publishing

4. **Ad Unit IDs**
   The default ad unit IDs work for testing:
   - Banner: `Banner_Android` / `Banner_iOS`
   - Interstitial: `Interstitial_Android` / `Interstitial_iOS`
   - Rewarded: `Rewarded_Android` / `Rewarded_iOS`

   For production, create custom ad units in Unity Dashboard and update these IDs.

### Monetization Strategy

**Current Implementation:**
- Banner ad displays constantly at bottom (low eCPM, steady income)
- Interstitial ad shows every 50 taps (medium eCPM, frequent)
- Rewarded video ad offers 500 coins (high eCPM, optional)

**Revenue Estimates:**
- 1,000 Daily Active Users (DAU): $5-15/day
- 10,000 DAU: $50-150/day
- 100,000 DAU: $500-1,500/day

*Actual revenue varies by location, engagement, and ad fill rates.*

## 🔧 Building the Game

### Build for Android

1. Go to File → Build Settings
2. Select Android platform
3. Click "Switch Platform"
4. Go to Player Settings
5. Set:
   - Company Name: Your company
   - Product Name: Tap Tycoon
   - Bundle Identifier: com.yourcompany.taptycoon
   - Minimum API Level: 22 (Android 5.1)
   - Target API Level: 33 or higher
6. Click "Build" and select output folder

### Build for iOS

1. Go to File → Build Settings
2. Select iOS platform
3. Click "Switch Platform"
4. Set Bundle Identifier in Player Settings
5. Build and open in Xcode
6. Configure signing in Xcode
7. Build from Xcode

## 📱 Publishing

### Google Play Store

1. **Prepare Store Listing**
   - Title: "Tap Tycoon - Idle Clicker"
   - Short description: "Tap to earn coins, buy upgrades, become a tycoon!"
   - Category: Casual / Simulation
   - Content Rating: Everyone

2. **Required Assets**
   - App icon (512x512 PNG)
   - Feature graphic (1024x500)
   - Screenshots (at least 2)
   - Privacy policy URL

3. **Upload APK/AAB**
   - Create signed release build
   - Upload to Google Play Console
   - Complete store listing
   - Submit for review

### Apple App Store

1. Create app in App Store Connect
2. Upload build via Xcode
3. Complete app information
4. Add screenshots and preview video
5. Submit for review

## 🎯 Future Enhancements

### Gameplay Features
- **Prestige System**: Reset progress for permanent multipliers
- **Achievements**: Unlock rewards for milestones
- **Daily Rewards**: Login bonuses to boost retention
- **Special Events**: Limited-time upgrades and bonuses
- **Leaderboards**: Compete with other players
- **Themes**: Visual customization options

### Monetization Enhancements
- **In-App Purchases**:
  - Remove Ads: $2.99
  - Coin Packs: $0.99 - $9.99
  - 2x Earnings: $4.99
- **VIP Subscription**: $4.99/month
  - Double coin production
  - Exclusive upgrades
  - No interstitial ads
  - Special VIP badge
- **Offer Wall Integration**: Additional ad revenue source

### Technical Improvements
- **Cloud Save**: Save progress across devices
- **Analytics**: Track user behavior and optimize
- **A/B Testing**: Test different features and monetization
- **Push Notifications**: Re-engage inactive players
- **Crash Reporting**: Improve stability

## 🐛 Troubleshooting

### Unity Ads Not Showing

**Problem**: Ads don't display during testing

**Solutions**:
- Ensure testMode is enabled in AdManager
- Check internet connection
- Wait a few minutes after first initialization
- Verify Game ID is correct
- Check Unity Ads is enabled in Services window

### Game Not Saving

**Problem**: Progress is lost when closing the game

**Solutions**:
- Check PlayerPrefs data is not being cleared
- Verify SaveSystem.SaveGame() is being called
- Test on device (not just in editor)
- Check for errors in console

### Build Errors

**Problem**: Can't build for Android/iOS

**Solutions**:
- Ensure correct Unity version (2022.3 LTS recommended)
- Update Android/iOS build support modules
- Check minimum API levels are set correctly
- Verify TextMeshPro and Unity Ads packages are imported

## 📝 Code Architecture

### GameManager.cs
- Singleton pattern for global access
- Manages game state (coins, upgrades, CPS)
- Handles tap logic and upgrade purchases
- Triggers auto-save every 30 seconds

### SaveSystem.cs
- JSON serialization using Unity's JsonUtility
- Stores data in PlayerPrefs
- Saves: coins, total taps, upgrade ownership

### AdManager.cs
- Singleton for ad management
- Implements Unity Ads interfaces
- Handles ad loading, showing, and callbacks
- Manages reward distribution

### UIManager.cs
- Updates UI based on game state
- Creates upgrade buttons dynamically
- Handles user input
- Provides visual feedback

### Upgrade.cs
- Data class for upgrade properties
- Calculates cost with exponential scaling (1.15x multiplier)
- Formats numbers for display

## 📄 License

This project is provided as-is for educational and commercial use.

## 🤝 Support

For issues or questions, open an issue on GitHub.

---

**Start tapping and build your empire!** 💎💰

## Tips for Success

1. **Test Extensively**: Test on multiple devices and OS versions
2. **Optimize Performance**: Profile and optimize for 60 FPS
3. **Collect Feedback**: Listen to player reviews and iterate
4. **Update Regularly**: Add new content to keep players engaged
5. **Market Wisely**: Use ASO (App Store Optimization) techniques
6. **Monitor Analytics**: Track KPIs like retention, ARPDAU, and LTV
