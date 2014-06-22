using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileBehaviour : MonoBehaviour
{
    public List<Vector2> Path
    {
        private get { return m_Path; }

        set
        {
            m_Path = value;

            m_StartTime = Time.time;
            m_CurrentId = 0;
        }
    }

    private List<Vector2> m_Path;

    private int m_CurrentId = 0;
    private float m_Speed = 10000.0f;
    private float m_StartTime;

	void Update ()
    {
        if (Path.Count == 0 || m_CurrentId >= Path.Count - 1) return;

        float pathLenght = Vector2.Distance(Path[m_CurrentId], Path[m_CurrentId + 1]);

        float distCovered = (Time.time - m_StartTime) * m_Speed * Time.deltaTime;
        float fracJourney = distCovered / pathLenght;

        transform.position = Vector2.Lerp(Path[m_CurrentId], Path[m_CurrentId + 1], fracJourney);

        //Move on to the next node when we get really close
        if (fracJourney > 1.0f)
        {
            m_StartTime = Time.time;
            ++m_CurrentId;
        }
	}
}
