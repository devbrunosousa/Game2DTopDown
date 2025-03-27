using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;

    [SerializeField] private Rigidbody2D rig2D;
    float moveX;
    float moveY;
    [SerializeField] private Animator anim;
    private bool isMoving;
    private bool isAttacking = false;
    private Vector2 lastMoveDirection;

    void Awake()
    {
        rig2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isMoving = false;
        lastMoveDirection = new Vector2(0, -1);
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            isMoving = true;
            lastMoveDirection = new Vector2(moveX, moveY).normalized;
        }
        else
        {
            isMoving = false;
        }

        Move();
        Animations();

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private void Move()
    {
        if(!isAttacking)
            rig2D.MovePosition(rig2D.position + new Vector2(moveX, moveY) * Time.deltaTime * moveSpeed);
    }

    private void Animations()
    {
        if (isMoving)
        {
            anim.SetFloat("Horizontal", moveX);
            anim.SetFloat("Vertical", moveY);
        }
        else
        {
            anim.SetFloat("Horizontal", lastMoveDirection.x);
            anim.SetFloat("Vertical", lastMoveDirection.y);
        }

        anim.SetBool("IsMoving", isMoving);
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        anim.Play("player_attack_special");
       
        // Aguarda até a animação terminar
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        isAttacking = false;
    }

    public void AttackFinashed()
    {
        anim.SetTrigger("Attack");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isAttacking) 
        {
            Debug.Log("você acertou algo");
        }
    }
}
