using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Update is called once per frame
    private Transform fireTr;
    private FireRequest fireRequest;
    private DamageRequest damageRequest;
    private void Start()
    {
        fireTr = transform.GetChild(0);
        fireRequest = transform.AddComponent<FireRequest>();
        damageRequest = transform.AddComponent<DamageRequest>();
    }
    void Update()
    {
        LookAt();
        Fire();
    }

    private void LookAt()
    {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angel = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation=Quaternion.AngleAxis(angel, Vector3.forward);
    }

    private void Fire()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angel = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            GameObject bulletObj=ResMgr.GetInstance().Load<GameObject>("Prefab/Bullet");
            bulletObj.transform.position = fireTr.position;
            bulletObj.transform.rotation = Quaternion.AngleAxis(angel, Vector3.forward);
            fireRequest.SendRequest(bulletObj.transform.position, bulletObj.transform.eulerAngles.z);

            bulletObj.GetComponent<Bullet>().damageRequest = damageRequest;
            bulletObj.GetComponent<Bullet>().isLocal = true;
        }
    }
}
