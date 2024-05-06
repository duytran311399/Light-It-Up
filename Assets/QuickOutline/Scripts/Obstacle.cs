using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Outline outline;
    public Vector3 size;
    private MeshRenderer renderer;
    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        size = renderer.bounds.size;
        Debug.Log(size);
        outline = GetComponent<Outline>();
    }
    private void Start()
    {
        DisableOutLine();
    }
    internal void EnableOutLine() => outline.OutlineWidth = 10;
    internal void DisableOutLine() { outline.OutlineWidth = 0; DropToGround(); }

    void DropToGround()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            transform.DOMoveY(hit.point.y + 0.5f, 0.5f);
        }
    }
}
