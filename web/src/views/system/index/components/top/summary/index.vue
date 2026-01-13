<template>
  <div class="summary-wrapper">
    <div class="item-wrapper flex-column">
      <div
        class="item flex-justify-main flex-center-cross"
        v-for="item of list"
        :key="item.label"
        @click="openDetail(item)"
      >
        <div class="border row1"></div>
        <div class="border row2"></div>
        <div class="border col1"></div>
        <div class="border col2"></div>
        <p class="label">{{ item.label }}</p>
        <p class="value flex-center-cross">
          <span class="number">{{ item.value }}</span>
          <span>{{ item.unit }}</span>
        </p>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "summary",
  data () {
    return {
      list: [
        {
          key: "unitNumber",
          label: "接入单位",
          unit: "家",
          value: 0,
          type: 'record/company-information'
        },
        {
          value: 0,
          key: "SDCount",
          label: "联网主机",
          unit: "台",
          type: 'device/device-overview'
        },
        {
          value: 0,
          key: "gageDeviceCount",
          label: "水系统压力",
          unit: "套",
          type: 'device/water-gage'
        },
        {
          value: 0,
          key: "levelDeviceCount",
          label: "水系统液位",
          unit: "个",
          type: 'device/water-level'
        },
        {
          value: 0,
          key: "FACount",
          label: "电气火灾",
          unit: "个",
          type: 'device/electronic-fire'
        },
        {
          value: 0,
          key: "videoCount",
          label: "视频监控",
          unit: "个",
          type: 'device/video-equipment'
        },
        {
          value: 0,
          key: "alarmCount",
          label: "当年火警累计",
          unit: ""
        },
        {
          value: 0,
          key: "smokeCount",
          label: "无线烟感",
          unit: "个"
        }
      ]
    };
  },
  computed: {
    unitLength () {
      return this.$store.state.sinuo.user.companyList.length;
    }
  },
  watch: {
    unitLength (val) {
      console.log(val);
    }
  },
  mounted () {
    this.getSummary();
  },
  methods: {
    openDetail (data) {
      if (data.type) {
        this.$router.push({
          path: data.type
        })
      }
    },
    getSummary () {
      this.$api.index.getSummaryData().then(res => {
        console.log(res);
        for (const item of this.list) {
          if (item.key === "unitNumber") {
            item.value = this.unitLength;
          } else {
            item.value = res[item.key];
          }
        }
      });
    }
  }
};
</script>

<style scoped lang="scss">
@import "~@/assets/style/theme/register.scss";
.summary-wrapper {
  .item-wrapper {
    height: 106px;
    flex-wrap: wrap;
    align-content: space-between;
    justify-content: space-between;
    .item {
      // @include corner-line-borders(6px, 2px, #2ec0f3);
      width: 220px;
      height: 48px;
      background: rgba(6, 39, 91, 0.7);
      cursor: pointer;
      font-size: 16px;
      font-weight: 400;
      color: rgba(216, 216, 218, 0.7);
      padding: 0 18px;
      position: relative;
      .border {
        position: absolute;
        padding: 3px;
      }
      .row1 {
        top: -1px;
        left: -2px;
        border-left: 2px solid #2ec0f3;
        border-top: 2px solid #2ec0f3;
      }
      .row2 {
        top: -1px;
        right : -2px;
        border-right: 2px solid #2ec0f3;
        border-top: 2px solid #2ec0f3;
      }
      .col1 {
        bottom: -2px;
        left: -2px;
        border-left: 2px solid #2ec0f3;
        border-bottom: 2px solid #2ec0f3;
      }
      .col2 {
        bottom: -2px;
        right: -2px;
        border-right: 2px solid #2ec0f3;
        border-bottom: 2px solid #2ec0f3;
      }
      // &:last-of-type {
      //   height: 100%;
      //   display: inline-flex;
      //   flex-direction: column;
      //   justify-content: center;
      //   align-items: flex-start;
      //   .label {
      //     margin-bottom: 15px;
      //   }
      // }
      .value {
        .number {
          font-size: 36px;
          font-weight: 500;
          color: rgba(46, 192, 243, 1);
          margin-right: 8px;
        }
      }
    }
  }
}
</style>
