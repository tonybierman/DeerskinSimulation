using DeerskinSimulation.Commands;

namespace DeerskinSimulation.Models
{
    public class RandomEventResultLostSkins : EventResult
    {
        public RandomEventResultLostSkins(int lostSkins) : base(
            new EventRecord($"Lost {lostSkins} skins due to bad weather.", "red", "images/bad_fortune_256.jpg"), 
                (p) =>
                {
                    p.RemoveSkins(lostSkins);
                    var h = p as ParticipantRole;
                    h.CurrentBag -= lostSkins;
                }
            )
        {
        }
    }
}
