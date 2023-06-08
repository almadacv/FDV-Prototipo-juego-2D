using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MovPlata : MonoBehaviour
{
    public Transform[] Objetivos;
    public float MovementSpeed = 1.0f;
    public float AcceptanceRadius = 1.0f;
    public bool IsEnabled = true;

    private int _currentGoalIdx = 0;

    void Update()
    {
        if (!IsEnabled)
        {
            return;
        }

        if (_currentGoalIdx < 0 || Objetivos.Length <= _currentGoalIdx)
        {
            return;
        }
        Transform currentGoal = Objetivos[_currentGoalIdx];
        Vector3 currentToGoal = currentGoal.position - transform.position;
        if (currentToGoal.magnitude > AcceptanceRadius)
        {
            transform.Translate(MovementSpeed * Time.deltaTime * currentToGoal.normalized);
        }
        else
        {
            _currentGoalIdx = (_currentGoalIdx + 1) % Objetivos.Length;
        }
    }

    public void ToggleEnabled()
    {
        IsEnabled = !IsEnabled;
    }

    public void SetEnabled(bool inIsEnabled)
    {
        IsEnabled = inIsEnabled;
    }
}
