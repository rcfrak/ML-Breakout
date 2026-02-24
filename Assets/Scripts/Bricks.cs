using UnityEngine;

public class Brick : MonoBehaviour
{
    public static System.Action OnBrickDestroyed;

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
        OnBrickDestroyed?.Invoke(); // used for brick breaking rewards.

        // Transform to white
        _renderer.color = Color.white;

        // Destroy the brick after 0.25 seconds
        Destroy(gameObject, 0.25f);
    }
}