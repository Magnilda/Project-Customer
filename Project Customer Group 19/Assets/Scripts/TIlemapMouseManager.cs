using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIlemapMouseManager : MonoBehaviour
{
    //======================================================
    //                      Update()
    //======================================================
    void Update()
    {
        mouseHover();
    }

    //======================================================
    //                     mouseHover()
    //======================================================
    private void mouseHover()
    {
        //Debug.Log(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit entryPoint = new RaycastHit();

        if (Physics.Raycast(ray, out entryPoint))
        {
            GameObject hitObject = entryPoint.collider.transform.gameObject;
            //Debug.Log("Raycast hit: " + hitObject.name);

            if (Input.GetMouseButtonDown(0))
            {
                hitObject.transform.parent.GetComponent<Tile>().ChangeColor(Color.blue);
                hitObject.transform.parent.GetComponent<Tile>().ChangeNeighboursColor(Color.red);
                Debug.Log(hitObject.transform.parent.GetComponent<Tile>().Type);
            }
        }
    }
}
