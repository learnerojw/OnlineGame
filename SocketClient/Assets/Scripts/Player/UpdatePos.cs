using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePos : MonoBehaviour
{
    private UpdatePosRequest updatePosRequest;

    private Transform gunTr;

    private void Start()
    {
        updatePosRequest = GetComponent<UpdatePosRequest>();
        gunTr = transform.Find("Gun");
        InvokeRepeating("UpPosFun", 1, 1f / 30f);
    }

    private void UpPosFun()
    {
        Vector2 pos = transform.position;
        float playerRotZ = transform.eulerAngles.z;
        float gunRotZ = gunTr.eulerAngles.z;
        updatePosRequest.SendRequest(pos, playerRotZ, gunRotZ);
    }
}
