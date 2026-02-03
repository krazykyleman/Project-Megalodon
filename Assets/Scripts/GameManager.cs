using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public double coins = 0;
    public double totalTaps = 0;
    public double coinsPerTap = 1;
    public double coinsPerSecond = 0;

    [Header("Settings")]
    public float autoClickInterval = 0.1f;

    [Header("References")]
    public List<Upgrade> upgrades = new List<Upgrade>();

    private float autoClickTimer = 0f;

    public event Action OnCoinsChanged;
    public event Action OnUpgradeChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeUpgrades();
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(AutoSave), 30f, 30f);
    }

    private void Update()
    {
        if (coinsPerSecond > 0)
        {
            autoClickTimer += Time.deltaTime;
            if (autoClickTimer >= autoClickInterval)
            {
                double coinsToAdd = coinsPerSecond * autoClickInterval;
                AddCoins(coinsToAdd);
                autoClickTimer = 0f;
            }
        }
    }

    private void InitializeUpgrades()
    {
        upgrades.Clear();

        upgrades.Add(new Upgrade
        {
            id = "cursor",
            upgradeName = "Auto Tapper",
            description = "Taps automatically",
            icon = "👆",
            baseCost = 10,
            coinsPerSecond = 0.1,
            owned = 0
        });

        upgrades.Add(new Upgrade
        {
            id = "grandma",
            upgradeName = "Grandma",
            description = "A nice grandma to tap for you",
            icon = "👵",
            baseCost = 100,
            coinsPerSecond = 1.0,
            owned = 0
        });

        upgrades.Add(new Upgrade
        {
            id = "farm",
            upgradeName = "Farm",
            description = "Grows coins naturally",
            icon = "🌾",
            baseCost = 1100,
            coinsPerSecond = 8.0,
            owned = 0
        });

        upgrades.Add(new Upgrade
        {
            id = "mine",
            upgradeName = "Mine",
            description = "Extracts precious coins",
            icon = "⛏️",
            baseCost = 12000,
            coinsPerSecond = 47.0,
            owned = 0
        });

        upgrades.Add(new Upgrade
        {
            id = "factory",
            upgradeName = "Factory",
            description = "Mass produces coins",
            icon = "🏭",
            baseCost = 130000,
            coinsPerSecond = 260.0,
            owned = 0
        });

        upgrades.Add(new Upgrade
        {
            id = "bank",
            upgradeName = "Bank",
            description = "Generates coin interest",
            icon = "🏦",
            baseCost = 1400000,
            coinsPerSecond = 1400.0,
            owned = 0
        });

        upgrades.Add(new Upgrade
        {
            id = "temple",
            upgradeName = "Temple",
            description = "Summons coin spirits",
            icon = "⛩️",
            baseCost = 20000000,
            coinsPerSecond = 7800.0,
            owned = 0
        });

        upgrades.Add(new Upgrade
        {
            id = "wizard",
            upgradeName = "Wizard Tower",
            description = "Creates coins with magic",
            icon = "🧙",
            baseCost = 330000000,
            coinsPerSecond = 44000.0,
            owned = 0
        });

        upgrades.Add(new Upgrade
        {
            id = "spaceship",
            upgradeName = "Spaceship",
            description = "Mines asteroids for coins",
            icon = "🚀",
            baseCost = 5100000000,
            coinsPerSecond = 260000.0,
            owned = 0
        });

        upgrades.Add(new Upgrade
        {
            id = "portal",
            upgradeName = "Portal",
            description = "Brings coins from other dimensions",
            icon = "🌀",
            baseCost = 75000000000,
            coinsPerSecond = 1600000.0,
            owned = 0
        });
    }

    public void OnTap()
    {
        AddCoins(coinsPerTap);
        totalTaps++;
    }

    public void AddCoins(double amount)
    {
        coins += amount;
        OnCoinsChanged?.Invoke();
    }

    public void BuyUpgrade(Upgrade upgrade)
    {
        double cost = upgrade.GetCurrentCost();
        if (coins >= cost)
        {
            coins -= cost;
            upgrade.owned++;
            RecalculateCoinsPerSecond();
            OnCoinsChanged?.Invoke();
            OnUpgradeChanged?.Invoke();
            SaveGame();
        }
    }

    public void AddRewardCoins(double amount)
    {
        AddCoins(amount);
        SaveGame();
    }

    private void RecalculateCoinsPerSecond()
    {
        coinsPerSecond = 0;
        foreach (var upgrade in upgrades)
        {
            coinsPerSecond += upgrade.GetTotalCoinsPerSecond();
        }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            coins = data.coins;
            totalTaps = data.totalTaps;

            for (int i = 0; i < upgrades.Count && i < data.upgradeOwned.Count; i++)
            {
                upgrades[i].owned = data.upgradeOwned[i];
            }

            RecalculateCoinsPerSecond();
            OnCoinsChanged?.Invoke();
            OnUpgradeChanged?.Invoke();
        }
    }

    private void AutoSave()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveGame();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
