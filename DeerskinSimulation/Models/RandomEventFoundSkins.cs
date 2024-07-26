namespace DeerskinSimulation.Models
{
    public class RandomEventFoundSkins : EventResult
    {
        public RandomEventFoundSkins(int extraSkins) : base(
            new EventRecord("Found extra skins during the hunt!", 
                "green", 
                "images/good_fortune_256.jpg"),
                p => p.AddSkins(extraSkins))
        { }
    }
}
