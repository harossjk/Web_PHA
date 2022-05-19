import React, { useCallback, useEffect, useRef } from 'react'
import { AgGridReact } from "ag-grid-react";
import { CellDoubleClickedEvent, CellValueChangedEvent, SelectionChangedEvent } from "ag-grid-community";
import "ag-grid-community/dist/styles/ag-grid.css";
import "ag-grid-community/dist/styles/ag-theme-alpine.css";
import { toJS } from 'mobx';
import useStore from '../../../stores';
import { observer } from 'mobx-react';

interface props {
  data: any;
  defaultColDef: any;
  type?: string;

}

const AgGrid = ({ data, defaultColDef, type }: props) => {

  const { UtilDataStore } = useStore();

  const gridRef = useRef(data);
  const isClicked = UtilDataStore.getExportBtnClick;
  // console.log("isClicked", isClicked);

  let params = {
    fileName: 'RAW_DATA_REPORT',
    sheetName: 'RawData',
    columnWidth: 200,
  }
  if (isClicked) {
    gridRef.current.api.exportDataAsCsv(params);
    UtilDataStore.setExportBtnClick(false);
  }
  return (
    <div>
      {/* <div style={{ margin: '10px 0' }}>
        <button onClick={onBtnExport}>Download CSV export file</button>
      </div> */}
      <div className="ag-theme-alpine" style={type === "rawData" ? { height: '800px' } : { height: '337px' }} >
        <AgGridReact
          columnDefs={defaultColDef}
          rowData={data}
          ref={gridRef}

        />
      </div>
    </div>
  )
}

export default observer(AgGrid)

