using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed;
    public DamageRequest damageRequest;
    //判断该子弹是否是当前客户端发射的
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
        //这里一定要判断该子弹是自己发射的还是对手，避免重复执行伤害请求
        if(collision.CompareTag("Player")&&collision.gameObject!=this.gameObject&& isLocal)
        {
            PlayerInfo playerInfo = collision.GetComponent<PlayerInfo>();
            damageRequest.SendRequest(transform.position.x, transform.position.y, transform.right.x,transform.right.y,playerInfo.userName);
        }
        Destroy(gameObject);
        Debug.Log("子弹销毁");
    }
}
