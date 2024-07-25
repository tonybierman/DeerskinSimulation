using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeerskinSimulation.Tests
{
    using DeerskinSimulation.Models;
    using System;
    using Xunit;

    public class StateContainerTests
    {
        [Fact]
        public void Debug_DefaultValue_ShouldBeFalse()
        {
            // Arrange
            var stateContainer = new StateContainer();

            // Act
            var debugValue = stateContainer.Debug;

            // Assert
            Assert.False(debugValue);
        }

        [Fact]
        public void Debug_SetValue_ShouldUpdateValue()
        {
            // Arrange
            var stateContainer = new StateContainer();

            // Act
            stateContainer.Debug = true;

            // Assert
            Assert.True(stateContainer.Debug);
        }

        [Fact]
        public void Debug_SetValue_ShouldTriggerOnChange()
        {
            // Arrange
            var stateContainer = new StateContainer();
            bool eventTriggered = false;
            stateContainer.OnChange += () => eventTriggered = true;

            // Act
            stateContainer.Debug = true;

            // Assert
            Assert.True(eventTriggered);
        }

        [Fact]
        public void Debug_SetSameValue_ShouldNotTriggerOnChange()
        {
            // Arrange
            var stateContainer = new StateContainer();
            bool eventTriggered = false;
            stateContainer.Debug = true;
            stateContainer.OnChange += () => eventTriggered = true;

            // Act
            stateContainer.Debug = false; // Set to a different value first
            eventTriggered = false; // Reset the flag
            stateContainer.Debug = false; // Now set it to the same value again

            // Assert
            Assert.False(eventTriggered);
        }

        [Fact]
        public void Debug_SetNullValue_ShouldNotThrow()
        {
            // Arrange
            var stateContainer = new StateContainer();

            // Act
            var exception = Record.Exception(() => stateContainer.Debug = null);

            // Assert
            Assert.Null(exception);
            Assert.False(stateContainer.Debug); // Should return the default false value
        }
    }


}
