using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayScale : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private float _duration = 1f;
    private static readonly int GrayscaleAmount = Shader.PropertyToID("_GrayscaleAmount");
    private float _maxHealth;
    [SerializeField]
    private Player _player;
    private float _playerHealth;
    private float _healthRatio;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerHealth = _player.GetCurrentHealth();
        _maxHealth = _player.GetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private IEnumerator GrayScaleRoutine(float currentHealth, float previousHealth)
    {
        float time = previousHealth / _maxHealth; //80/100 = 0.8   //0.6
        float duration = currentHealth / _maxHealth; //60/ 100 = 0.6 //0.8
        float ratio = 1f - time;  //0.4
        float grayAmount = ratio;  //0.4
        while (duration < time)
        {
            grayAmount += Time.deltaTime;
            SetGrayscale(grayAmount);
            duration += Time.deltaTime;
            yield return null;
        }
    }
    
    public void SetGrayscale(float amount=1)
    {
        _spriteRenderer.material.SetFloat(GrayscaleAmount, amount);
    }
    
}
