using UnityEngine;
using System;

public class Win_Screen : MonoBehaviour
{
    [SerializeField] private string finishTag = "Finish";

    private double startTime;
    private double completionTime;
    private bool hasWon;
    private Movement movement;
    private Rigidbody2D body;
    private GUIStyle titleStyle;
    private GUIStyle timeStyle;

    private void Awake()
    {
        startTime = Time.timeAsDouble;
        movement = GetComponent<Movement>();
        body = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryWin(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryWin(other.gameObject);
    }

    private void TryWin(GameObject other)
    {
        if (hasWon || other.tag != finishTag)
        {
            return;
        }

        hasWon = true;
        completionTime = Time.timeAsDouble;

        if (movement != null)
        {
            movement.enabled = false;
        }

        if (body != null)
        {
            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0f;
            body.simulated = false;
        }
    }

    private void OnGUI()
    {
        if (!hasWon)
        {
            return;
        }

        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 32,
                fontStyle = FontStyle.Bold
            };
            timeStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 22
            };
        }

        float width = Mathf.Min(420f, Screen.width - 40f);
        float height = 180f;
        Rect panel = new Rect((Screen.width - width) / 2f, (Screen.height - height) / 2f, width, height);
        GUI.Box(panel, GUIContent.none);
        GUI.Label(new Rect(panel.x, panel.y + 20f, panel.width, 55f), "You Win!", titleStyle);

        TimeSpan elapsed = TimeSpan.FromSeconds(completionTime - startTime);
        GUI.Label(
            new Rect(panel.x, panel.y + 80f, panel.width, 60f),
            $"Completion Time: {elapsed.Minutes:00}:{elapsed.Seconds:00}.{elapsed.Milliseconds:000}",
            timeStyle);
    }
}
