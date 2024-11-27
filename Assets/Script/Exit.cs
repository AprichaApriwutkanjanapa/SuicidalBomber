using UnityEngine;

public class Exit : Identity
{
    public GameObject YouWin;

    public override void Hit()
    {
        //int keyAmount = mapGenerator.player.inventory.numberOfItem("Red Key");

        //if (keyAmount > 0)
        {
            //mapGenerator.player.inventory.UseItem("Red Item");
            Debug.Log("Exit unlocked");
            mapGenerator.player.enabled = false;
            YouWin.SetActive(true);
            Debug.Log("You win");
        }
        //else
        {
            Debug.Log("You Need to Collect Key First");
        }
            

    }
}
