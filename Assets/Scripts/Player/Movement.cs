using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public Slider healthSlider;
    public Vector3 healthSliderOffset;
    public float MaxHealth;
    public float currentHealth;
    public float Speed;
    float healthPercent;
    
   

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();

        //Player Movement
        //Moves Player Up
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + new Vector3(0,0,0.01f) * Speed; 
        }
        //Moves Player Down
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position - new Vector3(0, 0, 0.01f) * Speed;
        }
        //Moves Player Left
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + new Vector3(-0.01f, 0, 0) * Speed;
        }
        //Moves Player Right
        if (Input.GetKey(KeyCode.D))
         {
            transform.position = transform.position + new Vector3(0.01f, 0, 0) * Speed;
        }

    }
  public void TakeDamage(float Amount)
    {
        currentHealth -= Amount;  
    }

    void UpdateUI()
    {
        healthPercent = currentHealth / MaxHealth;
        healthSlider.value = healthPercent;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        healthSlider.transform.position = screenPos + healthSliderOffset;
    }
}
