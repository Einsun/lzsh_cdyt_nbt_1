<template>
  <div>
    <sn-dialog
      :visible.sync="visiblePlan"
      :title="'启动预案'"
      width="450px"
      class="planDialog"
      :dialogProps="dialogProps"
    >

    <div>
      <div class="timer-group">
        <div class="timer hour">
          <div class="hand"><span :class="stop(str)"></span></div>
          <div class="hand"><span :class="stop(str)"></span></div>
        </div>
        <div class="timer minute">
          <div class="hand"><span :class="stop(str)"></span></div>
          <div class="hand"><span :class="stop(str)"></span></div>
        </div>
        <div class="timer second">
          <div class="hand"><span :class="stop(str)"></span></div>
          <div class="hand"><span :class="stop(str)"></span></div>
        </div>
        <div class="face">
          <p>{{ str }}</p>
        </div>
      </div>

      <div class="content-plan">{{ '消防预案倒计时，两分钟后将会自启动，如果需要立即启动，请点击“立即启动”，如果需要终止预案，请点击“终止启动”。' }}</div>

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
          @cancel-btn-click="cancel"
          @submitForm="startFirePlan"
        ></sn-form>
      </div>
    </div>

    </sn-dialog>
    <planInfo ref="planInfoDialog" v-on:close="closeDialog" :unitId="itemInfo.unitID" :address="itemInfo.alarmWhere"></planInfo>
  </div>
</template>

<script>
import planInfo from './planInfo'

export default {
  components: {
    planInfo
  },
  props: {
    itemInfo: {
      type: Object,
      default () {
        return {}
      }
    }
  },
  data () {
    return {
      visiblePlan: false,
      hour: 0,
      minute: 0,
      ms: 0,
      second: 0,
      time: "",
      str: "02:00",
      maxtime: 120,
      btnConfig: [
        {
          type: 'button',
          options: [
            {
              label: '终止启动',
              type: 'cancel'
            },
            {
              label: '立即启动',
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
  computed: {
    stop () {
      return function (val) {
        if (val === "00:00") {
          return 'stopAnimation'
        } else {
          return ''
        }
      }
    }
  },
  methods: {
    closeDialog () {
      this.visiblePlan = false
      this.$emit("close")
    },
    timeStart () {
      // this.time = setInterval(this.timer, 50)
      this.time = setInterval(this.CountDown, 1000)
    },
    CountDown () {
      if (this.maxtime >= 0) {
        this.minute = Math.floor(this.maxtime / 60)
        this.second = Math.floor(this.maxtime % 60)
        this.str = this.toDub(this.minute) + ':' + this.toDub(this.second)
        this.maxtime = this.maxtime - 1
      } else {
        clearInterval(this.time);
      }
    },
    toDub (n) {
      if (n < 10) {
        return "0" + n
      } else {
        return "" + n
      }
    },
    reset () {
      clearInterval(this.time)
      this.hour = 0
      this.minute = 0
      this.ms = 0
      this.second = 0
      this.str = "00:00:00"
    },
    cancel () {
      this.closeDialog()
    },
    startFirePlan () {
      let unit = (this.itemInfo.unitID.split("#"))[1]
      this.$refs.planInfoDialog.getTableList(unit)
      this.$refs.planInfoDialog.visiblePlanInfo = true
      this.$refs.planInfoDialog.tranAgain()
    }
  },
  watch: {
    str (newVal, oldVal) {
      if (newVal === "00:00") {
        this.startFirePlan()
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.planDialog {
  .el-dialog__wrapper {
    .el-dialog {
      .el-dialog__body {
        .button-wrapper {
          padding-top: 40px !important;
          width: 435px;
          display: flex !important;
          justify-content: center !important;
        }
        .content-plan {
          padding-top: 40px !important;
          color: white;
          font-size: 18px;
          line-height: 30px;
        }
      }
    }
  }
}
.stopAnimation {
  animation-play-state: paused;
}
</style>

<style>
.planDialog .el-dialog__wrapper .el-dialog {
  background: #111;
}

.planDialog .el-dialog__wrapper .el-dialog .el-dialog__body {
  padding: 40px 4px 40px 4px !important;
}

.timer-group {
  height: 400px;
  margin: 0 auto;
  position: relative;
  width: 400px;
}

.timer {
  border-radius: 50%;
  height: 100px;
  overflow: hidden;
  position: absolute;
  width: 100px;
}

.timer:after {
  background: #111;
  border-radius: 50%;
  content: "";
  display: block;
  height: 80px;
  left: 10px;
  position: absolute;
  width: 80px;
  top: 10px;
}

.timer .hand {
  float: left;
  height: 100%;
  overflow: hidden;
  position: relative;
  width: 50%;
}

.timer .hand span {
  border: 50px solid rgba(234, 31, 25, .4);
  border-bottom-color: transparent;
  border-left-color: transparent;
  border-radius: 50%;
  display: block;
  height: 0;
  position: absolute;
  right: 0;
  top: 0;
  transform: rotate(225deg);
  width: 0;
}

.timer .hand:first-child {
  transform: rotate(180deg);
}

.timer .hand span {
  animation-duration: 4s;
  animation-iteration-count: infinite;
  animation-timing-function: linear;
}

.timer .hand:first-child span {
  animation-name: spin1;
}

.timer .hand:last-child span {
  animation-name: spin2;
}

.timer.hour {
  background: rgba(0, 0, 0, .3);
  height: 400px;
  left: 0;
  width: 400px;
  top: 0;
}

.timer.hour .hand span {
  animation-duration: 121s;
  border-top-color:rgba(234, 31, 25, 1);
  border-right-color: rgba(234, 31, 25, 1);
  border-width: 200px;
}

.timer.hour:after {
  height: 360px;
  left: 20px;
  width: 360px;
  top: 20px;
}

.timer.minute {
  background: rgba(0, 0, 0, .2);
  height: 350px;
  left: 25px;
  width: 350px;
  top: 25px;
}

.timer.minute .hand span {
  animation-duration: 60500ms;
  border-top-color:rgba(255, 201, 86, 1);
  border-right-color: rgba(255, 201, 86, 1);
  border-width: 175px;
}

.timer.minute:after {
  height: 310px;
  left: 20px;
  width: 310px;
  top: 20px;
}

.timer.second {
  background: rgba(0, 0, 0, .2);
  height: 300px;
  left: 50px;
  width: 300px;
  top: 50px;
}

.timer.second .hand span {
  animation-duration: 1s;
  border-top-color: rgba(101, 226, 245, 1);
  border-right-color: rgba(101, 226, 245, 1);
  border-width: 150px;
}

.timer.second:after {
  height: 296px;
  left: 2px;
  width: 296px;
  top: 2px;
}

.face {
  background: rgba(0, 0, 0, .1);
  border-radius: 50%;
  height: 296px;
  left: 52px;
  position: absolute;
  width: 296px;
  text-align: center;
  top: 52px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.face p {
  border-radius: 20px;
  font-size: 50px;
  font-weight: 400;
  width: 100%;
  color: white;
}

@keyframes spin1 {
  0% {
    transform: rotate(225deg);
  }
  50% {
    transform: rotate(225deg);
  }
  100% {
    transform: rotate(405deg);
  }
}

@keyframes spin2 {
  0% {
    transform: rotate(225deg);
  }
  50% {
    transform: rotate(405deg);
  }
  100% {
    transform: rotate(405deg);
  }
}

</style>
