using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that helps keep track of the amount of each food item there is per level.
/// </summary>
public class Level
{
    private int level;
    private int Croissant; // L1 = 8; L2 = 0; L3 = 4.
    private int DoughnutChoco; // L1 = 8; L2 = 0; L3 = 8.
    private int DoughnutWhite; // L1 = 0; L2 = 8; L3 = 6.
    private int DoughnutPink; // L1 = 0; L2 = 0; L3 = 8.
    private int CupcakeCherry; // L1 = 0; L2 = 10; L3 = 10.
    private int CupcakeChocoChips; // L1 = 0; L2 = 0; L3 = 10.

    private GameObject[] level1foods;
    private GameObject[] level2foods;
    private GameObject[] level3foods;

    public Level(int level_)
    {
        level = level_;
        SetFoodAmounts();

        level1foods = GameObject.FindGameObjectsWithTag("foodLevel1");
        level2foods = GameObject.FindGameObjectsWithTag("foodLevel2");
        level3foods = GameObject.FindGameObjectsWithTag("foodLevel3");
    }

    public int GetGameLevel()
    {
        return level;
    }

    /// <summary>
    /// Set the amount for each food item depending on the level.
    /// </summary>
    private void SetFoodAmounts()
    {
        SetCroissants();
        SetDoughnutChoco();
        SetDoughnutWhite();
        SetDoughnutPink();
        SetCherryCupcakes();
        SetChocoCupcakes();
    }

    /// <summary>
    /// Set the amount of croissants on each level.
    /// </summary>
    private void SetCroissants()
    {
        switch (level)
        {
            case 1:
                Croissant = 8;
                break;
            case 2:
                Croissant = 0;
                break;
            case 3:
                Croissant = 4;
                break;
        }
    }

    /// <summary>
    /// Set the amount of choco doughnuts on each level.
    /// </summary>
    private void SetDoughnutChoco()
    {
        switch (level)
        {
            case 1:
            case 3:
                DoughnutChoco = 8;
                break;
            case 2:
                DoughnutChoco = 0;
                break;
        }
    }

    /// <summary>
    /// Set the amount of white doughnuts on each level.
    /// </summary>
    private void SetDoughnutWhite()
    {
        switch (level)
        {
            case 1:
                DoughnutWhite = 0;
                break;
            case 2:
                DoughnutWhite = 8;
                break;
            case 3:
                DoughnutWhite = 6;
                break;
        }
    }

    /// <summary>
    /// Set the amount of pink doughnuts on each level.
    /// </summary>
    private void SetDoughnutPink()
    {
        switch (level)
        {
            case 1:
            case 2:
                DoughnutPink = 0;
                break;
            case 3:
                DoughnutPink = 8;
                break;
        }
    }

    /// <summary>
    /// Set the amount of cherry and choco chip cupcakes on each level.
    /// </summary>
    private void SetCherryCupcakes()
    {
        switch (level)
        {
            case 1:
                CupcakeCherry = 0;
                break;
            case 2:
            case 3:
                CupcakeCherry = 10;
                break;
        }
    }

    private void SetChocoCupcakes()
    {
        switch (level)
        {
            case 1:
            case 2:
                CupcakeChocoChips = 0;
                break;
            case 3:
                CupcakeChocoChips = 10;
                break;
        }
    }

    public int GetLevel()
    {
        return level;
    }

    public GameObject[] GetCurrentLevelFoods()
    {
        switch(level)
        {
            case 1:
                return level1foods;
            case 2:
                return level2foods;
            case 3:
                return level3foods;
            default:
                return null;
        }
    }

    /// <summary>
    /// Method that helps removing one item to each food counter.
    /// </summary>
    /// <param name="food">Can have values: DoughnutChoco, DoughnutWhite, DoughnutPink, Croissant, CupcakeCherry, CupcakeChocoChips.</param>
    public void RemoveFood(string food)
    {
        switch(food)
        {
            case "Croissant":
                if (Croissant > 0)
                    Croissant -= 1;
                break;
            case "DoughnutChoco":
                if (DoughnutChoco > 0)
                    DoughnutChoco -= 1;
                break;
            case "DoughnutWhite":
                if (DoughnutWhite > 0)
                    DoughnutWhite -= 1;
                break;
            case "DoughnutPink":
                if (DoughnutPink > 0)
                    DoughnutPink -= 1;
                break;
            case "CupcakeCherry":
                if (CupcakeCherry > 0)
                    CupcakeCherry -= 1;
                break;
            case "CupcakeChocoChips":
                if (CupcakeChocoChips > 0)
                    CupcakeChocoChips -= 1;
                break;
        }
    }

    /// <summary>
    /// Method that helps knowing if there is availability of the food selected.
    /// </summary>
    /// <param name="food">Can take values 0:croissant; 1:chocoDou; 2: whiteDou; 3:pinkDou; 4:cherryCup; 5:chocoChipCup</param>
    /// <returns>Returns true if there is still food available. False otherwise.</returns>
    public bool CheckAvailability(int food)
    {
        bool isAvailable = true;

        switch (food)
        {
            case 0: // Croissant
                if (Croissant == 0)
                    isAvailable = false;
                break;
            case 1: // DoughnutChoco
                if (DoughnutChoco == 0)
                    isAvailable = false;
                break;
            case 2: // DoughnutWhite
                if (DoughnutWhite == 0)
                    isAvailable = false;
                break;
            case 3: // DoughnutPink
                if (DoughnutPink == 0)
                    isAvailable = false;
                break;
            case 4: // CupcakeCherry
                if (CupcakeCherry == 0)
                    isAvailable = false;
                break;
            case 5: // CupcakeChocoChips
                if (CupcakeChocoChips == 0)
                    isAvailable = false;
                break;
        }

        return isAvailable;
    }
}
