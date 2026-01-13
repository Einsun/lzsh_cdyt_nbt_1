<template>
  <div
    class="dash-card-wrapper"
    v-loading="loading"
    element-loading-text="数据量较大,正在加载..."
    element-loading-background="rgba(0, 0, 0, 0.8)"
  >
    <dashCard title="故障类型分析">
      <div
        class="content"
        v-show="showImg"
      >
        <p class="contentTitleP">
          <span>{{ contentTitle }}</span>
        </p>
        <p class="contentP">
          <span>{{ content }}</span>
        </p>
        <!--        <sn-histogram :chartData="chartData"></sn-histogram>-->
      </div>
    </dashCard>
  </div>
</template>

<script>
import dashCard from '../card/index'
import snHistogram from './components/sn-histogram'
import waveData from "@/request/modules/waveData";

export default {
  name: 'alarm-statistics',
  components: {
    dashCard,
    snHistogram
  },
  props: {
    contentTitle: {
      type: String,
      default() {
        return ''
      }
    },
    content: {
      type: String,
      default() {
        return ''
      }
    },
    deviceId: {
      type: Number,
      default() {
        return 0
      }
    },
    showImg: {
      type: Boolean,
      default() {
        return false
      }
    },
    loading: {
      type: Boolean,
      default() {
        return false
      }
    },
    imgSrc: {
      type: String,
      default() {
        return ''
      }
    },
    info: {
      type: Object,
      default() {
        return {};
      }
    }
  },
  mounted() {
    console.log('-------')
    this.getWaveData()
  },
  data() {
    return {
      dataList: []
    };
  },
  methods: {
    changeLine(str) {
      if (str) {
        var strtext = str.replace(/<br>/g, '\n')
        return strtext
      } else {
        return ''
      }
    },
    getWaveData() {
      const params = {
        deviceid: this.deviceId,
      }
      waveData.live({
        params
      }).then(res => {
        console.log(res)
        // this.dataList = res.data.data;
        console.log(this.dataList)
        // this.chartData = dataArray.map(function (value, index) {
        //   return [index, value];
        // });
      })
    },
  },
  computed: {
    chartData() {
      const columns = this.dataList.map(function (value, index) {
        return index;
      });
      const rows = this.dataList
      return {
        columns,
        rows
      }
    }
  }
}
</script>

<style scoped lang="scss">
.dash-card-wrapper {
  height: 320px;
  .content {
    width: 100%;
    height: calc(100% - 8px);
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    .contentTitleP {
      font-size: 20px;
      line-height: 1.8;
    }
    .contentP {
      line-height: 1.8;
    }
  }
}
</style>
