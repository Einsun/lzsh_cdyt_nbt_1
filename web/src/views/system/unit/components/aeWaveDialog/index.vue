<template>
  <div class="dialog-wrapper">
    <sn-dialog :visible.sync="dialogVisible" :title="dialogHeader" width="1600px" borderType="secondary" headerType="secondary">
      <div class="dialog-content-wrapper flex-column flex-center">
        <div class="content" style="width: 1400px">
          <sn-histogram :chartData="chartData"></sn-histogram>
        </div>
      </div>
    </sn-dialog>
  </div>
</template>

<script>
import snHistogram from './sn-histogram'
import dashCard from "@/views/system/unit/components/card/index.vue";
import waveData from "@/request/modules/waveData";

export default {
  name: 'editPlanDialog',
  components: {
    snHistogram
  },
  props: {
    visible: {
      type: Boolean,
      default () {
        return false
      }
    },
    deviceId: {
      type: Number,
      default () {
        return 0
      }
    },
    info: {
      type: Object,
      default () {
        return {};
      }
    }
  },
  data () {
    return {
      infoData: {},
      dialogVisible: false,
      alarmRulesList: [],
      gatherRulesList: [],
      dataList: [],
      btnConfig: [
        {
          type: 'button',
          options: [
            {
              label: '确定',
              type: 'submit',
            }
          ],
          style: {
            display: 'inline-flex'
          }
        }
      ]
    }
  },
  computed: {
    dialogHeader () {
      return '波形数据'
    },
    chartData () {
      const columns = this.dataList.map(function (value, index) {
        return index;
      });
      const rows = this.dataList
      return {
        columns,
        rows
      }
    }
  },
  created () {
  },
  mounted () {
    this.getWaveData()
  },
  methods: {
    getWaveData () {
      const params = {
        deviceid: this.deviceId,
      }
      waveData.live({
        params
      }).then(res => {
        console.log(res)
        this.dataList = res.data.data;
        console.log(this.dataList)
        // this.chartData = dataArray.map(function (value, index) {
        //   return [index, value];
        // });
      })
    },
    closeDialog () {
      this.dialogVisible = false
    },
    emitUpdateList () {
      this.$emit('update-list')
    },
    submitForm () {
      this.closeDialog()
      this.emitUpdateList()
    }
  },
  watch: {
    visible: {
      immediate: true, // 解决watch props的问题
      handler (val) {
        this.dialogVisible = val;
      }
    },
    dialogVisible (newVal) {
      this.$emit('update:visible', newVal)
    },
    info: {
      immediate: true,
      handler (newVal) {
        this.infoData = newVal
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/style/theme/register.scss";
.dialog-wrapper::v-deep{
  .el-dialog__body{
    padding: 40px 4px 0 !important;
  }
  .dialog-content-wrapper{
    justify-content: space-around;
    height: 100%;
  }
  .el-input__inner,.el-input-number{
    height: 36px !important;
    width: 300px !important;
  }
  .halfing-line{
    /* 逐步尝试值 */
    width: 435px;
    height: 1px;
    background-color: rgba(101, 226, 245, .1);
    border: none;
    margin: 302px 0 15px;
  }
  .button-wrapper{
    width: 435px;
    display: flex;
    justify-content: flex-end;
  }
  .el-date-editor--date {
    width: 300px !important;
    background-position-x: 272px !important;
  }
  .el-form-item__label {
    width: 80px !important;
  }
}
</style>
