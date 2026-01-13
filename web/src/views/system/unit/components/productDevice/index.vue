<template>
  <div
    class="dash-card-wrapper"
    v-loading="loading"
    element-loading-text="数据量较大,正在加载..."
    element-loading-background="rgba(0, 0, 0, 0.8)"
  >
    <dashCard title="传感器数据">
      <div class="selectBtnDiv">
        <el-button @click="changeCharts('day')">当日</el-button>
        <el-button @click="changeCharts('month')">当月</el-button>
        <el-button @click="changeCharts('year')">当年</el-button>
        <el-date-picker
          @change="dateChangeByDay"
          v-model="value1"
          type="date"
          placeholder="选择日期"
        >
        </el-date-picker>
      </div>
      <div class="content">
        <sn-histogram
          v-if="!loading"
          :dataType="dataType"
          :chartData="dataList"
          :xAxisData="xAxisData"
          :dateChangeByTime="dateChangeByTime"
          width="100%"
        ></sn-histogram>
      </div>
    </dashCard>
  </div>
</template>

<script>
import dashCard from "../card/index";
import snHistogram from "./components/sn-histogram";
import waveData from "@/request/modules/waveData";

export default {
  name: "alarm-statistics",
  data() {
    return {
      value1: "",
      dataType: 'day',
    };
  },
  components: {
    dashCard,
    snHistogram,
  },
  props: ["dataList", "loading", "xAxisData", "dateChangeTime"],
  methods: {
    changeCharts(type) {
      this.dataType = type
      this.$emit("changeCharts", type);
    },
    dateChangeByDay(v) {
      this.dataType = 'day'
      this.$emit("dateChangeByDay", v);
    },
    dateChangeByTime(v) {
      this.dataType = 'day'
      this.dateChangeTime(v);
    },
  },
};
</script>

<style scoped lang="scss">
@import "~@/assets/style/theme/register.scss";
.dash-card-wrapper {
  height: 380px;
}
.selectBtnDiv::v-deep {
  // margin:1vw;
  margin-left: 1vw;
  margin-top: 0.5vw;
  display: flex;
  justify-content: flex-start;
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
    border-image-source: url("../../../../../components/sn-form/images/border.png");
    border-image-repeat: round;
    border-image-slice: 2 2 2 2 fill;
    transition: 0s all;
    &:hover {
      background-image: url("../../../../../components/sn-form/images/button-background.png");
      background-size: 100% 100%;
      border: none;
      border-radius: 0;
    }
  }
  input::-webkit-input-placeholder {
    color: $color-secondary;
  }
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
    background-image: url("../../../../../components/sn-form/images/icon-date.png");
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
</style>
