using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void TakeDamage(float damage)
    {
        // Здесь можно добавить логику уменьшения здоровья врага и его уничтожения
        Debug.Log($"Enemy took {damage} damage!");
    }
}
