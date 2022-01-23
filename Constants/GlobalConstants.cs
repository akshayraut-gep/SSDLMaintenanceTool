using SSDLMaintenanceTool.Models;
using System.Collections.Generic;

namespace SSDLMaintenanceTool.Constants
{
    public class GlobalConstants
    {
        public List<EnvironmentSetting> Environments { get; set; }
        public const string QueryExecutionerDomainExportDirectory = @"QueryExecutioner\DomainExport";
        public const string QueryExecutionerQueryResultDirectory = @"QueryExecutioner\QueryResult";
        public const string PublishPredefinedQueriesMigration = "PublishPredefinedQueriesMigration";
        public const string GeneralQueries = "GeneralQueries";

        public GlobalConstants()
        {
            this.Environments = new List<EnvironmentSetting>();

            var devEnvironment = new EnvironmentSetting();
            devEnvironment.Name = "Dev";
            devEnvironment.ConfigSettingName = "DevSmartConfiguration";
            this.Environments.Add(devEnvironment);

            devEnvironment.Name = "Dev";
            devEnvironment.ConfigSettingName = "DevSmartConfiguration";
            this.Environments.Add(devEnvironment);

            var qcEnvironment = new EnvironmentSetting();
            qcEnvironment.Name = "QC Smart Config";
            qcEnvironment.ConfigSettingName = "QCSmartConfiguration";
            this.Environments.Add(qcEnvironment);

            var uatEnvironment = new EnvironmentSetting();
            uatEnvironment.Name = "UAT Smart Config";
            uatEnvironment.ConfigSettingName = "UATSmartConfiguration";
            this.Environments.Add(uatEnvironment);

            var prodEnvironment = new EnvironmentSetting();
            prodEnvironment.Name = "GEP Smart Config";
            prodEnvironment.ConfigSettingName = "GEPSmartConfiguration";
            this.Environments.Add(prodEnvironment);
        }
    }
}
