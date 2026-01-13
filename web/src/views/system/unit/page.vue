<template>
  <d2-container
    class="page"
    id="page-device"
    v-loading="downLoadLoading"
    element-loading-text="正在下载中，请勿离开当前页面"
    element-loading-background="rgba(0, 0, 0, 0.6)"
    element-loading-spinner="el-icon-loading"
  >
    <el-row
      :gutter="10"
      style="margin: 0; height: 100%"
    >
      <el-col
        :span="6"
        style=""
      >
        <dashCard title="全部设备">
          <div class="container-table">
            <div class="flex-column">
              <el-tree
                class="filter-tree"
                :data="tableData"
                node-key="id"
                :props="defaultProps"
                :default-checked-keys="defaultCheckedKeys"
                @node-click="handleNodeClick"
                ref="tree"
              >
              </el-tree>
            </div>
          </div>
        </dashCard>
        <dashCard title="采样数据">
          <div class="downloadBtnDiv">
            <el-button @click="downloadExport">下载</el-button>
            <el-date-picker
              v-model="timeValue"
              type="date"
              :picker-options="pickerOptions"
              @change="changeTime"
              format="yyyy-MM-dd"
              value-format="yyyy-MM-dd"
              placeholder="选择日期">
            </el-date-picker>
          </div>
          <div class="container-table">
            <div class="flex-column table-list" style="max-height:800px;">
              <el-table
                :data="tableDataList"
                stripe
                @selection-change="handleSelectionChange"
              >
                <el-table-column
                  type="selection"
                  width="50"
                > </el-table-column>
                <el-table-column
                  prop="name"
                  label="设备名称"
                  width="80"
                >
                </el-table-column>
                <el-table-column
                  prop="points"
                  label="波形点数"
                  width="90"
                >
                </el-table-column>
                <el-table-column
                  prop="insertTime"
                  label="采集时间"
                  width="90"
                >
                </el-table-column>
                <el-table-column
                  prop="showHistory"
                  label="操作"
                >
                  <template slot-scope="scope">
                    <span
                      class="color-secondary cursor-pointer handle-btn"
                      @click="waveData(scope.row)"
                    >查看</span>
                  </template>
                </el-table-column>
              </el-table>
            </div>
          </div>
        </dashCard>
      </el-col>
      <el-col
        :span="18"
        style="height: 100%"
      >
        <productDevice
          :dataList="dataList"
          :xAxisData="proXAxisData"
          :loading="loading"
          :dateChangeTime="dateChangeByTime"
          @dateChangeByDay="dateChangeByDay"
          @changeCharts="changeCharts"
        ></productDevice> 
        <el-row
          :gutter="10"
          style="margin-top: 10px"
        >
          <el-col
            :span="12"
            style=""
          >
            <dataWave
              :title="title"
              :dataList="dataWaveList"
              :loading="waveDataLoading"
            ></dataWave>
          </el-col>
          <el-col
            :span="12"
            style=""
          >
            <imageWave
              :showImg="showImg"
              :imgSrc="imageWaveSrc"
              :loading="waveDataLoading"
            ></imageWave>
          </el-col>
        </el-row>
        <el-row
          :gutter="10"
          style="margin-top: 10px"
        >
          <el-col
            :span="12"
            style=""
          >
            <rootMeanSquareWave
              :dataList="rootMeanSquareWaveDataList"
              :loading="waveDataLoading"
            ></rootMeanSquareWave>
          </el-col>
          <el-col
            :span="12"
            style=""
          >
            <faultWave
              :showImg="showImg"
              :contentTitle="fwContentTitle"
              :content="fwContent"
              :loading="waveDataLoading"
            ></faultWave>
          </el-col>
        </el-row>
      </el-col>
    </el-row>
    <ae-Wave-dialog
      v-if="waveDataDialogVisible"
      :visible.sync="waveDataDialogVisible"
      :deviceId="deviceId"
      :info="waveDataInfo"
    ></ae-Wave-dialog>
    <div
      class="fullProgressDiv"
      v-if="elProgress"
    >
      <div class="up_progress">
        <div class="text_con">正在下载中，请勿离开当前页面</div>
        <el-progress
          type="circle"
          :percentage="progressPercent"
          :show-text="true"
          :stroke-width="16"
        > </el-progress>
      </div>
    </div>
  </d2-container>
</template>

<script>
import dataWave from "@/views/system/unit/components/data-wave/index.vue";
import faultWave from "@/views/system/unit/components/fault-wave/index.vue";
import imageWave from "@/views/system/unit/components/image-wave/index.vue";
import rootMeanSquareWave from "@/views/system/unit/components/root-mean-square-wave/index.vue";
import productDevice from "@/views/system/unit/components/productDevice/index.vue";
import dashCard from "@/views/system/index/components/card/index.vue";
import waveData from "@/request/modules/waveData";
import aeWaveDialog from "@/views/system/unit/components/aeWaveDialog/index.vue";
import util from "@/libs/util";
import request from "@/plugin/axios";

import fwImg1 from '@/assets/images/正常运行状态.png';
import fwImg2 from '@/assets/images/动环有裂纹.png';
import fwImg3 from '@/assets/images/静环有划痕.png';
import TIFF from 'tiff.js';

export default {
  components: {
    faultWave,
    imageWave,
    rootMeanSquareWave,
    productDevice,
    dataWave,
    dashCard,
    aeWaveDialog,
  },
  computed: {
    showImg() {
      return this.dataWaveList.data ? this.dataWaveList.data.length > 0 : false;
    },
    tableData() {
      return this.$store.state.sinuo.dashboard.alarmDevice.devlist;
    },
  },
  watch: {
    tableData: {
      immediate: true,
      deep: true,
      handler() {
        if (this.tableData && this.tableData.length) {
          this.deviceId = this.tableData[0].id;
          this.defaultCheckedKeys = [this.deviceId];
          this.changeCharts("day");
        }
      },
    },
  },
  data() {
    return {
      timeValue: '', //时间选择
      pickerOptions: {
          disabledDate(time) {
            return time.getTime() > Date.now();
          }
        },
      curDateDay: '',
      // showImg: false,
      fwImgtitle3: '故障类型分析',
      imageWaveSrc: '',
      fwContentTitle: '无故障',
      fwContent1: '均方根历史数据',
      fwContent: '剩余使用寿命:3年6个月<br>健康状态:47%',//faultEnum faultSta waveImage
      elProgress: false,
      progressPercent: 0,
      // 声发射名称
      title: "声发射数据",
      downLoadLoading: false,
      defaultCheckedKeys: [],
      loading: false,
      waveDataLoading: false,
      dataList: [],
      proXAxisData: [],
      defaultProps: {
        children: "datas",
        label: "name",
      },
      role: util.cookies.get("role"),
      deviceId: 0,
      page: 1,
      total: 0,
      isLoading: false,
      waveDataDialogVisible: false,
      waveDataInfo: null,
      dataWaveList: {},
      rootMeanSquareWaveDataList: {},
      tableDataList: [],
      selectDownLoadList: [],
      tableProps: [
        // {
        //   label: 'ID',
        //   prop: 'id',
        // },
        {
          label: "设备名称",
          prop: "name",
        },
        // {
        //   label: '设备ID',
        //   prop: 'deviceId'
        // },
        {
          label: "波形点数",
          prop: "points",
        },
        // {
        //   label: '采样速率',
        //   prop: 'speed'
        // },
        // {
        //   label: '前放增益',
        //   prop: 'gain'
        // },
        // {
        //   label: '电路放大倍数',
        //   prop: 'enlarge'
        // },
        {
          label: "采集时间",
          prop: "insertTime",
          width: 100,
        },
        {
          label: "操作",
          prop: "showHistory",
          width: 50,
        },
        {
          label: "选择",
          prop: "select",
          width: 50,
        },
      ],
      
    };
  },
  mounted() {
    this.getcyData();
  },

  beforeDestroy() { },
  methods: {
    //修改日期
    changeTime(v){
      console.log('time---',v)
      if(v){
        let time1 = v + ' ' + '00:00:00'
        let time2 = v + ' ' + '23:59:59'
        this.getcyData([time1, time2]);
      }else{
        this.getcyData();
      }
      
    },
    dateChangeByDay(v) {
      console.log('dateChangeByDay' + v)
      this.curDateDay = v;
      this.getWaveListByDay(this.curDateDay)
    },
    dateChangeByTime(v) {
      let [hoursStr, minutesStr] = v.split(":");
      let hours = parseInt(hoursStr, 10);
      let minutes = parseInt(minutesStr, 10);
      const currentTime = new Date(this.curDateDay.getTime()); // 复制输入的时间
      currentTime.setHours(hours);
      currentTime.setMinutes(minutes);
      console.log('dateChangeByTimessss' + currentTime)
      var times = this.generateTimeArray(currentTime);
      console.log(times)
      this.proXAxisData = times;
      this.getWaveListByTime(currentTime);
    },
    generateTimeArray(inputTime) {
      const inputDate = new Date(inputTime); // 解析输入的时间
      const currentTime = new Date(inputDate.getTime()); // 复制输入的时间

      currentTime.setMinutes(currentTime.getMinutes() - 30); // 减去 30 分钟

      const timeArray = [];
      while (currentTime.getTime() < inputDate.getTime()) {
        const hours = currentTime.getHours();
        const minutes = currentTime.getMinutes();

        const formattedHours = String(hours).padStart(2, "0");
        const formattedMinutes = String(minutes).padStart(2, "0");

        const timeString = `${formattedHours}:${formattedMinutes}`;
        timeArray.push(timeString);

        currentTime.setMinutes(currentTime.getMinutes() + 1); // 每次增加 30 分钟
      }

      return timeArray;
    },
    changeCharts(type) {
      console.log(type);
      this.proXAxisData = [];
      if (type === "day") {
        // 生成底部时间数组
        let dateTime = [];
        for (let i = 0; i < 24; i++) {
          const flag = i >= 10;
          dateTime.push(
            ...[`${!flag ? "0" : ""}${i}:00`, `${!flag ? "0" : ""}${i}:30`]
          );
        }
        this.proXAxisData = dateTime;
        console.log("proXAxisData");
        console.log(this.proXAxisData);
        this.getWaveList();
      } else if (type == "month") {
        const currentDate = new Date();
        const year = currentDate.getFullYear();
        const month = currentDate.getMonth(); // Note: January is 0, February is 1, ...
        const daysInMonth = new Date(year, month + 1, 0).getDate(); // Get the total number of days in the month
        const daysArray = [];
        for (let i = 1; i <= daysInMonth; i++) {
          daysArray.push(i);
        }
        this.proXAxisData = daysArray;
        this.getWaveListByMonth();
      } else if (type === "year") {
        const months = [];
        for (let i = 0; i < 12; i++) {
          const month = new Date(0, i).toLocaleString("default", {
            month: "long",
          });
          months.push(month);
        }
        this.proXAxisData = months;
        this.getWaveListByYear();
      }
      this.$forceUpdate();
    },
    handleSelectionChange(val) {
      this.selectDownLoadList = [];
      this.selectDownLoadList = val;
    },
    // 采样数据
    getcyData(time) {
      let obj = {}
      if(time){
        obj={
          deviceid : 0,
          page : 1,
          pagecount : 100,
          starttime: time[0],
          endtime: time[1],
        }
      }
      waveData.getWaveData(obj).then((res) => {
        this.tableDataList = res.data;
        this.total = res.count;
      });
    },
    waveData(item) {
      console.log(item);
      this.waveDataLoading = true;
      this.title = item.name;
      this.imageWaveSrc='';
      waveData.getsfsData(item.id).then((res) => {
        // this.imageWaveSrc = 'http://192.168.1.108:5000/' + res.data.waveImage
        this.loadTiffImage('http://192.168.1.108:5000/' + res.data.waveImage)
        if (res.data.faultSta) {
          if (res.data.faultSta === 1) {
            this.fwContentTitle = '有故障'
            this.fwContent = '故障类型：' + res.data.faultEnum
          } else {
            this.fwContentTitle = '无故障'
            this.fwContent = ''
          }
        } else {
          this.fwContentTitle = '无故障'
          this.fwContent = ''
        }
        this.dataWaveList = {
          ...res.data,
          data: res.data.data,
          name: item.name,
          type: "line",
          symbol: "circle",
          smooth: true,
          symbolSize: 6,
          itemStyle: {
            color: "#389CC7",
          },
          lineStyle: {
            width: 2,
          },
        };
        this.rootMeanSquareWaveDataList = {
          ...res.data,
          data: res.data.rootMeanSquare,
          name: item.name,
          type: "line",
          symbol: "circle",
          smooth: true,
          symbolSize: 6,
          itemStyle: {
            color: "#389CC7",
          },
          lineStyle: {
            width: 2,
          },
        }
        console.log(this.dataWaveList, res, 999);
        this.waveDataLoading = false;
      });
    },
    async loadTiffImage(url) {
      console.log('当前图片地址' + url)
      this.imageWaveSrc='';
      const response = await fetch(url);
      const buffer = await response.arrayBuffer();
      const tiff = new TIFF({ buffer });
      const canvas = tiff.toCanvas();
      this.imageWaveSrc = canvas.toDataURL();
    },
    //chartData(dataList) {
    //  const columns = dataList.map(function (value, index) {
    //    return index;
    //  });
    //  const rows = dataList;
    //  return {
    //    columns,
    //    rows,
    //  };
    //},
    getData() {
      this.$store.dispatch("sinuo/dashboard/getAlarmDevice");
    },
    getWaveList() {
      this.loading = true;
      this.curDateDay = new Date();
      const params = {
        deviceid: this.deviceId,
        page: this.page,
      };
      waveData
        .aeInfoList({
          params,
        })
        .then((res) => {
          console.log(res);
          const LINE_COLOR = [
            "#FF1F1F",
            "#3BFF3B",
            "#1B1BFF",
            "#F8F850",
            "#FF25FF",
            "#67FFFF",
            "#AF41F2",
            "#9F8B29",
            "#2B9B9E",
            "#FFC1CC",
            "#CACACA",
            "#B6416D",
            "#9B4324",
            "#E1B3E1",
            "#4F9E71",
            "#A4BA78",
            "#B17150",
            "#FF9665",
            "#FFD06D",
          ];
          let resData = res.data;
          let colorLength = resData.length;
          this.dataList = resData.map((item, index) => {
            return {
              name: item.name,
              type: "line",
              symbol: "circle",
              data: item.data,
              smooth: true,
              symbolSize: 6,
              itemStyle: {
                color: LINE_COLOR[index % colorLength],
              },
              lineStyle: {
                width: 3,
              },
            };
          });
          this.loading = false;
          console.log('dataList----',this.dataList);
        });
    },
    getWaveListByMonth() {
      this.loading = true;
      const params = {
        deviceid: this.deviceId,
        page: this.page,
      };
      waveData
        .aeInfoListMonth({
          params,
        })
        .then((res) => {
          console.log(res);
          const LINE_COLOR = [
            "#FF1F1F",
            "#3BFF3B",
            "#1B1BFF",
            "#F8F850",
            "#FF25FF",
            "#67FFFF",
            "#AF41F2",
            "#9F8B29",
            "#2B9B9E",
            "#FFC1CC",
            "#CACACA",
            "#B6416D",
            "#9B4324",
            "#E1B3E1",
            "#4F9E71",
            "#A4BA78",
            "#B17150",
            "#FF9665",
            "#FFD06D",
          ];
          this.dataList = res.data;
          let colorLength = this.dataList.length;
          this.dataList = this.dataList.map((item, index) => {
            return {
              name: item.name,
              type: "line",
              symbol: "circle",
              data: item.data,
              smooth: true,
              symbolSize: 6,
              itemStyle: {
                color: LINE_COLOR[index % colorLength],
              },
              lineStyle: {
                width: 3,
              },
            };
          });
          this.loading = false;
          console.log(this.dataList);
        });
    },
    getWaveListByYear() {
      this.loading = true;
      const params = {
        deviceid: this.deviceId,
        page: this.page,
      };
      waveData
        .aeInfoListYear({
          params,
        })
        .then((res) => {
          console.log(res);
          const LINE_COLOR = [
            "#FF1F1F",
            "#3BFF3B",
            "#1B1BFF",
            "#F8F850",
            "#FF25FF",
            "#67FFFF",
            "#AF41F2",
            "#9F8B29",
            "#2B9B9E",
            "#FFC1CC",
            "#CACACA",
            "#B6416D",
            "#9B4324",
            "#E1B3E1",
            "#4F9E71",
            "#A4BA78",
            "#B17150",
            "#FF9665",
            "#FFD06D",
          ];
          this.dataList = res.data;
          let colorLength = this.dataList.length;
          this.dataList = this.dataList.map((item, index) => {
            return {
              name: item.name,
              type: "line",
              symbol: "circle",
              data: item.data,
              smooth: true,
              symbolSize: 6,
              itemStyle: {
                color: LINE_COLOR[index % colorLength],
              },
              lineStyle: {
                width: 3,
              },
            };
          });
          this.loading = false;
          console.log(this.dataList);
        });
    },
    getWaveListByTime(curtime) {
      this.loading = true;
      const params = {
        deviceid: this.deviceId,
        page: this.page,
        time: this.formatDate(curtime),
      };
      waveData
        .aeInfoListTime({
          params,
        })
        .then((res) => {
          console.log(res);
          const LINE_COLOR = [
            "#FF1F1F",
            "#3BFF3B",
            "#1B1BFF",
            "#F8F850",
            "#FF25FF",
            "#67FFFF",
            "#AF41F2",
            "#9F8B29",
            "#2B9B9E",
            "#FFC1CC",
            "#CACACA",
            "#B6416D",
            "#9B4324",
            "#E1B3E1",
            "#4F9E71",
            "#A4BA78",
            "#B17150",
            "#FF9665",
            "#FFD06D",
          ];
          this.dataList = res.data;
          let colorLength = this.dataList.length;
          this.dataList = this.dataList.map((item, index) => {
            return {
              name: item.name,
              type: "line",
              symbol: "circle",
              data: item.data,
              smooth: true,
              symbolSize: 6,
              itemStyle: {
                color: LINE_COLOR[index % colorLength],
              },
              lineStyle: {
                width: 3,
              },
            };
          });
          this.loading = false;
          console.log(this.dataList);
        });
    },
    getWaveListByDay(curtime) {
      this.loading = true;
      const params = {
        deviceid: this.deviceId,
        date: this.formatDate(curtime),
      };
      console.log(params)
      waveData
        .aeInfoListDay({
          params,
        })
        .then((res) => {
          console.log(res);
          const LINE_COLOR = [
            "#FF1F1F",
            "#3BFF3B",
            "#1B1BFF",
            "#F8F850",
            "#FF25FF",
            "#67FFFF",
            "#AF41F2",
            "#9F8B29",
            "#2B9B9E",
            "#FFC1CC",
            "#CACACA",
            "#B6416D",
            "#9B4324",
            "#E1B3E1",
            "#4F9E71",
            "#A4BA78",
            "#B17150",
            "#FF9665",
            "#FFD06D",
          ];
          this.dataList = res.data;
          let colorLength = this.dataList.length;
          this.dataList = this.dataList.map((item, index) => {
            return {
              name: item.name,
              type: "line",
              symbol: "circle",
              data: item.data,
              smooth: true,
              symbolSize: 6,
              itemStyle: {
                color: LINE_COLOR[index % colorLength],
              },
              lineStyle: {
                width: 3,
              },
            };
          });
          this.loading = false;
          console.log(this.dataList);
        });
    },
    formatDate(date) {
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, '0');
      const day = String(date.getDate()).padStart(2, '0');
      const hours = String(date.getHours()).padStart(2, '0');
      const minutes = String(date.getMinutes()).padStart(2, '0');
      const seconds = String(date.getSeconds()).padStart(2, '0');
      return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
    },
    // async downloadFile(item) {
    //   this.downLoadLoading = true;
    //   // Replace with your API endpoint for downloading the file
    //   const apiEndpoint =
    //     process.env.VUE_APP_API_HOST + "/api/AeDatas/Download?id=" + item.id;
    //   console.log(11, apiEndpoint);
    //   // Replace 'YourAccessToken' with your actual access token
    //   const accessToken = util.cookies.get("token");
    //   try {
    //     // Fetch the file with the Authorization header
    //     const response = await fetch(apiEndpoint, {
    //       method: "GET",
    //       headers: {
    //         Authorization: `Bearer ${accessToken}`,
    //       },
    //     });

    //     // Convert the response to a blob
    //     const blob = await response.blob();

    //     // Create a blob URL for the binary response
    //     const blobUrl = window.URL.createObjectURL(blob);

    //     // Create a temporary anchor element
    //     const link = document.createElement("a");

    //     // Set the href attribute with the blob URL
    //     link.href = blobUrl;

    //     // Set the target attribute to '_blank' to open in a new tab
    //     link.target = "_blank";

    //     // Set the download attribute with the desired file name
    //     link.download = "波形数据.csv"; // Replace with the desired file name

    //     // Append the anchor to the document
    //     document.body.appendChild(link);

    //     // Simulate a click on the link to trigger the file download
    //     link.click();

    //     // Remove the anchor from the document
    //     document.body.removeChild(link);

    //     // Release the blob URL
    //     window.URL.revokeObjectURL(blobUrl);
    //     this.downLoadLoading = false;
    //   } catch (error) {
    //     console.error("Error downloading file:", error);
    //   }
    // },
    async downloadExport() {
      if (this.selectDownLoadList.length > 0) {
        this.downLoadLoading = true;
        this.progressPercent = 0;
        var data = [];
        this.selectDownLoadList.forEach((element) => {
          data.push(element.id);
        });
        if (data) {
          const apiEndpoint =
            process.env.VUE_APP_API_HOST + "/api/AeDatas/DownloadMuti";
          const accessToken = util.cookies.get("token");
          try {
            const response = await fetch(apiEndpoint, {
              method: 'POST',
              headers: {
                'Content-Type': 'application/json', // 根据你的 API 需求设置合适的 Content-Type
                Authorization: `Bearer ${accessToken}`,
              },
              body: JSON.stringify(data),
            });
            if (!response.ok) {
              this.downLoadLoading = false;
              this.elProgress = false;
              throw new Error('Network response was not ok.');
            }

            this.downLoadLoading = false;
            this.elProgress = true;
            const contentLength = parseInt(response.headers.get('Content-Length'));
            const totalChunks = 100; // Number of chunks for the progress bar
            let downloadedBytes = 0;

            const reader = response.body.getReader();

            const chunks = [];
            let receivedLength = 0;

            while (true) {
              const { done, value } = await reader.read();

              if (done) break;

              chunks.push(value);
              receivedLength += value.length;

              // Calculate progress
              downloadedBytes = receivedLength;
              const progress = (downloadedBytes / contentLength) * 100;

              // Update the progress bar or any UI element accordingly
              // For example, update a progress bar element's width
              this.updateProgressBar(progress); // Replace this with your UI update function
            }

            // Concatenate all received chunks into a single Uint8Array
            const uint8Array = new Uint8Array(receivedLength);
            let position = 0;
            for (const chunk of chunks) {
              uint8Array.set(chunk, position);
              position += chunk.length;
            }

            // 从响应中获取文件内容
            const blob = new Blob([uint8Array]);
            // 创建一个临时的 URL
            const url = window.URL.createObjectURL(blob);
            // 创建一个隐藏的 <a> 标签用于下载文件
            const link = document.createElement('a');
            link.href = url;
            link.download = '波形数据.zip';
            // link.download = 'downloaded_file.zip'; // 下载文件的名称
            document.body.appendChild(link);
            // 模拟点击下载链接
            link.click();
            // 清理资源
            link.remove();
            window.URL.revokeObjectURL(url);
            this.downLoadLoading = false;
            this.elProgress = false;
          } catch (error) {
            this.downLoadLoading = false;
            this.elProgress = false;
            console.error("Error downloading file:", error);
          }
        }
      }
    },
    updateProgressBar(progress) {
      console.log(progress)
      this.progressPercent = Math.floor(progress)
    },
    handleNodeClick(data, node) {
      if (node.level !== 1) return;
      if (this.deviceId === data.id) return;
      this.deviceId = data.id;
      this.getWaveList();
    },
  },
};
</script>

<style lang="scss" scoped>
@import "~@/assets/style/theme/register.scss";
.page.container-component {
  color: #fff;
}
#page-device::v-deep {
  @include el-tabs-reset();
  @include el-table-reset-primary();
}
::-webkit-scrollbar{
    width: 2px;
    height: 5px
  }
::-webkit-scrollbar-thumb{
    background-color: #0f2443;
    border-radius: 1px;
  }
.el-date-picker {
    background: rgb(15, 36, 67) !important;
    color: rgb(101, 226, 245) !important;
    border-radius: 0 !important;
    opacity: 0.9 !important;
    border: 1px solid rgba(101, 226, 245, 0.5) !important;
    margin: 0 !important;
}
.downloadBtnDiv::v-deep {

  .el-date-picker {
    .el-picker-panel__footer {
      border-top: 1px solid rgba(101, 226, 245, 1);
      background-color: transparent;
    }
  }
  .el-date-range-picker {
    .el-picker-panel__footer {
      border-top: 1px solid rgba(101, 226, 245, 1);
      background-color: transparent;
    }
  }
  .el-input__inner {
    font-size: 12px;
    font-family: PingFangSC-Medium;
    color: rgba(101, 226, 245, 1);
    width: 200px;
    height: 32px;
    background: linear-gradient(
      180deg,
      rgba(8, 40, 79, 0.2) 0%,
      rgba(16, 86, 117, 0.3) 100%
    );
    line-height: 32px;
    padding-left: 12px;
    border-radius: 0;
    border: 1px solid rgba(101, 226, 245, 0.5);
    &::placeholder {
      opacity: 0.5;
    }
    & + .el-input__prefix {
      display: none;
    }
    & + .el-input__suffix {
      display: none;
    }
  }
  /*  日期选择器*/
  .el-date-editor--date {
    background-image: url("../../../components/sn-form/images/icon-date.png");
    background-repeat: no-repeat;
    background-position-y: 8px;
    background-position-x: 172px;
  }
  .el-date-editor.el-input,
  .el-date-editor.el-input__inner {
    width: 200px;
  }

  .el-form-item__label {
    padding: 0;
    font-size: 12px;
    margin-right: 8px;
    text-align: start;
    font-family: PingFangSC-Medium;
    height: 32px;
    line-height: 32px;
    font-weight: 500;
    color: rgba(101, 226, 245, 1);
  }
}
.page {
  .container-table {
    .table-list{
      @include styled-scrollbar;
      overflow-x: hidden;
    }
    @include styled-scrollbar;
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
}
.pagination-wrapper::v-deep {
  margin: 20px 0 0;
  @include el-pagination-reset();
}
.table-wrapper {
  @include styled-scrollbar;
  position: relative;
  max-height: 700px;
  overflow-y: auto;
}

.downloadBtnDiv {
  display: flex;
  align-items:center;
  justify-content: space-between;
  margin-top: 0.5vw;
  margin-bottom: 0.5vw;
  .el-button {
    width: 91px;
    height: 32px;
    font-size: 14px;
    font-weight: 500;
    color: rgba(101, 226, 245, 1);
    padding: 0 0;
    background: rgba(0, 21, 53, 1);
    margin: 0 15px 0 0;
    border-width: 2px;
    border-style: solid;
    border-image-source: url("../../../components/sn-form/images/border.png");
    border-image-repeat: round;
    border-image-slice: 2 2 2 2 fill;
    transition: 0s all;
    &:hover {
      background-image: url("../../../components/sn-form/images/button-background.png");
      background-size: 100% 100%;
      border: none;
      border-radius: 0;
    }
  }
}
.fullProgressDiv {
  position: absolute;
  background-color: rgba(0, 0, 0, 0.541);
  top: 0;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1;
  .up_progress {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    gap: 3vw;
  }
}
</style>
