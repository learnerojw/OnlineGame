using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed;
    public DamageRequest damageRequest;
    //�жϸ��ӵ��Ƿ��ǵ�ǰ�ͻ��˷����
    public bool isLocal = false;
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime) ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //����һ��Ҫ�жϸ��ӵ����Լ�����Ļ��Ƕ��֣������ظ�ִ���˺�����
        if(collision.CompareTag("Player")&&collision.gameObject!=this.gameObject&& isLocal)
        {
            PlayerInfo playerInfo = collision.GetComponent<PlayerInfo>();
            damageRequest.SendRequest(transform.position.x, transform.position.y, transform.right.x,transform.right.y,playerInfo.userName);
        }
        Destroy(gameObject);
        Debug.Log("�ӵ�����");
    }
}
