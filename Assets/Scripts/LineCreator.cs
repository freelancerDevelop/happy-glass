using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject Pencil;
    public StarSlider starSlider;
    float pencilRotateStep = 3f,lengthActiveLine=0;
    GameObject activeLine;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            activeLine = Instantiate(linePrefab);
            Pencil.GetComponent<SpriteRenderer>().DOFade(1, 0.3f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Line Length: "+lengthActiveLine);
            lengthActiveLine = 0;
            AddCollider();
            activeLine = null;
            Pencil.GetComponent<SpriteRenderer>().DOFade(0, 0.3f);
        }

        if (activeLine != null)
        {
            UpdateLine(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

    }
    void AddCollider()
    {
        GetComponent<LineRenderer>().enabled = false;
        PolygonCollider2D pg = activeLine.GetComponent<PolygonCollider2D>();
        LineRenderer lr = activeLine.GetComponent<LineRenderer>();
        if (lr.positionCount >= 2)
        {
            for (int i = 1; i < lr.positionCount; i++)
            {
                Vector2 p1 = lr.GetPosition(i - 1);
                Vector2 p2 = lr.GetPosition(i);
                Vector2 u = (p2 - p1).normalized * 0.05f;
                Vector2 n = new Vector2(u.y, -u.x);
                Vector2[] rect = new Vector2[4];
                rect[0] = p1 + n;
                rect[1] = p1 - n;
                rect[2] = p2 - n;
                rect[3] = p2 + n;
                pg.pathCount++;
                pg.SetPath(pg.pathCount - 1, rect);
            }
            activeLine.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        else Destroy(activeLine);
        
    }
    void UpdateLine(Vector2 point)
    {
        LineRenderer lr = activeLine.GetComponent<LineRenderer>();
        if (lr.positionCount == 0)
        {

            lr.positionCount++;
            lr.SetPosition(lr.positionCount - 1, point);
            PencilRotation(point);
        }
        else 
        if (Vector2.Distance(point, lr.GetPosition(lr.positionCount - 1)) > 0.15f){
            RaycastHit2D hit = Physics2D.Raycast(lr.GetPosition(lr.positionCount - 1), point - (Vector2)lr.GetPosition(lr.positionCount - 1),Vector2.Distance(point,lr.GetPosition(lr.positionCount - 1)));
            if (hit.collider != null&& hit.collider.gameObject.layer!=LayerMask.NameToLayer("Liquid"))
            {
                GetComponent<LineRenderer>().enabled = true;
                GetComponent<LineRenderer>().SetPosition(0, lr.GetPosition(lr.positionCount - 1));
                GetComponent<LineRenderer>().SetPosition(1, point);
                PencilRotation(point);
            }
            else
            {
                starSlider.Drawed(Vector2.Distance(point, lr.GetPosition(lr.positionCount - 1)));
                lengthActiveLine += Vector2.Distance(point, lr.GetPosition(lr.positionCount - 1));
                GetComponent<LineRenderer>().enabled = false;
                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, point);
                PencilRotation(point);
            }
        };
        
    }
    void PencilRotation(Vector2 point)
    {
        Pencil.transform.position = point;
        Vector3 pencilAngle = Pencil.transform.localEulerAngles;
        if (pencilAngle.z < 330f || pencilAngle.z > 340) pencilRotateStep = -pencilRotateStep;
        pencilAngle.z += pencilRotateStep;
        Pencil.transform.localEulerAngles = pencilAngle;
    }

}