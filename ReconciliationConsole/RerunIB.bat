IF DEFINED PROGRAMFILES(x86) (
  SET "_PROGRAMFILES=%PROGRAMFILES(x86)%"
) ELSE (
  SET "_PROGRAMFILES=%PROGRAMFILES%"
)

@For /F "tokens=3,2,4 delims=/ " %%A in ('Date /t') do @( 
Set ymd=%%C%%A%%B
)

"%_PROGRAMFILES%\Gargoyle Strategic Investments\ReconciliationConsole\ReconciliationConsole.exe" /dir:\\gargoyle-fs3\public\imports\FTP\IB /cfile:"F1957488.Trades.%ymd%.csv" /pfile:"F1957488.Positions.%ymd%.csv" /bfile:"F1957488.Transfers.%ymd%.csv" /dfile:"F1957488.Corporate_Actions.%ymd%.csv" /a:import /ch:IB  /tname:"Reconciliation - IB"
