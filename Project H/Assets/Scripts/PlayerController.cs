using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    Rigidbody2D rigid;
    bool isHorizontal;

    Vector3 directionRay; //Ray ����
    public float distanceRay; //Ray �Ÿ�
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


    //���� �̵� �ڵ�
    void CharacterMove()
    {
        //�ؽ�Ʈ �ڽ� Ȱ��ȭ �� �̵� ����
        if (GameManager.Instance.isAction) return;

        //�����°ſ� ���� �������� ����
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Vertical"))
            isHorizontal = true;
        else if (Input.GetButtonDown("Vertical") || Input.GetButtonUp("Horizontal") && Input.GetAxisRaw("Horizontal") == 0)
            isHorizontal = false;

        //���� �̵� ����
        Vector2 moveVec = isHorizontal ? new Vector2(Input.GetAxisRaw("Horizontal"), 0) : new Vector2(0, Input.GetAxisRaw("Vertical"));
        rigid.velocity = moveVec * speed;
    }

    //Ray ���� �ڵ�
    void InteractionRay()
    {
        //�ؽ�Ʈ �ڽ� Ȱ��ȭ �� ���� ����
        if (!GameManager.Instance.isAction)
        {
            //Ray �ٶ󺸴� ���� ����
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

        //Ray�� �����Ǵ��� �Ǵ�
        if (rayHit.collider != null)
            scanObject = rayHit.collider.gameObject; 
        else
            scanObject = null;

        //��ȣ�ۿ� �ڵ�
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            GameManager.Instance.Action(scanObject);
        }
            

    }





}
