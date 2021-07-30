using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowArrow : MonoBehaviour
{
    private Rigidbody myBody;
    public float speed = 30f;
    public float deactive_Timer = 3f;
    public float damage = 15f;
    // Start is called before the first frame update
    private void Awake()
    {
        myBody = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        Invoke("DeactivateGameObject", deactive_Timer);
    }
    public void Launch(Camera mainCamera)
    {
        myBody.velocity = mainCamera.transform.forward * speed;
        gameObject.transform.LookAt(gameObject.transform.position, myBody.velocity);
    }
    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider target)
    {
        
    }
}
