IF DEFINED PROGRAMFILES(x86) (
  SET "_PROGRAMFILES=%PROGRAMFILES(x86)%"
) ELSE (
  SET "_PROGRAMFILES=%PROGRAMFILES%"
)

@For /F "tokens=3,2,4 delims=/ " %%A in ('Date /t') do @( 
Set mdy=%%A%%B%%C
)

"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /cfile:"2MA00012_TXN_%mdy%.xml" /pfile:"2MA00012_POS_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Reconciliation - Wells - 2MA00012"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /cfile:"2MA00013_TXN_%mdy%.xml" /pfile:"2MA00013_POS_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Reconciliation - Wells - 2MA00013" 
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /pfile:"GARG_POS_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Reconciliation - Wells - GARG"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /pfile:"GARH_POS_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Reconciliation - Wells - GARH"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /cfile:"47881029_TXN_%mdy%.xml" /pfile:"47881029_POS_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Reconciliation - Wells - 47881029"
