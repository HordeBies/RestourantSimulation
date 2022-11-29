using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeCustomerJob : ServerJob
{
    CustomerBehaviour customer;
    public ServeCustomerJob(CustomerBehaviour customer)
    {
        this.customer = customer;
    }
    public override IEnumerator Execute(ServerBehaviour server)
    {
        base.Execute(server);

        ServingTableBehaviour servingTable = null;
        yield return new WaitUntil(()=>cafe.GetServingTableForCustomerMeal(out servingTable));
        cafe.TryMove(server, servingTable.placedObject);
        Debug.Log("Moving To Serving Table!");
        yield return new WaitUntil(() => server.HasReachedDestination());
        Meal servedMeal = servingTable.Serve();
        cafe.TryMove(server, customer.assignedChair.placedObject);
        Debug.Log("Moving To Customer Table!");
        yield return new WaitUntil(() => server.HasReachedDestination());
        customer.Serve(servedMeal);
        yield return null;
    }
}
