using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFadeObject : MonoBehaviour
{
    public Transform player;
    public float radius = 15;
    void Update()
    {
        // Player는 싱글톤이기에 전역적으로 접근할 수 있습니다.
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, direction, Vector3.Distance(transform.position, player.position),
                            1 << LayerMask.NameToLayer("EnvironmentObject"));
       
        for (int i = 0; i < hits.Length; i++)
        {
            TransparentObject[] obj = hits[i].transform.GetComponentsInChildren<TransparentObject>();

            for (int j = 0; j < obj.Length; j++)
            {
                obj[j]?.BecomeTransparent();
            }
        }
    }
}
