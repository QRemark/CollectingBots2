using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseTaskAssigner : MonoBehaviour
{
    private Dictionary<Resource, Unit> _activeTasks;

    public void Init(Dictionary<Resource, Unit> activeTasks)
    {
        _activeTasks = activeTasks;
    }

    public void AssignTasks(List<Unit> units, List<Resource> availableResources)
    {
        foreach (Unit unit in units)
        {
            if (unit.IsBusy == false && unit.ReadyForNewTask)
            {
                TryAssignTaskToUnit(unit, availableResources);
            }
        }
    }

    private void TryAssignTaskToUnit(Unit unit, List<Resource> availableResources)
    {
        Resource closest = availableResources
            .Where(resourse => resourse != null && resourse.IsAvailable && _activeTasks.ContainsKey(resourse) == false)
            .OrderBy(resource => Vector3.Distance(unit.transform.position, resource.transform.position))
            .FirstOrDefault();

        if (closest == null) 
            return;

        bool accepted = unit.SetTarget(closest);

        if (accepted)
        {
            _activeTasks[closest] = unit;
        }
    }
}