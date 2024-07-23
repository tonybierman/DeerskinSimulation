namespace DeerskinSimulation.Models
{
    public interface IRandomEventStrategy
    {
        //EventRecord ApplyEvent(Participant participant);
        EventResult ApplyEvent(Participant participant);
    }
}
