using DeerskinSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeerskinSimulation.Tests
{
    public class MockParticipant : Participant
    {
        public MockParticipant(string name) : base(name, 0) { }

    }
}
