const filterMenu = [{
  type: 'select',
  label: '报警类型',
  key: 'type',
  value: '',
  options: [
    {
      label: '全部',
      value: 3
    },
    {
      label: '正常',
      value: 0
    },
    {
      label: '报警',
      value: 1
    }, {
      label: '故障',
      value: 2
    },
  ],
  style: {
    display: 'inline-flex'
  }
},
{
  type: 'select',
  label: '产线',
  key: 'productLine',
  value: '',
  options: [
  ],
  style: {
    display: 'inline-flex'
  },
},
{
  type: 'select',
  label: '传输设备',
  key: 'commdeviceid',
  value: '',
  options: [
  ],
  style: {
    display: 'inline-flex'
  }
},
{
  type: 'select',
  label: '设备类型',
  key: 'gathertype',
  value: '',
  options: [
    {
      "label": "全部",
      "value": 0
    },
    {
      "label": "声发射",
      "value": 1
    },
    {
      "label": "温度传感器",
      "value": 2
    },
    {
      "label": "压力传感器",
      "value": 3
    },
    {
      "label": "流量传感器",
      "value": 4
    },
    {
      "label": "振动设备",
      "value": 5
    },
  ],
  style: {
    display: 'inline-flex'
  }
},
{
  type: 'input',
  label: '设备名称',
  key: 'name',
  value: '',
  style: {
    display: 'inline-flex'
  }
},
// {
//   type: 'date-time-picker',
//   label: '时间',
//   key: 'starttime',
//   value: '',
//   style: {
//     display: 'inline-flex',
//   }
// },
// {
//   type: 'date-time-picker',
//   label: '',
//   key: 'endtime',
//   value: '',
//   style: {
//     display: 'inline-flex',
//   }
// },

{
  type: 'datetimerange',
  label: '时间',
  key: 'timeselect',
  value: '',
  style: {
    display: 'inline-flex',
  }
},
{
  type: 'button',
  options: [
    {
      label: '搜索',
      type: 'submit',
      style: {
        width: '50px'
      }
    }, {
      label: '重置',
      type: 'reset',
      style: {
        width: '50px'
      }
    }
  ],
  style: {
    display: 'inline-flex',
  }
},
  // {
  //   type: 'date-picker',
  //   label: '开始日期',
  //   key: 'startTime',
  //   value: '',
  //   style: {
  //     display: 'inline-flex'
  //   }
  // },
  // {
  //   type: 'date-picker',
  //   label: '结束日期',
  //   key: 'endTime',
  //   value: '',
  //   style: {
  //     display: 'inline-flex'
  //   }
  // },
  // {
  //   type: 'input',
  //   label: '设备类型',
  //   key: 'deviceType',
  //   value: '',
  //   style: {
  //     display: 'inline-flex'
  //   }
  // },
  // {
  //   type: 'input',
  //   label: '报警位置',
  //   key: 'deviceAddress',
  //   value: '',
  //   style: {
  //     display: 'inline-flex'
  //   }
  // },
  // {
  //   type: 'select',
  //   label: '处理类型',
  //   key: 'alarmCategory',
  //   value: '',
  //   options: [
  //   ],
  //   style: {
  //     display: 'inline-flex'
  //   },
  //   props: {
  //     filterable: true
  //   }
  // },
  // {
  //   type: 'select',
  //   label: '单位名称',
  //   key: 'unitID',
  //   value: '',
  //   options: [
  //   ],
  //   style: {
  //     display: 'inline-flex'
  //   },
  //   props: {
  //     filterable: true
  //   }
  // },
  // {
  //   type: 'button',
  //   options: [
  //     {
  //       label: '搜索',
  //       type: 'submit',
  //       style: {
  //         width: '50px'
  //       }
  //     }, {
  //       label: '重置',
  //       type: 'reset',
  //       style: {
  //         width: '50px'
  //       }
  //     }
  //   ],
  //   style: {
  //     display: 'inline-flex',
  //   }
  // },
]
const deviceTypeMap = {
  '1': ['火警', '真实火警', '一般火警', '火警异常', '疑似火警', '故障', '启动', '屏蔽', '反馈', '监管', '复位'],
  '2': ['故障', '报警'],
  '3': ['过高', '过低'],
  '4': ['过高', '过低'],
  '5': ['火警'],
  '6': ['全部'],
  '7': ['全部'],
}
export { filterMenu, deviceTypeMap }
