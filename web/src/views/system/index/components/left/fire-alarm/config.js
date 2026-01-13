const fireColumn = [
  {
    name: '告警时间',
    key: 'time',
    class: 'wp-20'
  },
  {
    name: '所属单位',
    key: 'unitID',
    class: 'wp-20'
  },
  {
    name: '具体位置',
    key: 'deviceAddress',
    class: 'wp-20'
  },
  // {
  //   name: '设备类型',
  //   key: 'detail',
  //   class: 'wp-20'
  // },
  {
    name: '告警详情',
    key: 'alarmType',
    class: 'wp-20'
  },
  {
    name: '用户编码',
    key: 'fireUserCode',
    class: 'wp-20'
  }
]

const waterGageColumn = [
  {
    name: '告警时间',
    key: 'time',
    class: 'wp-20'
  },
  {
    name: '所属单位',
    key: 'unitID',
    class: 'wp-20'
  },
  {
    name: '具体位置',
    key: 'deviceAddress',
    class: 'wp-20'
  },
  {
    name: '告警详情',
    key: 'alarmType',
    class: 'wp-20'
  },
  {
    name: '当前水压',
    key: 'voltage',
    class: 'wp-20'
  }
]

const waterLevelColumn = [
  {
    name: '告警时间',
    key: 'time',
    class: 'wp-20'
  },
  {
    name: '所属单位',
    key: 'unitID',
    class: 'wp-20'
  },
  {
    name: '具体位置',
    key: 'deviceAddress',
    class: 'wp-20'
  },
  {
    name: '告警详情',
    key: 'alarmType',
    class: 'wp-20'
  },
  {
    name: '当前水位',
    key: 'level',
    class: 'wp-20'
  }
]

const electricAlarmColumn = [
  {
    name: '告警时间',
    key: 'time',
    class: 'wp-25'
  },
  {
    name: '所属单位',
    key: 'unitID',
    class: 'wp-25'
  },
  {
    name: '具体位置',
    key: 'deviceAddress',
    class: 'wp-25'
  },
  {
    name: '告警详情',
    key: 'alarmType',
    class: 'wp-25'
  }
]

export { fireColumn, waterGageColumn, waterLevelColumn, electricAlarmColumn }
