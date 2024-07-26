namespace DeerskinSimulation.Models
{
    public class RandomEventResultFoundSkins : EventResult
    {
        public RandomEventResultFoundSkins(int extraSkins) : base(
            new EventRecord($"Found {extraSkins} extra skins during the hunt!", 
                "green", 
                "images/good_fortune_256.jpg"),
                p => p.AddSkins(extraSkins))
        {
        }
    }
}
