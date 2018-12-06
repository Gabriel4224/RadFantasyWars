using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    public AudioSource AttackSound;

    public GameObject AttackHitBox; 
    public Camera mainCamera;
    public GameObject Player; 
    float Cooldown;
    public int Score;
    public float AttackTime;
    bool Attackcooldown;
    public Animator Anim; 
    public float attackDamage;
    // Use this for initialization
    void Start () {
        AttackSound = GetComponent<AudioSource>();
        //Anim = GetComponent<Animator>();
        Anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update () {
        
        RayCast();  
        //Cooldown for players attack
        if (Attackcooldown == true)
        {
            Cooldown += Time.deltaTime; 
            if (Cooldown >= AttackTime)
            {
                //Enables attack cone
                AttackHitBox.SetActive(true);
                Cooldown = 0;
                //Resets cooldown
                Attackcooldown = false;
            }
        }
        // Once mouse1 is clicked the player will attack
        if (Input.GetMouseButtonDown(0) && Cooldown <= 0)
        {
            AttackSound.Play();
            Anim.Play("Attack");
            Debug.Log("ButtonPressed");
            // disables attack cone
            AttackHitBox.SetActive(false);
            //Cooldown is true
            Attackcooldown = true;
        }
    

    }
    //Camera tracks the mouse cursor and points the player towards it 
    private void RayCast()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && Input.GetMouseButtonDown(0))
        {
            other.GetComponent<BigEnemy>().TakeDamage(attackDamage);
        }
    }
}
