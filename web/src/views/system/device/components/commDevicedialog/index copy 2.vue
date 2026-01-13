<template>
  <div class="dialog-wrapper">
    <sn-dialog :visible.sync="dialogVisible" :title="dialogHeader" width="450px" borderType="secondary" headerType="secondary">
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
          label: '设备类型',
          key: 'type',
          rule: {
            required: true,
          },
          options: [
            {
              label: '采集网关',
              value: 0,
            },
            {
              label: '声发射',
              value: 1,
            }
          ],
          style: {
            display: 'inline-flex'
          }
        },
        {
          type: 'input',
          label: '设备编号',
          key: 'sn',
          rule: {
            required: false,
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
    dialogHeader () {
      return (Object.keys(this.info).length === 0 ? '添加' : '修改') + '传输设备'
    }
  },
  methods: {
    closeDialog () {
      this.dialogVisible = false
    },
    emitUpdateList () {
      this.$emit('update-list')
    },
    submitForm () {
      this.$refs.form.submitForm().then(() => {
        const isEdit = Object.keys(this.info).length !== 0
        const productLineId = this.productLineId;
        if (isEdit) {
          const id = this.info.id
          const data = { ...this.infoData, id, productLineId }
          commDevice.update({
            method: 'POST',
            data
          }).then(res => {
            this.closeDialog()
            this.emitUpdateList()
          })
        } else {
          const data = { ...this.infoData, productLineId }
          commDevice.request({
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
