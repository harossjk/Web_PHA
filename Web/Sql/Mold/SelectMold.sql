﻿SELECT 
 A.RFID,
 A.MOLD_NAME,
 A.CAVITY, 
 A.TON, 
 A.RACK 
FROM 
	MOLD_SYSTEM.dbo.TB_MOLD A  with(nolock)