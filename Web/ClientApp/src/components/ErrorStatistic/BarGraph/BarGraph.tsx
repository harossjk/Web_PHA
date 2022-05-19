import React from 'react'
import { Bar, CartesianGrid, Cell, ComposedChart, Legend, Line, Tooltip, XAxis, YAxis } from 'recharts';

interface props {
  data: any
}

const colors = ['#ff66ad', '#ed4ad5', '#ce5bf5', '#745bf5', '#469dfa', '#46fae5', '#a0ff7a', '#e7f57a', '#ffbb73'];

const ReferenceLabel = (props: {
  fill: any;
  value: any;
  y: any;
  x: any;
}) => {
  const { fill, value, x, y } = props;

  return (
    <text
      x={x}
      y={y}
      dy={-4}
      dx={25}
      fill={fill}
      style={{ fontSize: "20px", fontFamily: 'sans-serif' }}
      textAnchor="middle">{value}</text>

  );
};

const BarGraph = ({ data }: any) => {
  return (
    <ComposedChart
      width={1030}
      height={300}
      data={data}
      margin={{
        top: 30, right: 30, left: 20, bottom: 1,
      }}
    >
      <CartesianGrid strokeDasharray="4 3" stroke="#919191" />
      <XAxis
        dataKey="label"
        tick={{ fill: 'white', fontSize: "14px" }}

      />
      <YAxis />
      <Tooltip />
      {/* <Line type="monotone" dataKey="uv" stroke="#82ca9d" /> */}
      <Bar dataKey="value" barSize={50} fill="#413ea0" label={ReferenceLabel}>
        {
          data.map((entry: any, index: any) => (
            <Cell key={`cell-${index}`} fill={colors[index]} />
          ))
        }
      </Bar>
    </ComposedChart>
  );
}


export default BarGraph