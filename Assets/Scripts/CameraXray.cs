using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraXray : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private List<XrayObject> currentXrayObjects = new List<XrayObject>();
    [SerializeField] private List<XrayObject> alreadyXrayTransperant = new List<XrayObject>();
    void FixedUpdate()
    {
        XRay();

        MakeObjectsSolid();
        MakeObjectsTransperant();
    }

    private void XRay()
    {
        currentXrayObjects.Clear(); 

        float cameraPlayerDistance = Vector3.Magnitude(transform.position - Player.position);
        Ray ray_forward = new Ray(transform.position, Player.position - transform.position);

        var hits = Physics.RaycastAll(ray_forward, cameraPlayerDistance);

        foreach(var hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent(out XrayObject obj))
            {
                if (!currentXrayObjects.Contains(obj))
                {
                    currentXrayObjects.Add(obj);
                }
            }
        }
    }

    private void MakeObjectsTransperant()
    {
        for(int i = 0; i < currentXrayObjects.Count; i++)
        {
            XrayObject obj = currentXrayObjects[i];

            if (!alreadyXrayTransperant.Contains(obj))
            {
                obj.ShowTranperant();
                alreadyXrayTransperant.Add(obj);
            }
        }
    }

    private void MakeObjectsSolid()
    {
        for (int i = 0; i < alreadyXrayTransperant.Count; i++)
        {
            XrayObject obj = alreadyXrayTransperant[i];

            if (!currentXrayObjects.Contains(obj))
            {
                obj.ShowSolid();
                alreadyXrayTransperant.Remove(obj);
            }
        }
    }
}
