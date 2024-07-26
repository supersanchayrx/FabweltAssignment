using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float speed;

    private float smoothRotTime = 0.3f;
    private float smoothRotvel;

    public bool canMove, isMoving, isWalking;

    Animator anim;

    Transform cam;

    Rigidbody rb;

    public Vector3 velocity;

    void Start()
    {
        canMove = true;
        isMoving = false;
        anim = GetComponent<Animator>();
        cam = GameObject.Find("Main Camera").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        velocity = rb.velocity;

        anim.SetBool("idle", !isMoving);
        anim.SetBool("isWalking",isWalking);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 moveInput = new Vector3(horizontal, 0, vertical);


        if (moveInput.magnitude > 0.1f && canMove)
        {
            //isMoving = true;
            float TargetAngle = Mathf.Atan2(moveInput.x, moveInput.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float smoothRotAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetAngle, ref smoothRotvel, smoothRotTime);
            transform.rotation = Quaternion.Euler(0, smoothRotAngle, 0);
            Vector3 moveDirn = Quaternion.Euler(0, TargetAngle, 0) * Vector3.forward;



            //transform.position += speed * Time.deltaTime * moveDirn.normalized;
            rb.velocity = moveDirn.normalized*speed*Time.deltaTime;
        }

        /*else
        {
            isMoving = false;
        }*/

        if(Mathf.Abs(rb.velocity.x) >=0.5f || Mathf.Abs(rb.velocity.z) >= 0.5f)
        {
            isMoving = true ;
            isWalking = true ;
        }
        else
        {
            isMoving = false;
            isWalking = false ;
        }

    }
}
