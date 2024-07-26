namespace DeerskinSimulation.Models
{
    public class RandomEventResultDamagedSkins : EventResult
    {
        public RandomEventResultDamagedSkins(int damagedSkins) : base(
            new EventRecord($"Damaged {damagedSkins} skins while forwarding.", "red", "images/packhorse_256.jpg"),
                    originator => originator.RemoveSkins(damagedSkins))
        {
        }
    }
}
