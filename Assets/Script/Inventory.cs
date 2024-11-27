using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> inventory = new Dictionary<string, int>();
    public int totalFragmentCount;
    private int flintAmout;
    private int powderAmout;
    private int minFragmentsToCraft;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            TryCraftBomb();
        }
    }

    public void AddItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += 1;
            totalFragmentCount++;
        }
        else
        {
            totalFragmentCount++;
            inventory.Add(itemName, 1);
        }
    }
    
    public void UseItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] -= 1;
            if (inventory[itemName] <= 0)
            {
                inventory.Remove(itemName);
            }
        }
        else
        {
            Debug.LogWarning("No item");
        }
    }
    
    public int numberOfItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        { 
            return inventory[itemName];
        }
        else
        {
            return 0;
        }
    }
    
    public void ShowInventory()
    {
        foreach (var kvp in inventory)
        {
            Debug.Log($"{kvp.Key} = {kvp.Value}");
        }
    }
    
    private void TryCraftBomb()
    {
        if (Bomb.IsBombCrafted == true)
        {
            // Check if player has enough fragments to craft
            if (totalFragmentCount >= minFragmentsToCraft)
            {
                // Craft the bomb
                CraftBomb();
            }
            else
            {
                Debug.Log($"Not enough fragments to craft. Need {minFragmentsToCraft}, current: {totalFragmentCount}");
            }
        }
        else
        {
            Debug.Log($"Already Crafted Bomb!");
        }
        
    }

    private void CraftBomb()
    {
        totalFragmentCount -= minFragmentsToCraft;
        inventory.Clear();
        
        // Optional: Show crafting success message
        Debug.Log($"Devil Bomb crafted!! Press Left Shift to use. Fragments remaining: {totalFragmentCount}");
    }
}
