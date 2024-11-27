using UnityEngine;

public abstract class Identity : MonoBehaviour
{
    [Header("Identity")]
    public string Name;
    public int positionX;
    public int positionY;

    public MapGenerator mapGenerator;

    public virtual void Hit()
    {
        
    }
}
