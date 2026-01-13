<template>
  <div>
    <sn-dialog
      :visible.sync="visiblePlanInfo"
      :title="'灭火预案'"
      width="450px"
      class="planDialog"
      :dialogProps="dialogProps"
    >
      <div class="dialog-plan-wrapper flex-column flex-center">
        <div class="message-wrapper">
          <template v-for="item of planInfo">
            <div :key="item.id" class="box">
              <div class="alarm-message">组织名称：{{ item.organization_name }}</div>
              <div class="alarm-message">组织责任：{{ item.organization_duty }}</div>
              <div class="alarm-message">负责人员：{{ item.responsible }}</div>
              <div class="alarm-message">联系电话：{{ item.phone }}</div>
              <div class="alarm-message-Highlight">已向 {{ item.responsible }} 发送短信通知！！！</div>
            </div>
          </template>
        </div>

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
            @cancel-btn-click="confirmTran"
            @submitForm="tranAgain"
          ></sn-form>
        </div>
      </div>
    </sn-dialog>
  </div>
</template>

<script>

export default {
  props: {
    unitId: {
      type: String,
      default () {
        return {}
      }
    },
    address: {
      type: String,
      default () {
        return {}
      }
    },
  },
  data () {
    return {
      visiblePlanInfo: false,
      tablePage: {
        page: 1,
        perPage: 100,
        total: 0
      },
      planInfo: [],
      btnConfig: [
        {
          type: 'button',
          options: [
            {
              label: '确认收到通知',
              type: 'cancel'
            },
            {
              label: '再次发送通知',
              type: 'submit'
            }
          ],
          style: {
            display: 'inline-flex'
          }
        }
      ],
      dialogProps: {
        'close-on-click-modal': false,
        'show-close': false
      }
    }
  },
  methods: {
    closeDialog () {
      this.visiblePlanInfo = false
      this.$emit("close")
    },
    getTableList (unitId) {
      const { page, perPage } = this.tablePage
      const unitID = unitId
      const params = {
        page,
        perPage,
        unitID
      }
      this.$store.dispatch('sinuo/fireControl/getFirePlanList', params).then(res => {
        this.planInfo = res.data
      })
    },
    confirmTran () {
      this.closeDialog()
    },
    tranAgain () {
      var unitId = this.unitId
      var address = this.address
      var data = {
        unitId,
        address
      }
      this.$store.dispatch('sinuo/fireControl/pushFireMessage', data).then(res => {
        // console.log('***** tranAgain ********', res)
      })
    }
  },
  mounted () {
    // this.getTableList()
  }
}
</script>

<style lang="scss" scoped>
// @import '~@/assets/style/theme/register.scss';
.dialog-wrapper {
  .el-dialog__body {
    padding: 40px 4px 0 !important;
    .dialog-plan-wrapper {
      justify-content: space-around;
      height: 100%;
      .message-wrapper {
        width: 100%;
        .box {
          padding-bottom: 20px;
          border-bottom: 1px solid rgba(101, 226, 245, 0.1);;
          .alarm-message {
            font-size: 18px;
            color: white;
            width: 100%;
            margin: 10px auto;
            text-align: left;
            padding: 5px 0;
          }
        }
        .alarm-message-Highlight {
          color: #FF1A25;
          font-size: 18px;
          width: 100%;
          margin: 10px auto;
          text-align: left;
          padding: 5px 0;
        }
      }
      .button-wrapper {
        padding-top: 40px !important;
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
