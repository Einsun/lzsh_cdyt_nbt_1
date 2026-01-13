import store from "@/store/index";
const tableRules = [
  {
    name: "温度报警规则",
    id: 0,
  },
  {
    name: "压力报警规则",
    id: 1,
  },
  {
    name: "流量报警规则",
    id: 2,
  },
  {
    name: "振动报警规则",
    id: 3,
  },
];
const gatherRules = [
  {
    name: "声发射采集规则",
    id: 0,
  },
  {
    name: "传感设备采集规则",
    id: 1,
  },
  // {
  //   "name": "振动设备采集规则",
  //   "id": 2
  // },
];
const alarmTableProps = [
  {
    label: "名称",
    prop: "name",
  },
  {
    label: "报警上线",
    prop: "max",
  },
  {
    label: "报警下线",
    prop: "min",
  },
  {
    label: "状态",
    prop: "status",
  },
  {
    label: "操作",
    prop: "",
    width: 18 + 158,
  },
];
const gatherTableProps1 = [
  {
    label: "名称",
    width: 120,
    prop: "name",
  },
  {
    label: "定时采集类型",
    width: 160,
    prop: "startTime1",
  },
  {
    label: "采样间隔",
    width: 160,
    prop: "endTime1",
  },
  {
    label: "采样次数",
    prop: "interval1",
  },
  {
    label: "操作",
    prop: "",
    width: 18 + 158,
  },
];
const gatherTableProps = [
  {
    label: "名称",
    width: 120,
    prop: "name",
  },
  {
    label: "开始时间",
    width: 160,
    prop: "startTime",
  },
  {
    label: "结束时间",
    width: 160,
    prop: "endTime",
  },
  {
    label: "间隔时间",
    prop: "interval",
  },
  {
    label: "次数",
    prop: "count",
  },
  {
    label: "状态",
    prop: "status",
  },
  {
    label: "操作",
    prop: "",
    width: 18 + 158,
  },
];
const aeGatherTableProps = [
  {
    label: "名称",
    width: 120,
    prop: "name",
  },
  {
    label: "采集速率",
    prop: "ae_measure_speed",
  },
  {
    label: "采集模式",
    prop: "ae_measure_mode",
  },
  {
    label: "采样间隔",
    prop: "ae_measure_interval",
  },
  {
    label: "定时采集类型",
    prop: "ae_timing_type",
  },
  {
    label: "操作",
    prop: "",
    width: 18 + 158,
  },
];
const comDeviceTable = [
  {
    label: "ID",
    width: 40,
    prop: "id",
  },
  {
    label: "设备名称",
    prop: "name",
  },
  {
    label: "设备类型",
    prop: "type",
  },
  {
    label: "SN",
    prop: "sn",
  },
  {
    label: "操作",
    prop: "",
    width: 18 + 158,
  },
];

const deviceTable = [
  {
    name: "声发射",
    id: 0,
  },
  {
    name: "温度",
    id: 1,
  },
  {
    name: "压力",
    id: 2,
  },
  {
    name: "流量",
    id: 3,
  },
  {
    name: "振动",
    id: 4,
  },
];

const deviceTableProps = [
  //   {
  //   label: '网关类型',
  //   prop: 'commDeviceId'
  // },
  {
    label: "名称",
    prop: "name",
    width: 130,
  },
  {
    label: "设备编号",
    prop: "sn",
    width: 130,
  },
  {
    label: "地址",
    prop: "address",
  },
  {
    label: "通道",
    prop: "pos",
  },
  {
    label: "网关ID",
    prop: "commDeviceId",
  },
  {
    label: "设备类型",
    prop: "type",
  },
  {
    label: "报警",
    prop: "id2",
  },
  {
    label: "状态",
    prop: "id1",
  },
  {
    label: "操作",
    prop: "",
    width: 18 + 158,
  },
];

export {
  tableRules,
  gatherRules,
  alarmTableProps,
  gatherTableProps,
  comDeviceTable,
  deviceTable,
  deviceTableProps,
  gatherTableProps1,
};
