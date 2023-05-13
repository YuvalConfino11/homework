using UnityEngine;

public class BackgroundColorController : MonoBehaviour
{
    private Camera cam;
    private SpriteRenderer backgroundSpriteRenderer;
    private float maxHealth;
    [SerializeField]
    private Player _player;

    private float _playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        backgroundSpriteRenderer = GetComponent<SpriteRenderer>();
        _playerHealth = _player.GetCurrentHealth();
        maxHealth = _player.GetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        float healthRatio = _player.GetCurrentHealth() / maxHealth;
        Color targetColor = Color.HSVToRGB(0,  0, healthRatio, false);
        backgroundSpriteRenderer.color = Color.Lerp(backgroundSpriteRenderer.color, targetColor, Time.deltaTime);
    }
}