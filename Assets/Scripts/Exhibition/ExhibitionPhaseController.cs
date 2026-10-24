using UnityEngine;

public class ExhibitionPhaseController : MonoBehaviour, IGamePhaseController
{
    
    //TIME CONTROL
    [SerializeField] private SimulationClock simulationClock;
    // press Space to toggle pause/resume

    //INPUT CONTROL

    //ENTITY SPAWNING

    //GLOBAL EVENT

    //SAVE/LOAD

    //DATA SUM

    //DEBUG


    void Start()
    {
        MainManager.Instance.RegisterPhaseController(this);
    }

    public void EnterPhase()
    {
        Debug.Log("Entering Exhibition Phase");
        // Initialize the exhibition phase, e.g., set up the scene, start simulation clock, etc.
    }

    public void ExitPhase()
    {
        Debug.Log("Exiting Exhibition Phase");
        // Clean up the exhibition phase, e.g., stop simulation clock, save state, etc.
    }
}
