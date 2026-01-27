using UnityEngine;

public class Brick : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private bool _isDestroying = false;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // stops multiple hits triggering it over and over
        if (_isDestroying) 
            return;
        _isDestroying = true;

        // Transform to white
        _renderer.color = Color.white;

        // Destroy the brick after 0.25 seconds
        Destroy(gameObject, 0.25f);
    }
}