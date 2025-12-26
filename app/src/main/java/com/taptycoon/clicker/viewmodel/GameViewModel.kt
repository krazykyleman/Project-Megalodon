package com.taptycoon.clicker.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.taptycoon.clicker.data.GameDataStore
import com.taptycoon.clicker.data.GameState
import com.taptycoon.clicker.data.Upgrade
import kotlinx.coroutines.Job
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

class GameViewModel(private val dataStore: GameDataStore) : ViewModel() {

    private val _gameState = MutableStateFlow(GameState())
    val gameState: StateFlow<GameState> = _gameState.asStateFlow()

    private var autoClickerJob: Job? = null

    private val availableUpgrades = listOf(
        Upgrade(
            id = "cursor",
            name = "👆 Auto Tapper",
            description = "Taps automatically",
            baseCost = 10L,
            coinsPerSecond = 0.1
        ),
        Upgrade(
            id = "grandma",
            name = "👵 Grandma",
            description = "A nice grandma to tap for you",
            baseCost = 100L,
            coinsPerSecond = 1.0
        ),
        Upgrade(
            id = "farm",
            name = "🌾 Farm",
            description = "Grows coins naturally",
            baseCost = 1100L,
            coinsPerSecond = 8.0
        ),
        Upgrade(
            id = "mine",
            name = "⛏️ Mine",
            description = "Extracts precious coins",
            baseCost = 12000L,
            coinsPerSecond = 47.0
        ),
        Upgrade(
            id = "factory",
            name = "🏭 Factory",
            description = "Mass produces coins",
            baseCost = 130000L,
            coinsPerSecond = 260.0
        ),
        Upgrade(
            id = "bank",
            name = "🏦 Bank",
            description = "Generates coin interest",
            baseCost = 1400000L,
            coinsPerSecond = 1400.0
        ),
        Upgrade(
            id = "temple",
            name = "⛩️ Temple",
            description = "Summons coin spirits",
            baseCost = 20000000L,
            coinsPerSecond = 7800.0
        ),
        Upgrade(
            id = "wizard",
            name = "🧙 Wizard Tower",
            description = "Creates coins with magic",
            baseCost = 330000000L,
            coinsPerSecond = 44000.0
        ),
        Upgrade(
            id = "spaceship",
            name = "🚀 Spaceship",
            description = "Mines asteroids for coins",
            baseCost = 5100000000L,
            coinsPerSecond = 260000.0
        ),
        Upgrade(
            id = "portal",
            name = "🌀 Portal",
            description = "Brings coins from other dimensions",
            baseCost = 75000000000L,
            coinsPerSecond = 1600000.0
        )
    )

    init {
        loadGameState()
        startAutoClicker()
    }

    private fun loadGameState() {
        viewModelScope.launch {
            dataStore.gameStateFlow.collect { (savedCoins, savedUpgrades) ->
                val upgrades = availableUpgrades.map { upgrade ->
                    upgrade.copy(owned = savedUpgrades[upgrade.id] ?: 0)
                }
                val totalCPS = upgrades.sumOf { it.totalCoinsPerSecond }

                _gameState.value = _gameState.value.copy(
                    coins = savedCoins,
                    upgrades = upgrades,
                    coinsPerSecond = totalCPS
                )
            }
        }
    }

    private fun startAutoClicker() {
        autoClickerJob?.cancel()
        autoClickerJob = viewModelScope.launch {
            while (true) {
                delay(100) // Update 10 times per second for smooth animation
                val currentState = _gameState.value
                if (currentState.coinsPerSecond > 0) {
                    val coinsToAdd = (currentState.coinsPerSecond * 0.1).toLong()
                    if (coinsToAdd > 0) {
                        _gameState.value = currentState.copy(
                            coins = currentState.coins + coinsToAdd
                        )
                        saveGameState()
                    }
                }
            }
        }
    }

    fun onTap() {
        val currentState = _gameState.value
        _gameState.value = currentState.copy(
            coins = currentState.coins + currentState.coinsPerTap,
            totalTaps = currentState.totalTaps + 1
        )
        saveGameState()
    }

    fun buyUpgrade(upgrade: Upgrade) {
        val currentState = _gameState.value
        if (currentState.coins >= upgrade.currentCost) {
            val updatedUpgrades = currentState.upgrades.map {
                if (it.id == upgrade.id) it.copy(owned = it.owned + 1)
                else it
            }
            val totalCPS = updatedUpgrades.sumOf { it.totalCoinsPerSecond }

            _gameState.value = currentState.copy(
                coins = currentState.coins - upgrade.currentCost,
                upgrades = updatedUpgrades,
                coinsPerSecond = totalCPS
            )
            saveGameState()
        }
    }

    fun addBonusCoins(amount: Long) {
        val currentState = _gameState.value
        _gameState.value = currentState.copy(
            coins = currentState.coins + amount
        )
        saveGameState()
    }

    private fun saveGameState() {
        viewModelScope.launch {
            val currentState = _gameState.value
            val upgradesMap = currentState.upgrades.associate { it.id to it.owned }
            dataStore.saveGameState(currentState.coins, upgradesMap)
        }
    }

    override fun onCleared() {
        super.onCleared()
        autoClickerJob?.cancel()
    }
}
