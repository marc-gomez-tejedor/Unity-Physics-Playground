[System.Serializable]
public class PlayerStatus
{
    public bool OnGround;
    public bool OnSteep;
    public bool Climbing;
    public bool InWater;
    public bool Swimming;
    public int StepsSinceLastGrounded;
    public int StepsSinceLastJump;
    public float Submergence;
}
