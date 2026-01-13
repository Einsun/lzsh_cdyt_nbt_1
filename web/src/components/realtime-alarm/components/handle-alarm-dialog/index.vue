<template>
  <div class="dialog-wrapper">
    <sn-dialog
      :visible.sync="dialogVisible"
      borderType="secondary"
      headerType="warninig"
      :title="itemInfo.alarmTitle"
      width="450px"
      :dialogProps="dialogProps"
    >
      <div class="dialog-content-wrapper flex-column flex-center">
        <div class="message-wrapper">
          <div class="alarm-message">报警时间：{{ itemInfo.time }}</div>
          <div class="alarm-message">报警单位：{{ itemInfo.unitName }}</div>
          <template v-if="itemInfo.managerName">
            <div class="alarm-message">消防主管：{{ itemInfo.managerName }}</div>
            <div class="alarm-message">主管电话：{{ itemInfo.managerPhone }}</div>
          </template>
          <template v-else>
            <div class="alarm-message">消防值班室：{{ itemInfo.unitMail }}</div>
            <div class="alarm-message">值班室电话：{{ itemInfo.unitPhone }}</div>
          </template>
          <div class="alarm-message-Highlight mt-20">已拨打电话 {{ itemInfo.managerPhone }} 通知消防主管 {{ itemInfo.managerName }}</div>
          <div v-for="item of itemInfo.employeeList" :key="item.id">
            <div class="alarm-message-Highlight">已拨打电话 {{ item.phone }} 通知值班人员 {{ item.name }}</div>
          </div>
        </div>
        <hr class="halfing-line" />
        <div class="button-wrapper">
          <sn-form
            :formList="btnConfig"
            :formStyle="{
              'margin-bottom': '0px',
              'margin-top': '0px'
            }"
            :itemStyle="{
              'margin-bottom': '0px',
              'margin-top': '0px'
            }"
            @cancel-btn-click="falseAlarm"
            @submitForm="trueFireAlarm"
          ></sn-form>
        </div>

      </div>
    </sn-dialog>
    <planDialog ref="planDialog" v-on:close="closeDialog" :itemInfo="itemInfo"></planDialog>
  </div>
</template>

<script>
import planDialog from '../plan-dialog/index'

export default {
  name: 'editItemDialog',
  components: {
    planDialog
  },
  props: {
    visible: {
      type: Boolean,
      default () {
        return false
      }
    },
    itemInfo: {
      type: Object,
      default () {
        return {}
      }
    },
  },
  data () {
    const vm = this
    return {
      itemData: {},
      dialogVisible: false,
      dialogProps: {
        'close-on-click-modal': false
      },
      btnConfig: [
        {
          type: 'button',
          options: [
            {
              label: '误报警',
              type: 'cancel'
            },
            {
              label: '真实火警',
              type: 'submit'
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
  },
  methods: {
    closeDialog () {
      this.dialogVisible = false
      this.$store.commit('sinuo/user/setPlanDialogStatus', false)
    },
    emitUpdateList (updateData) {
      this.$emit('update-list', updateData)
    },
    uploadFile (params, targetKey) {
      const file = params.file
      const data = new FormData()
      data.append('file', file)
      data.append('type', 'fm_company_pic')
      this.$api.fireManage.uploadFile({ data }).then(res => {
        this.itemData[targetKey].push(res)
        this.$refs.form.clearValidate(targetKey)
        params.onSuccess()
      }).catch(() => {
        params.onError('上传文件失败')
        // throw new Error('上传文件失败')
      })
    },
    removeFile (file, target) {
      const fileIndex = this.itemData[target].findIndex(item => {
        return item.id === file.id
      })
      this.itemData[target].splice(fileIndex, 1)
    },
    falseAlarm () {
      this.closeDialog()
    },
    trueFireAlarm () {
      // this.closeDialog()
      // this.$api.alarm.startFirePlan({ data })

      this.$store.commit('sinuo/user/setPlanDialogStatus', true)
      this.$refs.planDialog.visiblePlan = true
      this.$refs.planDialog.timeStart()
    }
  },
  watch: {
    visible: {
      immediate: true, // 解决watch props的问题
      handler (val) {
        this.dialogVisible = val
      }
    },
    dialogVisible (newVal) {
      this.$emit('closeDialog', newVal)
    },
  }
}
</script>

<style lang="scss" scoped>
@import '~@/assets/style/theme/register.scss';
.dialog-wrapper::v-deep {
  .el-dialog__body {
    padding: 40px 4px 0 !important;
  }
  .dialog-content-wrapper {
    justify-content: space-around;
    height: 100%;
    .message-wrapper{
      .alarm-message{
        color: white;
        width: 100%;
        margin: 0 auto;
        text-align: left;
        padding: 5px 0;
        font-size: 17px;
        div {
          display: inline-flex;
        }
      }
      .alarm-message-Highlight {
        color: #FF1A25;
        font-size: 17px;
        width: 100%;
        margin: 0 auto;
        text-align: left;
        padding: 5px 0;
        div {
          padding: 5px 0;
        }
      }
      >.alarm-table {
        color: white;
        background-color: #081C3D !important;
        tbody {
          tr:hover>td {
            background-color: #081C3D !important
          }
        }
        tr {
          background-color: #081C3D !important;
        }
        th {
          background-color: #081C3D !important;
          border-bottom: 0 !important;
          color: white;
          padding: 8px 0 !important;
          >.cell {
            padding-left: 0 !important;
          }
        }
        th.is-leaf {
          border-bottom: 0 !important;
        }
        td {
          border-bottom: 0 !important;
          padding: 8px 0 !important;
          >.cell {
            padding-left: 0 !important;
          }
        }
      }
      >.alarm-table::before {
        height: 0px !important;
      }
    }
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
    margin: 60px 0 15px;
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
    width: 60px !important;
  }
  .form-wrapper{
    .el-form{
      .el-form-item.is-required:not(.is-no-asterisk) .el-form-item__label-wrap>.el-form-item__label::before,
      .el-form-item.is-required:not(.is-no-asterisk)>.el-form-item__label::before{
        content: '' !important;
        font-size: 0px !important;
      }
    }
  }
}
</style>
