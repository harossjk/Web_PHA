import React, { useEffect, useState } from 'react'
import { Bar, CartesianGrid, Cell, ComposedChart, Legend, Line, Tooltip, XAxis, YAxis } from 'recharts';
import myStyle from './IndividualBarGraph.module.scss'
interface props {
  graphData: any
}


const ReferenceLabel = (props: {
  fill: any;
  value: any;
  y: any;
  x: any;
  width: any;
  height: any;

}) => {
  const { fill, value, x, y, width, height } = props;
  const fireOffset = value.toString().length < 5;
  const offset = fireOffset ? 10 : 5;
  return (
    <text x={Number(value) > 6 ? (x + width - offset) : (x + width + 15)} y={y + height - 10} fill={"#fff222"} textAnchor="end">
      {value}
    </text>
  );
};


const IndividualBarGraph = ({ graphData }: props) => {

  console.log("graphData", graphData);


  //커스텀툴팁
  const CustomTooltip = ({ active, payload, label }: any) => {

    if (active && payload && payload.length) {
      return (
        <div className={myStyle.customTooltip} style={{ backgroundColor: "#1f2328" }}>
          <p >{`${label}`}</p>
          <p >{`이상건수 : ${payload[0].value}건`}</p>
        </div>
      );
    }
    return null;
  };

  return (
    <ComposedChart
      width={580}
      height={320}
      data={graphData}
      margin={{
        top: 20, right: 30, left: 40
      }}
      layout="vertical"
    >
      <XAxis
        type='number'
      />
      <YAxis dataKey="label" type="category"
        tick={{ fill: 'white', fontSize: "12px" }}
      />
      <Tooltip content={<CustomTooltip />} />
      {/* <Tooltip contentStyle={{ backgroundColor: "#1f2328", color: "white" }} /> */}
      {/* <Line type="monotone" dataKey="uv" stroke="#82ca9d" /> */}
      {
        ReferenceLabel !== undefined &&
        <Bar dataKey="value" barSize={30} label={ReferenceLabel} >
          {
            graphData !== undefined && graphData.map((entry: any, index: any) => (
              <Cell key={`cell-${index}`} fill={"#285a64"} />
            ))
          }
        </Bar>
      }

    </ComposedChart>
  );
}


export default IndividualBarGraph