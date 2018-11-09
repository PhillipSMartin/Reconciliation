IF DEFINED PROGRAMFILES(x86) (
  SET "_PROGRAMFILES=%PROGRAMFILES(x86)%"
) ELSE (
  SET "_PROGRAMFILES=%PROGRAMFILES%"
)

@For /F "tokens=3,2,4 delims=/ " %%A in ('Date /t') do @( 
Set mdy=%%A%%B%%C
)

"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /dfile:"2MA00012_DIVRECON_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Dividends - Wells - 2MA00012"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /dfile:"2MA00013_DIVRECON_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Dividends - Wells - 2MA00013"
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /dfile:"GARG_DIVRECON_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Dividends - Wells - GARG" 
"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\WellsFargo\Inbound /dfile:"GARH_DIVRECON_%mdy%.xml" /a:import /ch:WellsFargo /tname:"Dividends - Wells - GARH" 
