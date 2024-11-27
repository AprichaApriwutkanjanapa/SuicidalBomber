using UnityEngine;

public class BombFragment : Identity
{
    //[Header("Crafting Item")]
    public string itemName;
    
    public BombFragment()
    {
        itemName = this.Name;
    }
    

    public override void Hit()
    {
        mapGenerator.player.inventory.AddItem(itemName);
        mapGenerator.player.inventory.ShowInventory();
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
        Destroy(gameObject);
    }
}
