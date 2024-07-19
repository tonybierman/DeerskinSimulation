# DeerskinSimulation

### Overview

**Objective**: Blazor WebAssembly simulation of the deerskin economy in the 1700s, focusing on the roles of hunters, traders, and exporters.

### Design Specifications
1. **Separation of Logic from Presentation**: Console input/output is handled separately from the business logic.
2. **Constants in a Dedicated Class**: Centralize all constant values.
3. **Business Logic in a Dedicated Class**: Separate business logic into its own class.
4. **Scalable and Object-Oriented Architecture**: Use an object-oriented approach for scalability and maintainability.

### Simulation Rules
1. **Hunter**: Incurs costs and risks of hunting and logistics to the store.
2. **Trader**: Incurs costs and risks of regional transportation to seaside ports.
3. **Exporter**: Incurs costs and risks of transatlantic logistics.
4. **Consolidation**: Traders and exporters benefit from consolidating efforts.
5. **Duties**: Exporters pay duties to the colonial government.
6. **Proportionate Risk and Reward**: All rewards come with proportional risks.
7. **User Interactions**: Users can risk money on hunting trips and accumulate skins.
8. **Selling Skins**: Users can sell fewer than 10,000 skins to a frontier trader or transport 10,000+ skins to a seaside exporter.

### Components
- **Constants**: Holds constant values.
- **Hunter**: Manages hunting activities and selling skins to traders.
- **Trader**: Manages buying skins from hunters and transporting them to exporters.
- **Exporter**: Manages receiving skins from traders and exporting them across the Atlantic.

### Blazor Components
1. **Home.razor**: Home page with navigation to start the simulation.
2. **Simulation.razor**: Main simulation page where users can perform actions (hunt, sell to trader, transport to exporter, export skins).
3. **NavMenu.razor**: Navigation menu to navigate between home and simulation pages.
4. **MainLayout.razor**: Main layout of the application.

### Implementation
- **Models**: Define `Hunter`, `Trader`, `Exporter`, and `Constants`.
- **Blazor Components**: Implement user actions and interactions in Blazor components.

### Key Points
- **Hunter Class**: Handles hunting and selling skins to traders.
- **Trader Class**: Handles buying skins from hunters and transporting them to exporters.
- **Exporter Class**: Handles receiving skins and exporting them.
- **Blazor Setup**: Create a Blazor WebAssembly project and define components for simulation actions.

This setup ensures a clear separation of concerns, scalable architecture, and user-friendly interaction through Blazor components.

### Testing and code coverage analysis
- dotnet test --collect:"XPlat Code Coverage"
- reportgenerator -reports:TestResults/**/*.xml -targetdir:CoverageReport -reporttypes:Html
