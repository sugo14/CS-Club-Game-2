using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HealhBar : MonoBehaviour
{
    public GameObject healthObject;
    public Gradient HealthColourGradient;

    public void setFill(float fillRatio)
    {
        healthObject.GetComponent<SpriteRenderer>().color = HealthColourGradient.Evaluate(fillRatio);
        Vector2 oldPos = healthObject.transform.localPosition;
        healthObject.transform.localPosition = new Vector2(-0.5f + fillRatio * 0.5f, 0);
        healthObject.transform.localScale = new Vector3(fillRatio, 1, 1);
    }
}
