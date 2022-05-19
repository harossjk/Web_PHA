import React from 'react'
import { ResponsivePie } from '@nivo/pie'
import myStyle from './CircleGraph.module.scss'
interface props {
    data: any;
}

const CircleGraph = ({ data }: props) => (
    <div className={myStyle.container}>
        <div className={myStyle.title}>이상 건수 기준 통계</div>
        <div className={myStyle.circle}></div>
        <ResponsivePie
            data={data}
            colors={['#ff66ad', '#ed4ad5', '#ce5bf5', '#745bf5', '#469dfa', '#46fae5', '#a0ff7a', '#e7f57a', '#ffbb73']}
            margin={{ top:24, right: 120, bottom: 75, }}
            innerRadius={0.4}
            padAngle={0}  // 값들 간격 띄움
            cornerRadius={0}
            activeOuterRadiusOffset={8}
            borderWidth={1}
            borderColor={{
                from: 'color',
                modifiers: [
                    [
                        'darker',
                        0.2
                    ]
                ]
            }}
            arcLinkLabelsSkipAngle={10}
            arcLinkLabelsTextColor="#ffffff"
            arcLinkLabelsThickness={2}
            arcLinkLabelsColor={{ from: 'color' }}
            arcLabelsSkipAngle={0}
            arcLabelsTextColor="#1f2328"
            arcLabelsRadiusOffset={0.5}
            defs={[
                {
                    id: 'dots',
                    type: 'patternDots',
                    background: 'inherit',
                    color: "#ffffff",
                    size: 4,
                    padding: 1,
                    stagger: true
                },
                {
                    id: 'lines',
                    type: 'patternLines',
                    background: 'inherit',
                    color: 'rgba(255, 255, 255, 0.3)',
                    rotation: -45,
                    lineWidth: 6,
                    spacing: 10
                }
            ]}

            legends={[
                {
                    anchor: 'right',
                    direction: 'column',
                    justify: false,
                    translateX: 90,
                    translateY: 10,
                    itemsSpacing: 3,
                    itemWidth: 100,
                    itemHeight: 18,
                    itemTextColor: '#ffffff',
                    itemDirection: 'left-to-right',
                    itemOpacity: 1,
                    symbolSize: 17,
                    symbolShape: 'square',
                    effects: [
                        {
                            on: 'hover',
                            style: {
                                itemTextColor: '#000'
                            }
                        }
                    ]
                }
            ]}
        />
    </div>
)

export default CircleGraph