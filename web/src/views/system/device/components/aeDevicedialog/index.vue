<template>
  <div class="dialog-wrapper">
    <sn-dialog :visible.sync="dialogVisible" :title="dialogHeader" width="460px" borderType="secondary" headerType="secondary">
      <div class="dialog-content-wrapper flex-column flex-center">
        <sn-form
          :formList="planConfig"
          ref="form"
          v-model="infoData"
          :formValue="infoData"
          :itemStyle="{
            'margin-bottom': '40px'
          }"
        ></sn-form>
        <hr class="halfing-line"/>
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
import aeRule from "@/request/modules/aeRule";
import aeDevices from "@/request/modules/aeDevice";

export default {
  name: 'editPlanDialog',
  props: {
    visible: {
      type: Boolean,
      default () {
        return false
      }
    },
    productLineId: {
      type: Number,
      default () {
        return 0
      }
    },
    aeType: {
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
      aeRuleList: [],
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
          type: 'input',
          value: '',
          label: '设备编号',
          key: 'sn',
          rule: {
            required: true,
          },
        },
        {
          type: 'select',
          label: '声发射规则',
          key: 'aeRuleId',
          rule: {
            required: true,
          },
          options: [],
          style: {
            display: 'inline-flex'
          }
        },
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
    dialogHeader () {
      return (Object.keys(this.info).length === 0 ? '添加' : '修改') + '声发射设备'
    },
  },
  created () {
    this.getCommDeviceList()
    this.getGatherRuleList()
  },
  methods: {
    closeDialog () {
      this.dialogVisible = false
    },
    emitUpdateList () {
      this.$emit('update-list')
    },
    getCommDeviceList () {
      const params = {
        id: this.productLineId,
        type: 1
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
        this.planConfig[3].options = this.commDevices
      })
    },
    getGatherRuleList () {
      const params = {
      }
      aeRule.request({
        params
      }).then(res => {
        this.aeRuleList = res.data.map(item => {
          if (item.name) {
            return {
              label: item.name,
              value: item.id
            }
          }
        })
        this.planConfig[2].options = this.aeRuleList
      })
    },
    submitForm () {
      this.$refs.form.submitForm().then(() => {
        const isEdit = Object.keys(this.info).length !== 0
        if (isEdit) {
          const id = this.info.id
          const productLineId = this.productLineId;
          const data = { ...this.infoData, id, productLineId }
          aeDevices.aeUpdate({
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
          aeDevices.aeRequest({
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
