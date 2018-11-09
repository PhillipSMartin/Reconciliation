using System;
using Gargoyle.Utilities.CommandLine;

namespace ReconciliationConsole
{
    /// <summary>
    /// Summary description for CommandLineParameters.
    /// </summary>
    public class CommandLineParameters
    {

        [CommandLineArgumentAttribute(CommandLineArgumentType.Required, ShortName = "a", Description = "Action for this execution")]
        public ReconciliationAction Action = ReconciliationAction.None;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "tname", Description = "Task name for reporting completion")]
        public string TaskName;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "date", Description = "Import date - defaults to today")]
        public string ImportDate;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "liq", Description = "Connection string for liquid database")]
        public string LiquidConnectionString;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "dir", Description = "Directory of files to import")]
        public string Directory;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "pfile", Description = "Name of position file to import")]
        public string PositionFileName;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "cfile", Description = "Name of confirmation file to import")]
        public string ConfirmationFileName;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "tfile", Description = "Name of taxlots file to import")]
        public string TaxlotsFileName;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "bfile", Description = "Name of bookkeeping file to import")]
        public string BookkeepingFileName;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "dfile", Description = "Name of dividends file to import")]
        public string DividendsFileName;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "trfile", Description = "Name of trade file to import")]
        public string TradeFileName;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "o", Description = "If true, import should overwrite existing records")]
        public bool Overwrite;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "ag", Description = "Account group name - null for all accounts")]
        public string AccountGroupName;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "tdr", Description = "Trader name used to add trades to Hugo")]
        public string TraderName;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "ch", Description = "Clearing house - needed for format of input files")]
        public ReconciliationLib.ClearingHouse ClearingHouse = ReconciliationLib.ClearingHouse.None;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "acctid", Description = "Acctid - needed only for ConstructTaxlotsFile action")]
        public int AccountId = -1;

        public CommandLineParameters()
        {
        }

        public DateTime? GetImportDateOverride()
        {
            if (string.IsNullOrEmpty(ImportDate))
                return null;
            else
            {
                DateTime importDate;
                if (DateTime.TryParse(ImportDate, out importDate))
                    return importDate;
                else
                    throw new ReconciliationLib.ReconciliationConversionException(
                        string.Format("Cannot convert {0} to a date", ImportDate));
            }
        }
    }
}
