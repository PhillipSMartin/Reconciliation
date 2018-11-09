IF DEFINED PROGRAMFILES(x86) (
  SET "_PROGRAMFILES=%PROGRAMFILES(x86)%"
) ELSE (
  SET "_PROGRAMFILES=%PROGRAMFILES%"
)

@For /F "tokens=3,2,4 delims=/ " %%A in ('Date /t') do @( 
Set ymd=%%C%%A%%B
)

"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\BNY /cfile:"Custody_Confirmations_%ymd%.csv" /pfile:"Custody_Valuation_%ymd%.csv" /a:import /ch:BONY  /tname:"Reconciliation - BONY"