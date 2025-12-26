# 💰 Tap Tycoon - Android Clicker Game

A modern, monetizable clicker game for Android built with Kotlin and Jetpack Compose.

## Features

### Game Mechanics
- **Tap to Earn**: Tap the golden diamond to earn coins
- **10+ Upgrades**: From Auto Tappers to Portals, each producing coins automatically
- **Progressive Difficulty**: Upgrade costs increase exponentially
- **Auto-Save**: Game progress is automatically saved using DataStore
- **Smooth Animations**: Spring-based animations for satisfying tap feedback

### Monetization Features
- **Banner Ads**: Persistent banner ad at the bottom of the screen
- **Interstitial Ads**: Full-screen ads shown every 50 taps
- **Rewarded Ads**: Players can watch ads to earn 500 bonus coins
- **Google AdMob Integration**: Ready for production with test ads configured

## Setup Instructions

### Prerequisites
- Android Studio Hedgehog (2023.1.1) or later
- JDK 8 or higher
- Android SDK with API level 34
- Gradle 8.2

### Building the Project

1. Open the project in Android Studio
2. Sync Gradle files
3. Run the app on an emulator or physical device (API 24+)

```bash
./gradlew assembleDebug
```

### Setting Up Real Ads for Production

The app currently uses **test ad unit IDs**. To monetize in production:

1. **Create a Google AdMob Account**
   - Go to https://admob.google.com
   - Sign up and create a new app

2. **Get Your Ad Unit IDs**
   - Create ad units for:
     - Banner Ad
     - Interstitial Ad
     - Rewarded Ad

3. **Update the App**

   Replace test IDs in `app/src/main/java/com/taptycoon/clicker/ads/AdManager.kt`:

   ```kotlin
   // Replace these with your real ad unit IDs
   const val BANNER_AD_UNIT_ID = "ca-app-pub-XXXXXXXXXXXXXXXX/YYYYYYYYYY"
   const val INTERSTITIAL_AD_UNIT_ID = "ca-app-pub-XXXXXXXXXXXXXXXX/YYYYYYYYYY"
   const val REWARDED_AD_UNIT_ID = "ca-app-pub-XXXXXXXXXXXXXXXX/YYYYYYYYYY"
   ```

4. **Update google-services.json**

   Replace `app/google-services.json` with your actual Firebase/AdMob configuration file.

   Also update the AdMob App ID in `app/src/main/AndroidManifest.xml`:

   ```xml
   <meta-data
       android:name="com.google.android.gms.ads.APPLICATION_ID"
       android:value="ca-app-pub-XXXXXXXXXXXXXXXX~YYYYYYYYYY"/>
   ```

### Publishing to Google Play Store

1. **Generate a Signed APK/Bundle**
   ```bash
   ./gradlew bundleRelease
   ```

2. **Create App Store Assets**
   - App icon (512x512 PNG)
   - Feature graphic (1024x500 PNG)
   - Screenshots (at least 2)
   - Privacy policy URL

3. **Set Up In-App Purchases (Optional)**
   - Consider adding IAP for:
     - Remove ads ($2.99)
     - Coin packs ($0.99 - $9.99)
     - Double coins permanently ($4.99)

4. **App Store Listing**
   - Title: "Tap Tycoon - Idle Clicker Game"
   - Category: Casual / Simulation
   - Content Rating: Everyone
   - Target audience: 13+

## Monetization Strategy

### Current Implementation
- **Banner Ads**: Always visible, generates steady passive revenue
- **Interstitial Ads**: Shows every 50 taps, balances revenue with user experience
- **Rewarded Ads**: Optional value exchange, typically highest eCPM

### Optimization Tips
1. **Ad Frequency**: Current settings are balanced. Monitor user retention if changing.
2. **Rewarded Ad Placement**: Offer rewards at strategic moments (e.g., when player is stuck)
3. **Ad Mediation**: Consider using AdMob mediation to maximize fill rates
4. **A/B Testing**: Test different reward amounts and ad frequencies

### Expected Revenue (Estimates)
- **1,000 DAU** with moderate engagement: $5-15/day
- **10,000 DAU**: $50-150/day
- **100,000 DAU**: $500-1,500/day

*Note: Actual revenue depends on geographic location, engagement, and ad quality.*

## Future Enhancement Ideas

### Gameplay
- Prestige system (reset for permanent bonuses)
- Achievement system
- Cloud save with Google Play Games
- Leaderboards
- Special events and limited-time upgrades
- Visual themes and skins

### Monetization
- In-app purchases for coin packs
- Remove ads option ($2.99)
- VIP subscription ($4.99/month)
  - 2x coin production
  - Exclusive upgrades
  - No interstitial ads
- Offer wall integration

### Technical
- Analytics integration (Firebase Analytics)
- Crash reporting (Firebase Crashlytics)
- Push notifications for re-engagement
- Daily rewards system

## Technical Stack

- **Language**: Kotlin
- **UI Framework**: Jetpack Compose
- **Architecture**: MVVM with StateFlow
- **Persistence**: DataStore Preferences
- **Ads**: Google AdMob SDK
- **Min SDK**: 24 (Android 7.0)
- **Target SDK**: 34 (Android 14)

## Project Structure

```
app/src/main/java/com/taptycoon/clicker/
├── MainActivity.kt              # Main UI and Compose screens
├── ads/
│   └── AdManager.kt            # AdMob integration
├── data/
│   ├── GameState.kt            # Game state data classes
│   └── GameDataStore.kt        # Persistence layer
└── viewmodel/
    └── GameViewModel.kt        # Game logic and state management
```

## License

This project is provided as-is for educational and commercial use.

## Support

For issues or questions, please open an issue on GitHub.

---

**Happy Tapping!** 💎💰
