using UnityEngine;

public class Player : Character
{
    public Inventory inventory;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2.left);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2.right);
        }
        
    }
    
}
