using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastTarget : MonoBehaviour
{
    [SerializeField] UnityEvent<Obstacle> OnSellectionOBS;
    [SerializeField] UnityEvent<Obstacle> DeSellectionOBS;
    [SerializeField] LayerMask layerObs, layerIgnoreRaycast;

    Obstacle obsSellection;
    Obstacle obsTrigger;

    private Vector3 boundSizeObs;

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerObs))
            {
                if (hit.transform.TryGetComponent<Obstacle>(out Obstacle obstacle))
                {
                    OnSellectionOBS?.Invoke(obstacle);
                    OnSellection(obstacle);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            DeSellectionOBS?.Invoke(obsSellection);
            DeSellection();
            OnTriggerExitObstarget();
        }
        if (Input.GetMouseButton(0)) 
        {
            if(Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, ~layerIgnoreRaycast))
            {
                if (obsSellection is not null)
                {
                    obsSellection.transform.DOMove(GetMouseWoldPosDragObs(hit.point), 0.1f);
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                    {
                        OnTriggerEnterObstarget(hit.transform.GetComponent<Obstacle>());
                    }
                    else
                        OnTriggerExitObstarget();
                }
            }
        }
        
    }
    void OnSellection(Obstacle obs)
    {
        if(obsSellection is null)
        {
            obsSellection = obs;
            boundSizeObs = obs.size;
            int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
            obs.gameObject.layer = LayerIgnoreRaycast;
            obs.EnableOutLine();
        }
    }
    void DeSellection()
    {
        if (obsSellection is not null)
        {
            obsSellection.DisableOutLine();
            int LayerObstacle = LayerMask.NameToLayer("Obstacle");
            obsSellection.gameObject.layer = LayerObstacle;
            obsSellection = null;
        }
    }
    Vector3 GetMouseWoldPosDragObs(Vector3 point)
    {
        float xClampMax = 10f, xClampMin = -10f;
        float zClampMax = 5f, zClampMin = -2f;

        float xBounds = boundSizeObs.x / 2;
        float yBounds = boundSizeObs.y / 2;
        float zBounds = boundSizeObs.z / 2;
        return new Vector3(
            Mathf.Clamp(point.x, xClampMin + xBounds, xClampMax - xBounds),
            point.y,
            Mathf.Clamp(point.z, zClampMin + zBounds, zClampMax - zBounds * 2)
            ) + Vector3.up * (yBounds + 1) + Vector3.forward * zBounds;
    }

    void OnTriggerEnterObstarget(Obstacle obs) 
    {
        if(obsTrigger == null)
        {
            obsTrigger = obs;
            obs.EnableOutLine();
        }
        else if(obs != obsTrigger)
        {
            obsTrigger.DisableOutLine();
            obsTrigger = obs;
            obs.EnableOutLine();
        }
    }
    void OnTriggerExitObstarget()
    {
        if(obsTrigger is not null)
        {
            obsTrigger.DisableOutLine();
            obsTrigger = null;
        }
    }
}
