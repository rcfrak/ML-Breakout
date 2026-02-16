// This boiler plate is intended to be used as a template for generating a paddle agent
// It is adapted from this tutorial: https://www.youtube.com/watch?v=rMZi1nt0pBw&t=140s
// Needs ML Agents Python back end to execute

using Unity.MLAgents;
using Unity.MLAgents.Actuators; //May not be needed
using Unity.MLAgents.Sensors; //May not be needed
using UnityEngine;

// IMPORTANT - after changing the name of the class (i.e. AgentBoilerPlate), make sure to rename
// the CS file to have the same name as the class
public class AgentBoilerPlate : Agent
{
    private int _currentEpisode = 0;
    private float _cumulativeReward = 0f;
    public override void Initialize()
    {
        Debug.Log("Initialize()");
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("OnEpisodeBegin()");

        _currentEpisode++;
        _cumulativeReward = 0f;

        // If something else needs to be reset within the Agent class when an episode begins
        // like spawning bricks (might be handled elsewhere?) a function call would go here
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float paddlePosX_normalized = transform.localPosition.x; // divide by stage width to normalize;

        sensor.AddObservation(paddlePosX_normalized);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // move the agent using the action
        MoveAgent(actions.DiscreteActions);

        AddReward(0); // Placeholder, if you want a small penalty for moving for example

        _cumulativeReward = GetCumulativeReward();
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var action = act[0];

        switch (action)
        {
            case 1:
                //move Left?
                break;
            case 2:
                //move Right?
                break;
        }
    }

    /* Skeleton for creating collision based logic for rewarding the agent
     
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(" "))
        {
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(" "))
        {

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(" "))
        {
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(" "))
        {
            
        }
    }
    */
}
