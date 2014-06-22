using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour
{
    //Fields
    public GameObject m_LineRendererObject;
    public string m_LibraryName;

    private GestureLibrary m_GestureLibrary;
    private List<Vector2> m_Points = new List<Vector2>();

    private LineRenderer m_LineRenderer;
    private int m_VertexCount = 0;

    private Rect m_DrawArea;

	// Use this for initialization
	void Start ()
    {
        m_GestureLibrary = new GestureLibrary(m_LibraryName);
        m_LineRenderer = m_LineRendererObject.GetComponent<LineRenderer>();
        m_DrawArea = new Rect(0, 0, Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //transform.Translate(new Vector3(0.0f, Time.deltaTime, 0.0f));

        Vector3 virtualKeyPosition = Vector3.zero;

        //Get the input
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0) virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
        }
        else
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0) || Input.GetMouseButton(0)) virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        }

        //Right click = cancel
        if (Input.GetMouseButtonUp(1))
        {

        }

        //If we are clicking inside of the drawarea
        if (m_DrawArea.Contains(virtualKeyPosition))
        {
            //if (m_SpellCharged) AimSpell(virtualKeyPosition);
            //else DrawSpell(virtualKeyPosition);
            DrawSpell(virtualKeyPosition);
        }
	}

    private void DrawSpell(Vector3 virtualKeyPosition)
    {
        //Left mouse down: Start drawing a new spell
        if (Input.GetMouseButtonDown(0))
        {
            m_Points.Clear();
            ClearLineRenderer();
        }

        //Hold left mouse button: Keep on drawing the spell
        if (Input.GetMouseButton(0))
        {
            m_Points.Add(new Vector2(virtualKeyPosition.x, -virtualKeyPosition.y));

            m_LineRenderer.SetVertexCount(++m_VertexCount);
            m_LineRenderer.SetPosition(m_VertexCount - 1, WorldCoordinateForGesturePoint(virtualKeyPosition));
        }

        //Left mouse release: Our spell is drawn, wait for a swipe to cast it!
        if (Input.GetMouseButtonUp(0))
        {
            Gesture gesture = new Gesture(m_Points);
            Result result = gesture.Recognize(m_GestureLibrary, true);

            Debug.Log(result.Name + " score: " + result.Score);

            if (result.Name == "rectangle")
            {
                //m_CurrentSpell.AddComponent<DamageSpell>();
                //m_SpellCharged = true;
            }
            else if (result.Name == "circle")
            {
                //m_CurrentSpell.AddComponent<ShieldSpell>();
                //m_SpellCharged = true;
            }
            else if (result.Name == "triangle")
            {
                //m_CurrentSpell.AddComponent<HealSpell>();
                //m_SpellCharged = true;
            }
            else
            {
                //CancelSpell();
                ClearLineRenderer();
                //m_SpellCharged = false;
            }
        }
    }

    private void ClearLineRenderer()
    {
        m_LineRenderer.SetVertexCount(0);
        m_VertexCount = 0;
    }

    private Vector3 WorldCoordinateForGesturePoint(Vector3 gesturePoint)
    {
        Vector3 worldCoordinate = new Vector3(gesturePoint.x, gesturePoint.y, 10);
        return Camera.main.ScreenToWorldPoint(worldCoordinate);
    }

}
