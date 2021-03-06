SELECT 
	A.MACHINE_TYPE 
,	A.MACHINE_ID
,	A.RFID
,   B.MOLD_NAME 
,	A.DATA_TYPE
,	A.VALUE 
,	A.COLLECT_DT 
,	A.CYCLE_NO 
,	REPLACE(A.CH_GROUP, 'Ch', 'T') AS CH_GROUP
FROM 
	TB_SENSOR_DATA_TEMPERATURE  A 
JOIN 
	TB_MOLD B
	ON CAST(A.RFID AS INT) = CAST (B.RFID AS INT)
WHERE 
	CONVERT(CHAR(23), A.KEY_DT , 21)  BETWEEN CAST(@START_DT AS Date) AND CAST(@END_DT AS Date)
    AND CAST (A.COLLECT_DT AS time(0)) BETWEEN CAST (@START_DT AS time(0)) AND CAST (@END_DT AS time(0))
	AND CAST(A.RFID AS INT) = CAST(@RFID AS INT)
	AND A.DATA_TYPE NOT IN ('MeasData_Temperature')
	ORDER BY len (A.CYCLE_NO) ASC, A.CYCLE_NO ASC , A.COLLECT_DT ASC , A.CH_GROUP ASC
