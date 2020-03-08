using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Turret))]
public class TurretEditor : Editor
{
    private Turret turret = null;

    private void OnEnable()
    {
        turret = target as Turret;
    }

    private void OnSceneGUI()
    {
        Vector3 forward = Quaternion.AngleAxis(-turret.MaxAngle / 2f, Vector3.up) * Vector3.forward;

        Color c = Color.blue;
        c.a = 0.5f;

        Handles.color = c;
        Handles.DrawSolidArc(turret.transform.position, Vector3.up, forward, turret.MaxAngle, 5);
    }
}
