namespace DeerskinSimulation.Models
{
    public class RandomEventLostSkins : EventResult
    {
        public RandomEventLostSkins(int lostSkins) : base(new EventRecord("Lost some skins due to bad weather.", 
            "red", 
            "images/bad_fortune_256.jpg"),
            p => p.RemoveSkins(lostSkins))
        { }
    }
}
