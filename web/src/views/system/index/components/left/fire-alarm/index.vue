<template>
  <div class="dash-card-wrapper">
    <openCard :title="title">
      <div class="content">
        <table class="scroll-header">
          <thead>
            <tr>
              <th v-for="(item, i) in AlarmColumn(deviceTypeId)" :key="i" :class="item.class">{{ item.name }}</th>
            </tr>
          </thead>
        </table>
        <vue-seamless-scroll v-if="deviceTypeId === 1" :data="tableData(deviceTypeId)" class="seamless-warp" :class-option="classOption">
          <ul>
            <li v-for="(item, i) in tableData(deviceTypeId)" :key="i">
              <div class="wp-20">{{ item.time }}</div>
              <div class="wp-20">{{ $store.getters["sinuo/user/companyName"](item.unitID) }}</div>
              <div class="wp-20">{{ item.deviceAddress }}</div>
              <!-- <div class="wp-15">{{ item.detail }}</div> -->
              <div class="wp-20" :style="alarmColor(item.deviceType, item.alarmType)">{{ item.deviceType }}{{ item.alarmType ? `-${item.alarmType}` : "" }}</div>
              <div class="wp-20">{{ item.fireUserCode }}</div>
            </li>
          </ul>
        </vue-seamless-scroll>
        <vue-seamless-scroll v-if="deviceTypeId === 2" :data="tableData(deviceTypeId)" class="seamless-warp" :class-option="classOption">
          <ul>
            <li v-for="(item, i) in tableData(deviceTypeId)" :key="i">
              <div class="wp-25">{{ item.time }}</div>
              <div class="wp-25">{{ $store.getters["sinuo/user/companyName"](item.unitID) }}</div>
              <div class="wp-25">{{ item.deviceAddress }}</div>
              <div class="wp-25" :style="alarmColor(item.deviceType, item.alarmType)">{{ item.deviceType }}{{ item.alarmType ? `-${item.alarmType}` : "" }}</div>
            </li>
          </ul>
        </vue-seamless-scroll>
        <vue-seamless-scroll v-if="deviceTypeId === 3" :data="tableData(deviceTypeId)" class="seamless-warp" :class-option="classOption">
          <ul>
            <li v-for="(item, i) in tableData(deviceTypeId)" :key="i">
              <div class="wp-20">{{ item.time }}</div>
              <div class="wp-20">{{ $store.getters["sinuo/user/companyName"](item.unitID) }}</div>
              <div class="wp-20">{{ item.deviceAddress }}</div>
              <div class="wp-20" :style="alarmColor(item.deviceType, item.alarmType)">{{ item.deviceType }}{{ item.alarmType ? `-${item.alarmType}` : "" }}</div>
              <div class="wp-20">{{ item.voltage }}{{ 'Mpa' }}</div>
            </li>
          </ul>
        </vue-seamless-scroll>
        <vue-seamless-scroll v-if="deviceTypeId === 4" :data="tableData(deviceTypeId)" class="seamless-warp" :class-option="classOption">
          <ul>
            <li v-for="(item, i) in tableData(deviceTypeId)" :key="i">
              <div class="wp-20">{{ item.time }}</div>
              <div class="wp-20">{{ $store.getters["sinuo/user/companyName"](item.unitID) }}</div>
              <div class="wp-20">{{ item.deviceAddress }}</div>
              <div class="wp-20" :style="alarmColor(item.deviceType, item.alarmType)">{{ item.deviceType }}{{ item.alarmType ? `-${item.alarmType}` : "" }}</div>
              <div class="wp-20">{{ item.level }}{{ 'm' }}</div>
            </li>
          </ul>
        </vue-seamless-scroll>
      </div>
    </openCard>
  </div>
</template>

<script>
import openCard from '../../card/openCard'
import vueSeamlessScroll from 'vue-seamless-scroll'
import { fireColumn, waterGageColumn, waterLevelColumn, electricAlarmColumn } from './config'
const perPage = 20;

export default {
  name: 'fire-alarm',
  components: {
    openCard,
    vueSeamlessScroll
  },
  props: {
    title: {
      type: String
    },
    deviceTypeId: {
      type: Number
    }
  },
  data () {
    return {
      pagination: {
        page: 1,
        total: 0,
        perPage: perPage
      }
    };
  },
  computed: {
    classOption () {
      return {
        step: 0.2, // 数值越大速度滚动越快
        limitMoveNum: 2, // 开始无缝滚动的数据量 this.dataList.length
        hoverStop: true, // 是否开启鼠标悬停stop
        direction: 1, // 0向下 1向上 2向左 3向右
        openWatch: true, // 开启数据实时监控刷新dom
        singleHeight: 0, // 单步运动停止的高度(默认值0是无缝不停止的滚动) direction => 0/1
        singleWidth: 0, // 单步运动停止的宽度(默认值0是无缝不停止的滚动) direction => 2/3
        waitTime: 1000 // 单步运动停止的时间(默认值1000ms)
      }
    },
    alarmColor () {
      return (deviceType, alarmType) => {
        let color = 'rgba(255, 255, 255, 0.5)'
        switch (deviceType) {
          case '火警系统': {
            switch (alarmType) {
              case '火警':
                color = 'rgba(236, 54, 54, 0.5)'
                break
              case '复位':
                color = 'rgba(173, 255, 47, 0.5)'
                break
              case '故障':
                color = 'rgba(238, 230, 133, 0.5)'
                break
              default:
                break
            }
            break
          }
          default: {
            color = 'rgba(236, 54, 54, 0.5)'
            break
          }
        }
        return {
          color
        }
      }
    },
    tableData () {
      return (val) => {
        var value = [];
        switch (val) {
          case 1:
            value = this.$store.state.sinuo.dashboard.fireAlarm
            break
          case 2:
            value = this.$store.state.sinuo.dashboard.electricAlarm
            break
          case 3:
            value = this.$store.state.sinuo.dashboard.waterGage
            break
          case 4:
            value = this.$store.state.sinuo.dashboard.waterLevel
            break
          default:
            break
        }
        return value
      }
    },
    AlarmColumn () {
      return (val) => {
        var value = [];
        switch (val) {
          case 1:
            value = fireColumn
            break
          case 2:
            value = electricAlarmColumn
            break
          case 3:
            value = waterGageColumn
            break
          case 4:
            value = waterLevelColumn
            break
          default:
            break
        }
        return value
      }
    }
  },
  methods: {
    getAlarmList (params) {
      const { page, perPage } = this.pagination;
      params = {
        page,
        perPage,
        ...params
      };
      this.$store.dispatch('sinuo/dashboard/getAlarmScrollInfo', params).then(() => {
      })
    }
  },
  mounted () {
    this.getAlarmList({ deviceTypeID: this.deviceTypeId });
  }
}
</script>

<style scoped lang="scss">
.dash-card-wrapper{
  height: 220px;
  .content {
    height: 100%;
    .scroll-header {
      width: 100%;
      font-size: 10px;
      color: rgba(255, 255, 255, 0.5);
      thead {
        tr {
          line-height: 30px;
        }
      }
    }
    .seamless-warp {
      width: 100%;
      height: calc(100%);
      overflow: hidden;
      ul li {
        display: flex;
        line-height: 20px;
        div {
          font-size: 10px;
          color: rgba(255, 255, 255, 0.5);
          text-overflow:ellipsis;
          white-space: nowrap;
          overflow: hidden;
          text-align: center;
        }
      }
    }
  }
}
</style>
