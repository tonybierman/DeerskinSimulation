﻿using DeerskinSimulation.Resources;

namespace DeerskinSimulation.Models
{
    public class RoleExporter : ParticipantRole
    {
        public RoleExporter(string name) : base(name, Constants.ExporterStartingFunds) { }

        public EventResult Export(int numberOfSkins)
        {
            if (Skins < numberOfSkins)
            {
                return new EventResult(new EventRecord(Strings.NotEnoughSkinsToExport, "images/merchant_ship_256.jpg"));
            }

            return ExportSkins(numberOfSkins, Constants.TransatlanticTransportCost, Constants.ExportDuty, Constants.DeerSkinPrice, Constants.ExporterMarkup);
        }

        public EventResult RollForRandomExportingEvent()
        {
            return ApplyRandomExportingEvent();
        }

    }
}
