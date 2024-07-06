using UnityEngine;

public class CollectableComponent : MonoBehaviour
{
    [SerializeField] private Collectable collectable;

    [HideInInspector] public new string name;
    [HideInInspector] public float scoreAmount;
    [HideInInspector] public Sprite sprite;
    
    void Start()
    {
        name = collectable.name;
        scoreAmount = collectable.scoreAmount;
        sprite = collectable.sprite;

        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
