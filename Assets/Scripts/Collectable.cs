using UnityEngine;

[CreateAssetMenu(fileName = "New Collectable", menuName = "Collectable")]
public class Collectable : ScriptableObject
{
    public new string name;
    public float scoreAmount;
    
    public Sprite sprite;
}
