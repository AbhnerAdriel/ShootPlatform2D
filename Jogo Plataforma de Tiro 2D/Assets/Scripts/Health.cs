using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int totalHealth = 3;
    public int health;
    private SpriteRenderer _renderer;
    // Start is called before the first frame update
    void Awake()
    {
        _renderer   = GetComponentInParent<SpriteRenderer>();
        health      = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDamage(int amount) 
    {
        health -= amount;

        StartCoroutine("VisualFeedback");
        if (health <= 0)
        {
            health = 0;
        }
    }

    private IEnumerator VisualFeedback() 
    {
        _renderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _renderer.color = Color.white;
    }
}
