using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    Rigidbody2D rigid;
    bool isHorizontal;

    Vector3 directionRay; //Ray 방향
    public float distanceRay; //Ray 거리
    GameObject scanObject;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
        InteractionRay();
    }


    //수직 이동 코드
    void CharacterMove()
    {
        //텍스트 박스 활성화 시 이동 제한
        if (GameManager.Instance.isAction) return;

        //누르는거에 따라 수직방향 설정
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Vertical"))
            isHorizontal = true;
        else if (Input.GetButtonDown("Vertical") || Input.GetButtonUp("Horizontal") && Input.GetAxisRaw("Horizontal") == 0)
            isHorizontal = false;

        //수직 이동 조건
        Vector2 moveVec = isHorizontal ? new Vector2(Input.GetAxisRaw("Horizontal"), 0) : new Vector2(0, Input.GetAxisRaw("Vertical"));
        rigid.velocity = moveVec * speed;
    }

    //Ray 관련 코드
    void InteractionRay()
    {
        //텍스트 박스 활성화 시 방향 고정
        if (!GameManager.Instance.isAction)
        {
            //Ray 바라보는 방향 설정
            if (!isHorizontal && Input.GetAxisRaw("Vertical") == 1)
                directionRay = Vector3.up;

            else if (!isHorizontal && Input.GetAxisRaw("Vertical") == -1)
                directionRay = Vector3.down;

            else if (isHorizontal && Input.GetAxisRaw("Horizontal") == 1)
                directionRay = Vector3.right;

            else if (isHorizontal && Input.GetAxisRaw("Horizontal") == -1)
                directionRay = Vector3.left;

        }

        //Ray
        Debug.DrawRay(rigid.position, directionRay * distanceRay, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, directionRay, distanceRay, LayerMask.GetMask("Object"));

        //Ray에 감지되는지 판단
        if (rayHit.collider != null)
            scanObject = rayHit.collider.gameObject; 
        else
            scanObject = null;

        //상호작용 코드
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            GameManager.Instance.Action(scanObject);
        }
            

    }





}
