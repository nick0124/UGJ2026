using UnityEngine;

public class Turret : MonoBehaviour
{
    [field: SerializeField] public Texture Icon { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
}
