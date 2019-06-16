using System.Diagnostics.Contracts;
using UnityEngine.Assertions;

public class JobCounter
{
    public bool IsCompleted => _completePermission && _jobCount == 0;

    private int _jobCount;
    private bool _completePermission;

    public JobCounter(bool waitForPermission)
    {
        _completePermission = !waitForPermission;
    }

    public void PermitCompletion()
    {
        _completePermission = true;
    }

    public void JobStarted()
    {
        _jobCount++;
    }

    public void JobFinished()
    {
        _jobCount--;
    }

    [ContractInvariantMethod]
    private void _Invariant()
    {
        Assert.IsFalse(_jobCount < 0, "We got negative job count somehow boss, something is wrong here.");
    }
}
