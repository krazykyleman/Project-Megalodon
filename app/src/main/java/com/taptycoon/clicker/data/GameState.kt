package com.taptycoon.clicker.data

data class GameState(
    val coins: Long = 0L,
    val totalTaps: Long = 0L,
    val coinsPerTap: Int = 1,
    val coinsPerSecond: Double = 0.0,
    val upgrades: List<Upgrade> = emptyList()
)

data class Upgrade(
    val id: String,
    val name: String,
    val description: String,
    val baseCost: Long,
    val coinsPerSecond: Double,
    val owned: Int = 0
) {
    val currentCost: Long
        get() = (baseCost * Math.pow(1.15, owned.toDouble())).toLong()

    val totalCoinsPerSecond: Double
        get() = coinsPerSecond * owned
}
