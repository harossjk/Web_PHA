﻿SELECT 
  MACHINE_ID
, MACHINE_NAME 
, MACHINE_TYPE
, MACHINE_LEGEND 
, STATUS 
, VIEW_STATUS 
,DATE
FROM PI_SVR.dbo.REALTIME_LINKI   with(nolock)
ORDER BY len (MACHINE_ID) ASC, MACHINE_ID ASC 