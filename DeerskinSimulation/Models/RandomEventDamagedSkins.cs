namespace DeerskinSimulation.Models
{
    public class RandomEventDamagedSkins : EventResult
    {
        public RandomEventDamagedSkins(int damagedSkins) : base(
            new EventRecord($"Damaged {damagedSkins} skins while forwarding.", "red", "images/packhorse_256.jpg"),
                    originator => originator.RemoveSkins(damagedSkins))
        {
        }
    }
}
