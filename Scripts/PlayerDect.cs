using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDect : MonoBehaviour
{
    [field: SerializeField]
    public bool PlayerDeteted { get; private set; }
    public Vector2 DirectionToTarget => target.transform.position - detetorOrigin.position;

    [Header("overlapbox Para")]
    [SerializeField]
    private Transform detetorOrigin;
    public Vector2 detetorSize = Vector2.one;
    public Vector2 detetorOriginOffset = Vector2.one;

    public float deteletionDelay = 0.3f;
    public LayerMask detetorLayermask;

    [Header("GIsmos Para")]
    public Color GizmoIdle = Color.green;
    public Color Gizmodeteted = Color.red;
    public bool showGismos = true;

    private GameObject target;

    public GameObject Target
    {
        get => target;
        private set
        {
            target = value;
            PlayerDeteted = target != null;
        }
    }

    private void Start()
    {
        detetorOrigin = this.transform;
        StartCoroutine(DetetionCoroutine());
    }
    IEnumerator DetetionCoroutine()
    {
        yield return new WaitForSeconds(deteletionDelay);
        PerformDetetion();
        StartCoroutine(DetetionCoroutine());
    }
    public void PerformDetetion()
    {
        Collider2D collider = Physics2D.OverlapBox(
            (Vector2)detetorOrigin.position + detetorOriginOffset,
             detetorSize, 0, detetorLayermask);

        Debug.Log(collider.name);
        if (collider != null)
        {
            Target = collider.gameObject;
        }
        else
        {
            Target = null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoIdle;
        if (PlayerDeteted)
            Gizmos.color = Gizmodeteted;
        Gizmos.DrawCube((Vector2)detetorOrigin.position + detetorOriginOffset, detetorSize);

    }
}