<template>
  <div class="dash-card-wrapper">
    <dashCard title="报警汇总栏">
      <div class="content">
        <div class="container-table">
          <div class="table-wrapper">
            <el-table :data="tableDataList" stripe>
              <el-table-column prop="Label" label="设备名称" width="100">
              </el-table-column>
              <el-table-column prop="Reason" label="原因" width="80">
              </el-table-column>
              <el-table-column prop="Time" label="报警时间"> </el-table-column>
              <el-table-column prop="showHistory" label="操作" width="55">
                <template slot-scope="scope">
                  <span
                    class="color-secondary cursor-pointer handle-btn"
                    @click="Settle(scope.row)"
                    >处理</span
                  >
                </template>
              </el-table-column>
            </el-table>
          </div>
        </div>
        <!-- <sn-histogram :chartData="chartData"></sn-histogram> -->
      </div>
    </dashCard>
  </div>
</template>

<script>
import dashCard from "../../card/index";
import snHistogram from "./components/sn-histogram";

import history from "@/request/modules/histroyData";
import waveData from "@/request/modules/waveData";

export default {
  name: "alarm-statistics",
  components: {
    dashCard,
    snHistogram,
  },
  mounted() {
    // 创建 WebSocket 连接
    this.socket = new WebSocket("ws://192.168.1.108:5000/ws");
    // 当连接打开时触发
    this.socket.onopen = () => {
      console.log("WebSocket 连接已建立");
    };
    // 接收消息时触发
    this.socket.onmessage = (event) => {
      const message = JSON.parse(event.data);
      this.socket(message);
    };
    // 发生错误时触发
    this.socket.onerror = (error) => {
      console.error("WebSocket 错误:", error);
    };
    // 当连接关闭时触发
    this.socket.onclose = () => {
      console.log("WebSocket 连接已关闭");
    };
    this.getDevice();
    this.$nextTick().then(() => {
      // this.getData();
    });
    // this.getData();
  },
  data() {
    return {
      tableDataList: [],
      socket: null,
      deviceNameList: [],
    };
  },
  beforeDestroy() {
    this.socket.close();
  },
  methods: {
    socket(message) {
      if (message.settle == false) {
        let devectType = deviceTypeData.find(
          (t) => t.value == message.deviceId
        );
        var typeName = devectType ? devectType.label : null;
        this.tableDataList.push({
          id: message.id,
          deviceId: message.deviceId,
          Label: typeName,
          Reason: message.reason,
          Time: message.time,
        });
      }
    },
    Settle(item) {
      console.log(item);
      var data = [];
      data.push(item.id);
      waveData.settleAlarm(data).then((res) => {
        console.log("Settle");
        console.log(res);
      });
    },
    getData() {
      // this.$store.dispatch("sinuo/dashboard/getIndustryAlarmStatistics");
      const params = {
        deviceid: 16,
        page: 1,
        pagecount: 10,
      };
      waveData
        .adInfoList({
          params,
        })
        .then((res) => {
          console.log("getData");
          console.log(res.data);
          this.tableDataList = [];
          let deviceTypeData = this.deviceNameList;
          res.data.forEach((item) => {
            if (item.settle == false) {
              let devectType = deviceTypeData.find(
                (t) => t.value == item.deviceId
              );
              var typeName = devectType ? devectType.label : null;
              this.tableDataList.push({
                id: item.id,
                deviceId: item.deviceId,
                Label: typeName,
                Reason: item.reason,
                Time: item.time,
              });
              console.log("devectTypeName");
              console.log(typeName);
            }
          });
        });
      // [{label: "实验室A通道",value: 13},{label: "实验室A通道",value: 14},{label: "实验室A通道",value: 15},{label: "实验室A通道",value: 16}]
    },
    // 获取传输设备数据
    getDevice() {
      console.log("getDevicehishishis");
      history.getDevice().then((res) => {
        console.log(res.data);
        let list = res.data.devlist || [];
        this.deviceNameList = [];
        list.forEach((data) => {
          data.datas.forEach((item) => {
            item.datas.forEach((v) => {
              this.deviceNameList.push({
                label: v.name,
                value: v.id,
              });
            });
          });
        });
        console.log("options");
        console.log(this.deviceNameList);
      });
    },
  },
  computed: {
    statisticsData() {
      return this.$store.state.sinuo.dashboard.industryAlarmStatistics;
    },
    chartData() {
      const dataList = this.statisticsData.slice(1);
      const columns = dataList.map((item) => {
        return item[2];
      });
      const rows = dataList.map((item) => {
        return item[0];
      });
      return {
        columns,
        rows,
      };
    },
  },
};
</script>

<style scoped lang="scss">
@import "~@/assets/style/theme/register.scss";
.dash-card-wrapper {
  height: 233px;
  &::v-deep .table-wrapper {
    @include el-table-reset-primary();
  }
  .table-wrapper {
    @include styled-scrollbar;
    position: relative;
    max-height: 700px;
    overflow-y: auto;
    .operation-btn {
      margin-right: 20px;
    }
  }
}
.page.container-component {
  color: #fff;
}
</style>
