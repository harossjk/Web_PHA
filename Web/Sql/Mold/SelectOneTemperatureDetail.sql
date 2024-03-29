﻿DECLARE @vDATE VARCHAR(20)
SET @vDATE = CONVERT(CHAR(10), GETDATE() , 23)
SELECT 
   A.CYCLE_NO,
   A.MACHINE_TYPE ,
   A.MACHINE_ID,
   A.VALUE ,
   D.CH_GROUP ,
   A.DATA_TYPE ,
   A.COLLECT_DT,
   D.CH_NAME ,
   D.COLOR 
FROM 
   TB_SENSOR_DATA_TEMPERATURE  A  with(nolock)
JOIN 
   TB_SENSOR_CHANNEL_INFO D 
   ON D.MACHINE_TYPE  =@MACHINE_TYPE
   AND D.MACHINE_ID =A.MACHINE_ID 
   AND D.CH_GROUP = A.CH_GROUP 
   AND D.CH_NAME LIKE '%T%'
WHERE 
    A.CYCLE_NO =@CYCLE_NO
    AND A.KEY_DT  =@vDATE
    AND A.MACHINE_ID = @MACHINE_ID
    AND A.DATA_TYPE NOT IN ('MeasData_Temperature')
    AND A.DATA_TYPE NOT IN ('None')
   ORDER BY A.COLLECT_DT DESC