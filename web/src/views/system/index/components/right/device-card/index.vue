<template>
  <div class="dash-card-wrapper">
    <dashCard title="产线列表">
      <el-row :gutter="10" style="margin-top: 10px">
        <el-col v-for="(item, index) in deviceData" :key="index" :span="24">
          <dashCard
            title="全部设备"
            :showHeader="false"
            style="margin-bottom: 12px"
          >
            <div class="flex flex-column" style="overflow: hidden">
              <div class="flex-justify-main flex-center title">
                <div>{{ item.name }}</div>
                <div>
                  <i class="el-icon-s-tools" style="font-size: 22px"></i>
                </div>
              </div>
              <el-row :gutter="10" style="margin: 10px 0; height: 200px" type="flex" align="center">
                <el-col :span="12" style="display:flex;align:center" >
                  <el-image
                    class="flex-column"
                    :src="getImage(item.image)"
                    style="max-width: 400px; max-height: 200px;width:auto;height:auto"
                  >
                    <div
                      slot="error"
                      class="flex-center image-slot"
                      style="height: 200px; background: #dddddd"
                    >
                      <i
                        class="el-icon-picture-outline"
                        style="font-size: 30px"
                      ></i>
                    </div>
                  </el-image>
                </el-col>
                <el-col :span="12">
                  <ve-pie
                    :legend-visible="false"
                    :data="item.chartData"
                    height="200px"
                    :settings="chartSettings"
                  ></ve-pie>
                </el-col>
              </el-row>
              <el-row :gutter="10" class="liveData">
                <el-col :span="6"> 设计寿命：10年 </el-col>
                <el-col :span="6"> 已运行寿命：10年 </el-col>
                <el-col :span="6"> 剩余寿命：10年 </el-col>
                <el-col :span="6"> 健康评估得分：10年 </el-col> </el-row
              >:
              <div>
                <!-- <div v-for="(device, index) in item.device" :key="index">
                  <div class="flex-justify-main flex-center device-details"
                       v-for="(sensor, indexSensor) in device.sensordata" :key="indexSensor">
                    <div>传输设备 : {{ device.name }}-{{ device.sn }}</div>
                    <div>传感设备 : {{ sensor.name }}-{{ sensor.unit }}</div>
                    <div class="flex-justify-main flex-center">
                      <div :class="'status-color-' + sensor.state" style="margin-right: 18px"> |
                        {{ sensor.state }}
                      </div>
                      <div style="color: #FFCD6A">| 通道：{{ sensor.channel }} </div>
                    </div>
                  </div>
                </div> -->
                <div class="device-info__container">
                  <div
                    class="device-item"
                    v-for="(devices, k) in item.deviceArr"
                    :key="k"
                  >
                    <div
                      class="device-item__info"
                      v-for="(device, j) in devices.deviceArr"
                      :key="j"
                    >
                      <div class="device-item__info__name">
                        <el-tooltip
                          class="item"
                          effect="light"
                          :content="device.name"
                          placement="top"
                        >
                          <span class="d_name">{{ device.name }}:</span>
                        </el-tooltip>
                        <span style="margin-left: 10px; color: #389cc7">
                          {{dealNumber(device.value)}} {{device.unit ? device.unit : ''}}
                        </span>
                      </div>
                      <span class="device-item__info__status af-line success"
                        >通道{{ device.channel }}</span
                      >
                      <span
                        class="device-item__info__status af-line success"
                        :class="{ danger: device.state === '离线' }"
                        >{{ device.state }}</span
                      >
                      <span class="device-item__info__level af-line"
                        >{{ device.level === null ? 0 : device.level }}级</span
                      >
                    </div>
                    <div class="device-line__box">
                      <div class="device-line__item">
                        <div class="device-line__item__name">
                          <el-tooltip
                            class="item"
                            effect="light"
                            :content="devices.name + devices.sn"
                            placement="top"
                          >
                            <span class="d_name">{{ devices.name }}</span>
                          </el-tooltip>
                          <span style="font-size: 14px">
                            - {{ devices.id }}</span
                          >
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </dashCard>
        </el-col>
      </el-row>
    </dashCard>
  </div>
</template>

<script>
import dashCard from "../../card/index";
import VePie from "v-charts/lib/pie.common";
import productLine from "@/request/modules/productLine";
import sensor from "@/request/modules/sensor";
import aeDevice from "@/request/modules/aeDevice";
import { forEach } from "lodash";

export default {
  name: "alarm-history",
  components: {
    dashCard,
    VePie,
  },
  mounted() {
    this.getTableList();
  },
  data() {
    return {
      deviceData: [],
      chartSettings: {
        offsetY: 110,
        radius: 70,
        itemStyle: {
          color(params) {
            return ["#1BAE77", "#ff435c", "#f89728", "#00a0e6"][
              params.dataIndex
            ];
          },
        },
        label: {
          formatter: ["{b}:{c}，", "{second|占比{d}%}"].join("\n"),
          align: "right",
          fontSize: "12px",
          rich: {
            second: {
              align: "right",
            },
          },
        },
      },
      chartData: {
        columns: ["设备名称", "数量"],
        rows: [
          { 设备名称: "设备1", 数量: 19810 },
          { 设备名称: "设备2", 数量: 4398 },
          { 设备名称: "设备3", 数量: 52910 },
          { 设备名称: "设备4", 数量: 52910 },
        ],
      },
      deviceStatusData: [
        { name: "设备1", status: 1 },
        { name: "设备2", status: 1 },
        { name: "设备3", status: 0 },
        { name: "设备4", status: 0 },
      ],
      deviceDetailsData: [
        { name: "温度", status: 1, level: 3, value: "34°C" },
        { name: "温度", status: 1, level: 3, value: "34°C" },
        { name: "压力", status: 1, level: 3, value: "100Mpa" },
        { name: "压力", status: 1, level: 3, value: "100Mpa" },
        { name: "流量", status: 0, level: 3, value: "100" },
        { name: "流量", status: 0, level: 3, value: "100" },
      ],
    };
  },
  computed: {
    // deviceData () {
    //   return this.$store.state.sinuo.dashboard.alarmDevice
    // }
  },
  methods: {
    //处理数字
    dealNumber(v){
      return v ? v.toFixed(2) : '0'
    },
    getImage(image) {
      console.log(process.env.VUE_APP_API_HOST + image);
      return process.env.VUE_APP_API_HOST + image;
    },
    getData() {
      this.$store.dispatch("sinuo/dashboard/getAlarmDevice");
    },
    getTableList() {
      const params = {};
      productLine
        .line({
          params,
        })
        .then((res) => {
          const list = res.data;
          if (list) {
            this.deviceData = list;

            list.forEach((info, index) => {
              var _productlineid = info.id;
              console.log("productlineid" + _productlineid);
              const params = {
                productlineid: _productlineid,
              };
              sensor
                .line({
                  params,
                })
                .then((res) => {
                  const devices = res.data.comms;
                  console.log(info, 999, index, devices);
                  if (devices) {
                    this.getChartData(_productlineid, index);
                    let result = [];
                    for (let i = 0; i < devices.length; i++) {
                      let array = devices[i].sensordata;
                      for (let j = 0; j < array.length; j += 2) {
                        let item = JSON.parse(JSON.stringify(devices[i]));
                        delete item.sensordata;
                        item.deviceArr = array.slice(j, j + 2);
                        result.push(item);
                      }
                      devices[i].deviceArr = result;
                    }
                    this.deviceData[index].deviceArr = [];
                    this.deviceData[index].deviceArr = result;
                    console.log(this.deviceData, index, 110);
                    this.$forceUpdate();
                  }
                });
            });
          }
        });
    },
    getChartData(_productlineid, index) {
      const params = {
        prodecelineid: _productlineid,
      };
      aeDevice
        .requestDeviceSummary({
          params,
        })
        .then((res) => {
          if (res.data) {
            const typeName = [
              "声发射",
              "温度传感器",
              "压力传感器",
              "流量传感器",
              "振动设备",
            ];
            var chData = {
              columns: ["设备名称", "百分比"],
              rows: [],
            };
            console.log("设备名称");
            console.log(res.data);
            res.data.forEach((element) => {
              chData.rows.push({
                设备名称: typeName[element.type],
                百分比: element.percent,
              });
            });

            if (this.deviceData[index])
              this.deviceData[index].chartData = chData;
            this.$forceUpdate();
          }
        });
    },
    // getAeDeviceSummary(id) {
    //     const params = {
    //       prodecelineid: id,
    //     };
    //     aeDevice
    //       .requestDeviceSummary({
    //         params,
    //       })
    //       .then((res) => {
    //         console.log(res)
    //         this.chartData = res.data;
    //       });
    // },
  },
};
</script>

<style scoped lang="scss">
@import "~@/assets/style/theme/register.scss";

.dash-card-wrapper {
  height: 100%;
  color: white;
  margin-bottom: 10px;

  .title {
    height: 40px;
    border-bottom: solid 1px $color-secondary;
  }

  .liveData {
    height: 30px;
    margin: 10px 0;
    font-size: 14px;
    border-bottom: solid 1px $color-secondary;
  }

  .device-status-line {
    padding-left: 8px;
    border-left: solid 1px $color-secondary;
  }

  .device-status {
    margin-left: 10px;
    margin-right: 10px;
    margin-bottom: 8px;
    font-size: 14px;
  }

  .device-details {
    margin-left: 20px;
    margin-bottom: 12px;
    margin-right: 20px;
    font-size: 16px;
  }

  .status-color-正常 {
    color: #1cae77;
  }

  .status-color-故障 {
    color: #ff1a25;
  }

  .content {
    padding-top: 20px;

    .value-item-wrapper {
      width: 100%;
      max-height: 160px;
      flex-wrap: wrap;
      align-content: space-between;
    }
  }
}

.container-table {
  @include corner-borders(19px, $color-secondary);
  // 防止四角border被同颜色覆盖
  padding: 10px;

  &::v-deep {
    @include el-table-reset-primary();
  }

  .filter-tree {
    background-color: rgba(255, 255, 255, 0);
    color: $color-secondary;
  }
}

.device-info__container {
  .device-item {
    display: flex;
    justify-content: space-between;
    flex-wrap: nowrap;
    align-items: center;
    height: 28px;
    font-size: 14px;
    padding: 5px 0;
    .device-item__info {
      box-sizing: border-box;
      display: flex;
      padding-left: 15px;
      width: 43%;
      .device-item__info__name {
        display: flex;
        align-items: center;
        width: 55%;
        white-space: nowrap;
        color: #fff;
        .d_name {
          cursor: pointer;
          display: inline-block;
          max-width: 100px;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
        }
      }
      .device-item__info__status,
      .device-item__info__level {
        position: relative;
        width: 15%;
      }
      .af-line {
        &::before {
          content: "";
          position: absolute;
          left: -10px;
          width: 0;
          height: 100%;
          border-left: 2px solid;
        }
      }
      .device-item__info__status.success {
        color: #1cae77;
      }
      .device-item__info__status.danger {
        color: #de1f2e;
      }
      .device-item__info__level {
        color: #ffcd6a;
      }
    }
    .device-line__box {
      width: 14%;
      box-sizing: border-box;
      .device-line__item__name {
        white-space: nowrap;
        color: #fff;
        font-size: 12px;
        .d_name {
          cursor: pointer;
          display: inline-block;
          max-width: 100px;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
        }
      }
    }
  }
}
</style>
