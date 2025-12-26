package com.taptycoon.clicker

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.animation.core.*
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.PlayArrow
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.scale
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.compose.ui.viewinterop.AndroidView
import com.google.android.gms.ads.AdRequest
import com.google.android.gms.ads.AdSize
import com.google.android.gms.ads.AdView
import com.taptycoon.clicker.ads.AdManager
import com.taptycoon.clicker.data.GameDataStore
import com.taptycoon.clicker.data.Upgrade
import com.taptycoon.clicker.viewmodel.GameViewModel
import java.text.NumberFormat

class MainActivity : ComponentActivity() {

    private lateinit var adManager: AdManager
    private lateinit var viewModel: GameViewModel

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        adManager = AdManager(this)
        adManager.loadRewardedAd()
        adManager.loadInterstitialAd()

        val dataStore = GameDataStore(this)
        viewModel = GameViewModel(dataStore)

        setContent {
            TapTycoonTheme {
                GameScreen(
                    viewModel = viewModel,
                    adManager = adManager,
                    activity = this
                )
            }
        }
    }
}

@Composable
fun TapTycoonTheme(content: @Composable () -> Unit) {
    MaterialTheme(
        colorScheme = darkColorScheme(
            primary = Color(0xFF6200EE),
            secondary = Color(0xFF03DAC6),
            background = Color(0xFF121212),
            surface = Color(0xFF1E1E1E)
        ),
        content = content
    )
}

@Composable
fun GameScreen(
    viewModel: GameViewModel,
    adManager: AdManager,
    activity: ComponentActivity
) {
    val gameState by viewModel.gameState.collectAsState()
    var tapAnimation by remember { mutableStateOf(false) }
    var showInterstitialCounter by remember { mutableStateOf(0) }

    val scale by animateFloatAsState(
        targetValue = if (tapAnimation) 1.2f else 1f,
        animationSpec = spring(
            dampingRatio = Spring.DampingRatioMediumBouncy,
            stiffness = Spring.StiffnessLow
        ),
        finishedListener = { tapAnimation = false }
    )

    Column(
        modifier = Modifier
            .fillMaxSize()
            .background(
                Brush.verticalGradient(
                    colors = listOf(
                        Color(0xFF1a237e),
                        Color(0xFF0d47a1),
                        Color(0xFF01579b)
                    )
                )
            )
    ) {
        // Header with coin count
        Column(
            modifier = Modifier
                .fillMaxWidth()
                .padding(16.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Text(
                text = "💰 Tap Tycoon",
                fontSize = 32.sp,
                fontWeight = FontWeight.Bold,
                color = Color(0xFFFFD700)
            )
            Spacer(modifier = Modifier.height(8.dp))
            Text(
                text = formatNumber(gameState.coins),
                fontSize = 48.sp,
                fontWeight = FontWeight.Bold,
                color = Color.White
            )
            Text(
                text = "Coins",
                fontSize = 18.sp,
                color = Color(0xFFB0B0B0)
            )
            Spacer(modifier = Modifier.height(4.dp))
            Text(
                text = "${formatNumber(gameState.coinsPerSecond.toLong())} per second",
                fontSize = 16.sp,
                color = Color(0xFF4CAF50)
            )
        }

        // Tap Button
        Box(
            modifier = Modifier
                .fillMaxWidth()
                .padding(16.dp),
            contentAlignment = Alignment.Center
        ) {
            Surface(
                modifier = Modifier
                    .size(200.dp)
                    .scale(scale)
                    .clickable {
                        tapAnimation = true
                        viewModel.onTap()
                        showInterstitialCounter++
                        if (showInterstitialCounter >= 50) {
                            adManager.showInterstitialAd(activity)
                            showInterstitialCounter = 0
                        }
                    },
                shape = CircleShape,
                color = Color(0xFFFFD700),
                shadowElevation = 8.dp
            ) {
                Box(contentAlignment = Alignment.Center) {
                    Text(
                        text = "💎",
                        fontSize = 80.sp
                    )
                }
            }
        }

        // Rewarded Ad Button
        Button(
            onClick = {
                adManager.showRewardedAd(
                    activity = activity,
                    onUserEarnedReward = { _ ->
                        viewModel.addBonusCoins(500)
                    },
                    onAdDismissed = {}
                )
            },
            modifier = Modifier
                .fillMaxWidth()
                .padding(horizontal = 16.dp),
            colors = ButtonDefaults.buttonColors(
                containerColor = Color(0xFF4CAF50)
            )
        ) {
            Icon(Icons.Default.PlayArrow, contentDescription = null)
            Spacer(modifier = Modifier.width(8.dp))
            Text("Watch Ad for 500 Coins!")
        }

        Spacer(modifier = Modifier.height(16.dp))

        // Upgrades Section
        Text(
            text = "⬆️ Upgrades",
            fontSize = 24.sp,
            fontWeight = FontWeight.Bold,
            color = Color.White,
            modifier = Modifier.padding(horizontal = 16.dp)
        )

        Spacer(modifier = Modifier.height(8.dp))

        LazyColumn(
            modifier = Modifier
                .weight(1f)
                .fillMaxWidth()
                .padding(horizontal = 16.dp),
            verticalArrangement = Arrangement.spacedBy(8.dp)
        ) {
            items(gameState.upgrades) { upgrade ->
                UpgradeCard(
                    upgrade = upgrade,
                    canAfford = gameState.coins >= upgrade.currentCost,
                    onBuy = { viewModel.buyUpgrade(upgrade) }
                )
            }
        }

        // Banner Ad at bottom
        BannerAdView(
            modifier = Modifier
                .fillMaxWidth()
                .height(50.dp)
        )
    }
}

@Composable
fun UpgradeCard(
    upgrade: Upgrade,
    canAfford: Boolean,
    onBuy: () -> Unit
) {
    Card(
        modifier = Modifier
            .fillMaxWidth()
            .clickable(enabled = canAfford) { onBuy() },
        shape = RoundedCornerShape(12.dp),
        colors = CardDefaults.cardColors(
            containerColor = if (canAfford) Color(0xFF2C2C2C) else Color(0xFF1A1A1A)
        )
    ) {
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(16.dp),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Column(modifier = Modifier.weight(1f)) {
                Text(
                    text = upgrade.name,
                    fontSize = 18.sp,
                    fontWeight = FontWeight.Bold,
                    color = if (canAfford) Color.White else Color.Gray
                )
                Text(
                    text = upgrade.description,
                    fontSize = 14.sp,
                    color = if (canAfford) Color(0xFFB0B0B0) else Color.DarkGray
                )
                Text(
                    text = "Produces ${formatNumber(upgrade.coinsPerSecond.toLong())}/sec",
                    fontSize = 12.sp,
                    color = Color(0xFF4CAF50)
                )
            }

            Column(horizontalAlignment = Alignment.End) {
                Text(
                    text = "Owned: ${upgrade.owned}",
                    fontSize = 14.sp,
                    color = Color(0xFF03DAC6)
                )
                Spacer(modifier = Modifier.height(4.dp))
                Text(
                    text = formatNumber(upgrade.currentCost),
                    fontSize = 16.sp,
                    fontWeight = FontWeight.Bold,
                    color = if (canAfford) Color(0xFFFFD700) else Color.Gray
                )
            }
        }
    }
}

@Composable
fun BannerAdView(modifier: Modifier = Modifier) {
    val context = LocalContext.current
    AndroidView(
        modifier = modifier,
        factory = { ctx ->
            AdView(ctx).apply {
                setAdSize(AdSize.BANNER)
                adUnitId = AdManager.BANNER_AD_UNIT_ID
                loadAd(AdRequest.Builder().build())
            }
        }
    )
}

fun formatNumber(number: Long): String {
    return when {
        number >= 1_000_000_000_000 -> String.format("%.2fT", number / 1_000_000_000_000.0)
        number >= 1_000_000_000 -> String.format("%.2fB", number / 1_000_000_000.0)
        number >= 1_000_000 -> String.format("%.2fM", number / 1_000_000.0)
        number >= 1_000 -> String.format("%.2fK", number / 1_000.0)
        else -> NumberFormat.getInstance().format(number)
    }
}

fun formatNumber(number: Double): String {
    return formatNumber(number.toLong())
}
