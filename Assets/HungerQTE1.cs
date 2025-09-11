using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class HungerQTE1 : MonoBehaviour
{
    private float speed;
    private float stepsNumber;
    public GameObject dot;
    public GameObject bar;
    private Rigidbody2D dotrb;
    private Rigidbody2D barrb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dotrb = dot.GetComponent<Rigidbody2D>();
        barrb = bar.GetComponent<Rigidbody2D>();
        speed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        dotrb.linearVelocity = new Vector2(speed, 0f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        speed = -speed;
    }
}
