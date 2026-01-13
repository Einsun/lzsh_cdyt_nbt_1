<template>
  <div class="footer">
    <section class="content">
      <div class="img-background">
        <img src="./images/background.png" alt="" />
      </div>
      <div class="tab-wrapper flex-justify-main cursor-pointer">
        <span
          v-for="(tab, index) in tabList"
          :key="tab.label"
          @click="switchTab(index)"
          class="tab-item"
          :class="{ 'tab-active': index === activeTab }"
          >{{ tab.label }}</span
        >
      </div>
      <div class="table-wrapper" ref="table">
        <table>
          <thead>
            <tr>
              <td class="placeholder"></td>
              <td v-for="(item, index) in activeTableType" :key="index">
                {{ item }}
              </td>
              <td class="placeholder"></td>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, index) in tableData" :key="index" @click="changeActiveUnit(item)" class="cursor-pointer">
              <td class="placeholder"></td>
              <td v-if="activeTableType.includes('告警时间')">
                {{ item.time }}
              </td>
              <td v-if="activeTableType.includes('所属单位')">
                {{ $store.getters["sinuo/user/companyName"](item.unitID) }}
              </td>
              <td v-if="activeTableType.includes('具体位置')">
                {{ item.deviceAddress }}
              </td>
              <td v-if="activeTableType.includes('设备类型')">
                {{ item.detail }}
              </td>
              <td v-if="activeTableType.includes('告警详情')" :style="alarmColor(item.deviceType, item.alarmType)">
                {{ item.deviceType }}
                {{ item.alarmType ? `-${item.alarmType}` : "" }}
              </td>
              <td v-if="activeTableType.includes('处理状态')">
                {{ item.time }}
              </td>
              <td v-if="activeTableType.includes('用户编码')">
                {{ item.fireUserCode }}
              </td>
              <td v-if="activeTableType.includes('当前水压')">
                {{ item.voltage }}{{ 'Mpa' }}
              </td>
              <td v-if="activeTableType.includes('当前水位')">
                {{ item.level }}{{ 'm' }}
              </td>
              <td class="placeholder"></td>
            </tr>
            <tr @click="watchMore" class="watch-more-line" v-if="tableData.length>=LIST_LENGTH && activeTabType !== 'smoke'">
              <td :colspan="activeTableType.length + 2" class="cursor-pointer">查看更多</td>
            </tr>
          </tbody>
        </table>
      </div>
    </section>
  </div>
</template>

<script>
const LIST_LENGTH = 10
const TAB_TABLE_HEAD = {
  fireAlarm: ['告警时间', '所属单位', '具体位置', '设备类型', '告警详情', '用户编码'],
  waterGage: ['告警时间', '所属单位', '具体位置', '告警详情', '当前水压'],
  waterLevel: ['告警时间', '所属单位', '具体位置', '告警详情', '当前水位'],
  electricAlarm: ['告警时间', '所属单位', '具体位置', '告警详情'],
  smoke: ['告警时间', '所属单位', '具体位置', '告警详情'],
};
const audioNode = new Audio(`/audio/alarm.mp3`)
export default {
  name: 'index',
  mounted () {
    /**
     * watch ‘activeTabType’ 时 就会加载 getAlarmList
     * 所以 mounted 时 不用加载
     */
    // this.getAlarmList({ deviceTypeID: 1 });
  },
  data: function () {
    return {
      LIST_LENGTH,
      tabList: [
        {
          label: '火警',
          type: 'fireAlarm' // deviceTypeID=1
        },
        {
          label: '水压',
          type: 'waterGage' // 3
        },
        {
          label: '液位',
          type: 'waterLevel' // 4
        },
        {
          label: '电气报警',
          type: 'electricAlarm' // 2
        },
        {
          label: '独立烟感',
          type: 'smoke' // 2
        }
      ],
      activeTab: 0,
    };
  },
  computed: {
    userId () {
      return this.$store.state.sinuo.user.unitID
    },
    activeTabType () {
      return this.tabList[this.activeTab].type;
    },
    activeTableType () {
      return TAB_TABLE_HEAD[this.activeTabType];
    },
    needAlarm () {
      return this.$store.state.sinuo.functionSwitch.hasAlarm
    },
    alarmColor () {
      return (deviceType, alarmType) => {
        let color = '#fff'
        switch (deviceType) {
          case '火警系统': {
            switch (alarmType) {
              case '火警':
                color = '#EC3636'
                break
              case '复位':
                color = '#ADFF2F'
                break
              case '故障':
                color = '#EEE685'
                break
              default:
                break
            }
            break
          }
          default: {
            color = '#EC3636'
            break
          }
        }
        return {
          color
        }
      }
    },
    tableData: {
      get () {
        return this.$store.state.sinuo.dashboard.alarmList
      },
      set (val) {
        return val
      }
    }
  },
  // watch: {
  //   activeTabType: {
  //     immediate: true,
  //     handler (val) {
  //       // this.$store.commit('sinuo/dashboard/setAlarmActiveTabType', val)
  //       switch (val) {
  //         case 'fireAlarm':
  //           this.getAlarmList({ deviceTypeID: 1 });
  //           break;
  //         case 'waterGage':
  //           this.getAlarmList({ deviceTypeID: 3 });
  //           break;
  //         case 'waterLevel':
  //           this.getAlarmList({ deviceTypeID: 4 });
  //           break;
  //         case 'electricAlarm':
  //           this.getAlarmList({ deviceTypeID: 2 });
  //           break;
  //         case 'smoke':
  //           this.getAlarmList({ deviceTypeID: 5 });
  //           break;
  //         default:
  //           break;
  //       }
  //     }
  //   }
  // },
  methods: {
    getAlarmList (params) {
      // loading
      const options = {
        target: this.$refs.table,
        background: 'rgba(13, 39, 72, 1)'
      }
      let loadingInstance = this.$loading(options)
      this.$store.dispatch('sinuo/dashboard/getAlarmListFromSn', params).then(() => {
        loadingInstance.close();
      })
    },
    switchTab (index) {
      this.tableData = [];
      this.activeTab = index;
    },
    changeActiveUnit (item) {
      const unit = this.$store.state.sinuo.user.companyList.find(unit => {
        return unit.unitID === item.unitID
      })
      this.$store.commit('sinuo/dashboard/setMapActiveUnit', unit)
    },
    watchMore () {
      this.$router.push({
        path: '/alarm/all',
        query: {
          deviceTypeID: (() => {
            let id = ''
            switch (this.activeTabType) {
              case 'fireAlarm':
                id = 1;
                break;
              case 'waterGage':
                id = 3;
                break;
              case 'waterLevel':
                id = 4;
                break;
              case 'electricAlarm':
                id = 2;
                break;
              default:
                break;
            }
            return id
          })()
        }
      });
    }
  }
};
</script>

<style scoped lang="scss">
.footer {
  height: 306px;
  mask-image: url("./images/content-background2.png");
  mask-repeat: no-repeat;
  mask-position: bottom;
  .content {
    position: relative;
    .img-background {
      position: absolute;
      left: 0;
      top: 0;
      z-index: 1;
    }
    background-color: rgba(18, 58, 110, 0.7);
    height: 100%;
    width: 100%;
    .tab-wrapper {
      width: 600px;
      padding: 0 50px;
      margin: 0 auto;
      /*与图片对齐*/
      position: relative;
      z-index: 9;
      top: 3px;
      left: -10px;
      line-height: 50px;
      color: rgba(255, 255, 255, 0.3);
      .tab-item {
        padding: 0 24px;
        height: 32px;
        line-height: 32px;
        margin: 9px 0;
      }
      .tab-active {
        @extend .color-primary;
        background-image: url("/image/theme/sinuo/button/button-background-primary.png");
        background-size: 100% 100%;
        border: none;
        border-radius: 0;
      }
    }
    .table-wrapper {
      width: 100%;
      margin: 0 auto;
      position: relative;
      top: 3px;
      left: -10px;
      height: 256px; // 计算来的
      overflow-y: scroll;
      &::-webkit-scrollbar { width: 0 !important }
      table {
        width: 100%;
        text-align: center;
        tr {
          /*padding: 0 660px;*/
          width: 100%;
          height: 36px;
          line-height: 36px;
        }
        td {
          width: auto;
          font-size: 14px;
          font-weight: 500;
          color: rgba(255, 255, 255, 1);
          position: relative;
          z-index: 99;
        }
        thead {
          tr {
            td {
              color: rgba(255, 255, 255, 0.6);
            }
            background-color: rgba(13, 39, 72, 0.9);
            .placeholder {
              width: 600px;
            }
          }
        }
        tbody {
          tr {
            background-color: rgba(13, 39, 72, 0.9);
            td {
              text-overflow:ellipsis;
              white-space: nowrap;
              overflow: hidden;
              text-align: center;
              padding: 0 3px 0 3px;
            }
            &:nth-of-type(odd) {
              background-color: rgba(13, 39, 72, 0.4);
            }
          }
          .watch-more-line{
            width: 100%;
            td{
              width: 100%;
              text-align: center;
            }
          }
        }
        .color-red {
          color: rgba(229, 62, 51, 1);
        }
      }
    }
  }
}
</style>
