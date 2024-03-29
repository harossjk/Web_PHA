﻿
DECLARE @vDATE VARCHAR(20)
SET @vDATE = CONVERT(CHAR(10), GETDATE() , 23)

SELECT 
	 D.MACHINE_TYPE
	,A.MACHINE_ID
	,A.MACHINE_NAME
	,D.CH_NAME 
	,B.CYCLE_NO 
	,B.VALUE 
	,B.DATA_TYPE
	,A.MACHINE_LEGEND 
	,A.VIEW_STATUS
	,D.COLOR 
	,B.KEY_DT
	,D.IS_VISIBLE
	,C.IS_CONNECTED AS SENSOR_IS_ACTIVE 
FROM 
	PI_SVR.dbo. REALTIME_LINKI A with(nolock)
JOIN
	TB_SENSOR_DATA_PRESSURE B 
	ON  B.MACHINE_TYPE  =@MACHINE_TYPE
   	AND B.MACHINE_ID    = A.MACHINE_ID
	AND B.DATA_TYPE ='MeasData_Pressure'    
	AND B.KEY_DT = @vDATE
	AND B.CYCLE_NO = @CYCLE_NO
JOIN 
	TB_SENSOR_CHANNEL_INFO D 
	ON D.MACHINE_TYPE  =B.MACHINE_TYPE 
	AND D.MACHINE_ID  = B.MACHINE_ID 
	AND D.CH_GROUP = B.CH_GROUP 
	AND D.CH_NAME LIKE '%P%'
	AND D.MACHINE_ID = B.MACHINE_ID 
JOIN 
	TB_SENSOR_STATUS_INFO C 
	ON C.MACHINE_TYPE  = B.MACHINE_TYPE 
	AND C.MACHINE_ID   = A.MACHINE_ID
WHERE 
	 A.MACHINE_ID =@MACHINE_ID
	 ORDER BY len (D.CH_NAME) ASC
