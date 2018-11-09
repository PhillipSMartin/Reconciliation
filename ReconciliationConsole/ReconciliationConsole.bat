IF DEFINED PROGRAMFILES(x86) (
  SET "_PROGRAMFILES=%PROGRAMFILES(x86)%"
) ELSE (
  SET "_PROGRAMFILES=%PROGRAMFILES%"
)

@For /F "tokens=3,2,4 delims=/ " %%A in ('Date /t') do @( 
Set mdy=%%A%%B%%C
Set ymd=%%C%%A%%B
)

"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /cfile:"2MA00012_TXN_%mdy%.xml" /pfile:"2MA00012_POS_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Reconciliation - Wells - 2MA00012"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /cfile:"2MA00013_TXN_%mdy%.xml" /pfile:"2MA00013_POS_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Reconciliation - Wells - 2MA00013" 
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /pfile:"GARG_POS_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Reconciliation - Wells - GARG"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /pfile:"GARH_POS_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Reconciliation - Wells - GARH"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /cfile:"47881467_TXN_%mdy%.xml" /pfile:"47881467_POS_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Reconciliation - Wells - 47881467"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /dfile:"2MA00012_DIVRECON_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Dividends - Wells - 2MA00012"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /dfile:"2MA00013_DIVRECON_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Dividends - Wells - 2MA00013"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /dfile:"GARG_DIVRECON_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Dividends - Wells - GARG" 
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /dfile:"GARH_DIVRECON_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Dividends - Wells - GARH" 
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\MorganStanley\Inbound /cfile:"TradeConfirms.%ymd%.csv" /pfile:"Gargoyle-ListedOptionPositionExtracts.%ymd%.csv" /a:import /ch:MorganStanley /tname:"Reconciliation - Morgan"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\BNY /cfile:"Custody_Confirmations_%ymd%.csv" /pfile:"Custody_Valuation_%ymd%.csv" /a:import /ch:BONY  /tname:"Reconciliation - BONY"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\IB /cfile:"F1957488.Trades.%ymd%.csv" /pfile:"F1957488.Positions.%ymd%.csv" /bfile:"F1957488.Transfers.%ymd%.csv" /dfile:"F1957488.Corporate_Actions.%ymd%.csv" /a:import /ch:IB  /tname:"Reconciliation - IB"
