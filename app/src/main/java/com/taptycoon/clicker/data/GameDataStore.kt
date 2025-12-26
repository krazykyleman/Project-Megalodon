package com.taptycoon.clicker.data

import android.content.Context
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.Preferences
import androidx.datastore.preferences.core.edit
import androidx.datastore.preferences.core.longPreferencesKey
import androidx.datastore.preferences.core.stringPreferencesKey
import androidx.datastore.preferences.preferencesDataStore
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.map

private val Context.dataStore: DataStore<Preferences> by preferencesDataStore(name = "game_data")

class GameDataStore(private val context: Context) {

    companion object {
        private val COINS_KEY = longPreferencesKey("coins")
        private val TOTAL_TAPS_KEY = longPreferencesKey("total_taps")
        private val UPGRADES_KEY = stringPreferencesKey("upgrades")
    }

    val gameStateFlow: Flow<Pair<Long, Map<String, Int>>> = context.dataStore.data.map { preferences ->
        val coins = preferences[COINS_KEY] ?: 0L
        val upgradesJson = preferences[UPGRADES_KEY] ?: ""
        val upgrades = parseUpgrades(upgradesJson)
        Pair(coins, upgrades)
    }

    suspend fun saveGameState(coins: Long, upgrades: Map<String, Int>) {
        context.dataStore.edit { preferences ->
            preferences[COINS_KEY] = coins
            preferences[UPGRADES_KEY] = serializeUpgrades(upgrades)
        }
    }

    private fun parseUpgrades(json: String): Map<String, Int> {
        if (json.isEmpty()) return emptyMap()
        return json.split(";").mapNotNull { entry ->
            val parts = entry.split(":")
            if (parts.size == 2) parts[0] to parts[1].toIntOrNull()
            else null
        }.mapNotNull { (key, value) ->
            value?.let { key to it }
        }.toMap()
    }

    private fun serializeUpgrades(upgrades: Map<String, Int>): String {
        return upgrades.entries.joinToString(";") { "${it.key}:${it.value}" }
    }
}
