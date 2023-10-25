using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveTask : MonoBehaviour
{
    public TaskUI taskUI;

    public void Remove()
    {
        taskUI.RemoveTask();
    }
}
