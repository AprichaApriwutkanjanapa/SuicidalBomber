using UnityEngine;

public class Wall : Identity
{
    [Header("Wall Configuration")] public bool IsGodWall;
    public int itemDropChance = 15;
    public int Randomcheck;

    private void Start()
    {
        DefineWalls();
    }

    public override void Hit()
    {
        if (!IsGodWall)
        {
            Randomcheck = Random.Range(0, 100);
            if (Randomcheck <= itemDropChance)
            {
                DropItem();
                mapGenerator.mapdata[positionX, positionY] = mapGenerator.bomb;
                Destroy(gameObject);
                Debug.Log($"Wall Destroyed! at {positionX} {positionY} and got Bomb Fragment! (mapdata is {mapGenerator.mapdata[positionX, positionY]})");
            }
            else
            {
                Destroy(gameObject);
                mapGenerator.mapdata[positionX, positionY] = 0;
                //Debug.Log($"Wall Destroyed! {positionX} {positionY} and mapdata is {mapGenerator.mapdata[positionX, positionY]}");
            }
        }
        else
        {
            return;
        }
    }

    public void DefineWalls()
    {
        //ต้องมี Depth First Search
        IsGodWall = Random.Range(0, 100) < 20 ? true : false;
        if (IsGodWall)
        {
            gameObject.layer = LayerMask.NameToLayer("Wall");
            GetComponent<SpriteRenderer>().color = Color.grey;
        }
        else
        {
            // If this GodWall blocks the path, convert it to a normal wall
            gameObject.layer = LayerMask.NameToLayer("Destructible Wall");
            GetComponent<SpriteRenderer>().color = Color.white; // Or your default wall color
        }
    }

    public void DropItem()
    {
        //เดวเขียนเรื่องสุ่มได้ไอเทม BombFragment กับ Health Potion ด้วย
        // Check if we should drop an item
        if (mapGenerator.bombFragments != null && Random.value <= itemDropChance)
        {
            mapGenerator.PlaceItem(positionX,positionY);
                
        }
    }
}
