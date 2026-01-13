<template>
  <div class="dialog-wrapper">
    <sn-dialog
      :visible.sync="dialogVisible"
      :title="dialogHeader"
      width="850px"
      borderType="secondary"
      headerType="secondary"
    >
      <div class="dialog-content-wrapper flex-column flex-center">
        <sn-form
          inline
          :formList="planConfig"
          ref="form"
          v-model="infoData"
          :formValue="infoData"
          :itemStyle="{
            'margin-bottom': '40px'
          }"
        ></sn-form>
        <!--        <hr class="halfing-line"/>-->
        <div class="button-wrapper">
          <sn-form
            :formList="btnConfig"
            @cancel-btn-click="closeDialog"
            @submitForm="submitForm"
            :itemStyle="{
              'margin-bottom': '0px',
              'margin-top': '0px'
            }"
            :formStyle="{
              'margin-bottom': '0px',
              'margin-top': '0px'
            }"
          ></sn-form>
        </div>
      </div>
    </sn-dialog>
  </div>
</template>

<script>
import commDevice from "@/request/modules/commDevice";
import alarmRule from "@/request/modules/alarmRule";
import gatherRule from "@/request/modules/gatherRule";
import aeDevices from "@/request/modules/aeDevice";
import aeRule from "@/request/modules/aeRule";
import signal from "@/request/modules/signal";

export default {
  name: 'editPlanDialog',
  props: {
    visible: {
      type: Boolean,
      default() {
        return false
      }
    },
    productLineId: {
      type: Number,
      default() {
        return 0
      }
    },
    aeType: {
      type: Number,
      default() {
        return 0
      }
    },
    info: {
      type: Object,
      default() {
        return {};
      }
    }
  },
  data() {
    return {
      infoData: {},
      dialogVisible: false,
      alarmRulesList: [],
      gatherRulesList: [],
      signalList: [],
      planConfig: [
        {
          type: 'input',
          value: '',
          label: '设备名称',
          key: 'name',
          rule: {
            required: true,
          },
        },
        {
          type: 'select',
          label: '报警规则',
          key: 'alarmRuleId',
          rule: {
            required: true,
          },
          options: [],
          style: {
            display: 'inline-flex'
          }
        },
        // {
        //   type: 'select',
        //   label: '采集规则',
        //   key: 'gatherRuleId',
        //   rule: {
        //     required: true,
        //   },
        //   options: [],
        //   style: {
        //     display: 'inline-flex'
        //   }
        // },
        // {
        //   type: 'select',
        //   label: '声发射规则',
        //   key: 'aeRuleId',
        //   rule: {
        //     required: true,
        //   },
        //   options: [],
        //   style: {
        //     display: 'inline-flex'
        //   }
        // },
        {
          type: 'select',
          label: '传输设备',
          key: 'commDeviceId',
          rule: {
            required: true,
          },
          options: [],
          style: {
            display: 'inline-flex'
          }
        },
        // {
        //   type: 'input',
        //   label: '设备编号',
        //   key: 'sn',
        //   rule: {
        //
        //   },
        // },
        {
          type: 'input',
          label: '485地址',
          key: 'address',
          rule: {

          },
        },
        {
          type: 'select',
          label: '485通道',
          key: 'pos',
          rule: {
            required: true,
          },
          options: [
            {
              label: '通道0',
              value: 0,
            },
            {
              label: '通道1',
              value: 1,
            },
            {
              label: '通道2',
              value: 2,
            },
            {
              label: '通道3',
              value: 3,
            },
            {
              label: '通道4',
              value: 4,
            },
            {
              label: '通道5',
              value: 5,
            },
            {
              label: '通道6',
              value: 6,
            },
            {
              label: '通道7',
              value: 7,
            },
          ],
          style: {
            display: 'inline-flex'
          }
        },
        {
          type: 'select',
          label: '信号选择',
          key: 'signalTypeId',
          rule: {
            required: true,
          },
          options: [
          ],
          style: {
            display: 'inline-flex'
          }
        },
        // {
        //   type: 'input',
        //   label: '最大值',
        //   key: 'maxValue',
        //   rule: {
        //   },
        // },
        // {
        //   type: 'input',
        //   label: '最小值',
        //   key: 'minValue',
        //   rule: {
        //   },
        // },
        {
          type: 'select',
          label: '采样位11',
          key: 'samping',
          rule: {
            required: true,
          },
          options: [
            {
              label: 10,
              value: 10,
            },
            {
              label: 12,
              value: 12,
            },
            {
              label: 16,
              value: 16,
            },

          ],
          style: {
            display: 'inline-flex'
          }
        },
        {
          type: 'input',
          label: '量程最大值',
          key: 'maxValue',
          rule: {
          },
        },
        {
          type: 'input',
          label: '量程最小值',
          key: 'minValue',
          rule: {
          },
        },
        {
          type: 'select',
          label: '单位',
          key: 'unit',
          options: [],
          rule: {
          },
        },
      ],
      btnConfig: [
        {
          type: 'button',
          options: [
            {
              label: '取消',
              type: 'cancel',
            },
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
    dialogHeader() {
      let aeType = Number(this.aeType)
      let data = [
        [{ label: '℃', value: '℃' }, { label: '℉', value: '℉' }],
        [{ label: 'Pa', value: 'Pa' }, { label: 'Hpa', value: 'Hpa' }, { label: 'Kpa', value: 'Kpa' }, { label: 'Mpa', value: 'Mpa' },],
        [{ label: 'Nm³/h', value: 'Nm³/h' }, { label: 'Nm³/min', value: 'Nm³/min' }, { label: 'slm', value: 'slm' }],
        [{ label: 'mm', value: 'mm' }, { label: 'mm/s', value: 'mm/s' }, { label: 'mm/(s^2)', value: 'mm/(s^2)' }],
      ]
      this.planConfig[9].options = data[aeType - 1]
      console.log(this.planConfig);
      // 设置单位数据
      let label = ['声发射', '温度', '压力', '流量', '振动'][aeType]
      return label + (Object.keys(this.info).length === 0 ? '添加' : '修改')
    },
  },
  created() {
    console.log(this.aeType)
    this.getCommDeviceList()
    this.getAlarmRuleList()
    // this.getGatherRuleList()
    this.getSignalList()
  },
  methods: {
    closeDialog() {
      this.dialogVisible = false
    },
    emitUpdateList() {
      this.$emit('update-list')
    },
    getCommDeviceList() {
      const params = {
        id: this.productLineId,
        type: 0
      }
      commDevice.requestTypeCommDevice({
        params
      }).then(res => {
        this.commDevices = res.data.map(item => {
          return {
            label: item.name,
            value: item.id
          }
        })
        this.planConfig[2].options = this.commDevices
      })
    },
    getAlarmRuleList() {
      const params = {
        productLineId: this.productLineId,
        type: this.aeType - 1
      }
      alarmRule.requestTypeRules({
        params
      }).then(res => {
        this.alarmRulesList = res.data.map(item => {
          return {
            label: item.name,
            value: item.id
          }
        })
        // this.alarmRuleList = res.data
        this.planConfig[1].options = this.alarmRulesList
        // console.log(this.alarmRulesList);
      })
    },
    // getGatherRuleList() {
    //   if (this.aeType === 0) {
    //     const params = {
    //     }
    //     aeRule.request({
    //       params
    //     }).then(res => {
    //       this.gatherRuleList = res.data.map(item => {
    //         return {
    //           label: item.name,
    //           value: item.id
    //         }
    //       })
    //       this.planConfig[2].options = this.gatherRulesList
    //     })
    //   } else {
    //     const params = {
    //     }
    //     gatherRule.request({
    //       params
    //     }).then(res => {
    //       this.gatherRulesList = res.data.map(item => {
    //         return {
    //           label: item.name,
    //           value: item.id
    //         }
    //       })
    //       this.planConfig[2].options = this.gatherRulesList
    //       // console.log(this.gatherRulesList);
    //     })
    //   }
    // },
    getSignalList() {
      const params = {
      }
      signal.request({
        params
      }).then(res => {
        this.signalList = res.data.map(item => {
          return {
            label: item.name,
            value: item.id
          }
        })
        this.planConfig[5].options = this.signalList
        // console.log(this.alarmRulesList);
      })
    },
    submitForm() {
      this.$refs.form.submitForm().then(() => {
        const isEdit = Object.keys(this.info).length !== 0
        if (isEdit) {
          const id = this.info.id
          const type = this.aeType;
          const productLineId = this.productLineId;
          const data = { ...this.infoData, id, productLineId, type }
          console.log(this.info, this.infoData, data);
          aeDevices.update({
            method: 'POST',
            data
          }).then(res => {
            this.closeDialog()
            this.emitUpdateList()
          })
        } else {
          const productLineId = this.productLineId;
          const type = this.aeType;
          const data = { ...this.infoData, productLineId, type }
          aeDevices.request({
            method: 'POST',
            data
          }).then(res => {
            this.closeDialog()
            this.emitUpdateList()
          })
        }
      })
    }
  },
  watch: {
    visible: {
      immediate: true, // 解决watch props的问题
      handler(val) {
        this.dialogVisible = val;
      }
    },
    dialogVisible(newVal) {
      this.$emit('update:visible', newVal)
    },
    info: {
      immediate: true,
      handler(newVal) {
        this.infoData = newVal
        console.log(this.infoData, 999)
      }
    },
    aeType: {
      immediate: true,
      handler(newVal) {
        this.aeType = newVal
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/style/theme/register.scss";
.dialog-wrapper::v-deep {
  .el-dialog__body {
    padding: 40px 4px 0 !important;
  }
  .dialog-content-wrapper {
    justify-content: space-around;
    height: 100%;
  }
  .el-input__inner,
  .el-input-number {
    height: 36px !important;
    width: 300px !important;
  }
  .halfing-line {
    /* 逐步尝试值 */
    width: 435px;
    height: 1px;
    background-color: rgba(101, 226, 245, 0.1);
    border: none;
    margin: 302px 0 15px;
  }
  .button-wrapper {
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
