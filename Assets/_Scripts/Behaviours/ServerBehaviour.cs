using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerBehaviour : NPCBehaviour
{
    private ServerJob currentJob;
    private bool isExecuting;
    private bool isWaitingForJob;

    public override void OnClick()
    {
        Debug.Log("Clicked on a server!");
    }
    private void Update()
    {
        if (currentJob == null && !isWaitingForJob) StartCoroutine(DoNewJob());
    }
    private IEnumerator DoNewJob()
    {
        isWaitingForJob = true;
        while(true)
        {
            currentJob = cafe.GetServerJob();
            if (currentJob != null) break;
            yield return new WaitForSeconds(1f);
        }
        yield return currentJob.Execute(this);
        currentJob = null;
        isWaitingForJob = false;
    }
}
