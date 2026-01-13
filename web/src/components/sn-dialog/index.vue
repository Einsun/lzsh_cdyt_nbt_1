<template>
  <div class="el-dialog-wrapper">
    <el-dialog :visible.sync="innerVisible" :title="title" :class="[headerClass, borderClass, flashing]" @close="onClose" :width="width" v-bind="dialogProps">
      <div class="slotWrapper">
        <slot></slot>
      </div>
    </el-dialog>
  </div>
</template>

<style lang="scss" scoped>
  .el-dialog-wrapper::v-deep {
    /*border样式定义*/
    .border-primary{
      .el-dialog{
        margin-top: 15vh;
        border-image-source: url("images/popup-background-border-primary.png");
        border-image-outset: 0px;
        border-width: 22px;
        border-radius: 0;
        border-image-repeat: round;
        border-style: solid;
        border-image-slice: 22 22 22 22 fill;
        border-image-width: 22px;
        position: relative;
        box-shadow: 0 0;
        background-color: rgba(255, 255, 255, 0);
        background-size: cover;
        .el-dialog__body {
          padding: 20px 32px 27px;
        }
      }
    }
    .border-secondary{
      .el-dialog{
        margin-top: 15vh;
        border-image-source: url("images/popup-background-border-secondary.svg");
        border-image-outset: 0px;
        border-width: 22px;
        border-radius: 0;
        border-image-repeat: stretch;
        border-style: solid;
        border-image-slice: 50 fill;
        border-image-width: 50px;
        position: relative;
        box-shadow: 0 0;
        background-color: rgba(255, 255, 255, 0);
        background-size: cover;
        .el-dialog__body {
          padding: 20px 32px 27px;
        }
      }
    }
    /*header 样式定义*/
    .header-none {
      .el-dialog__header {
        /*display: none;*/
      }
    }
    .header-primary {
      .el-dialog__header {
        padding: 0;
        text-align: center;
        height: 58px;

        background-image: url("images/popup-header-primary.png");
        background-repeat: no-repeat;
        background-position-x: center;

        margin-top: -50px;

        .el-dialog__title {
          line-height: 58px;
          font-size: 20px;
          font-weight: 500;
          color: rgba(101, 226, 245, 1);
        }

        .el-dialog__headerbtn {
          position: absolute;
          top: 3px;
          right: 0px;

          .el-dialog__close {
            font-size: 19px;

            //color: $color-secondary;
            color: #65e2f5;
          }
        }
      }
    }
    .header-secondary {
      .el-dialog__header {
        background-image: url("images/pop-header-secondary.png");
        background-repeat: no-repeat;
        background-position-x: center;
        padding: 0;
        text-align: center;
        height: 38px;
        margin-top: -20px;
        .el-dialog__title {
          font-size: 14px;
          font-weight: 500;
          color: rgba(255, 255, 255, 1);
          line-height: 38px;
        }
        .el-dialog__headerbtn {
          display: none;
        }
      }
    }
    .header-warning {
      .el-dialog__header {
        background-image: url("images/pop-header-secondary.png");
        background-repeat: no-repeat;
        background-position-x: center;
        padding: 0;
        text-align: center;
        height: 38px;
        margin-top: -20px;
        .el-dialog__title {
          font-size: 16px;
          font-weight: 600;
          color: #FF1A25;
          line-height: 38px;
        }
        .el-dialog__headerbtn {
          display: none;
        }
      }
      .el-dialog__body {
        .el-button--submit {
          color:#FF1A25 !important;
          font-weight: 600;
          animation: glow 300ms ease-out infinite alternate;
        }
      }
    }
  }
  @keyframes glow {
    0% {
      border-color: #FF1A25;
      box-shadow: 0 0 2px rgba(234,31,25,.5), inset 0 0 2px rgba(234,31,25,.5), 0 0 0 #FF1A25;
    }
    100% {
      border-color: #FF1A25;
      box-shadow: 0 0 10px rgba(234,31,25,.9), inset 0 0 10px rgba(234,31,25,.9), 0 0 0 #FF1A25;
    }
  }
</style>

<style>
.flashing {
  animation: glow 300ms ease-out infinite alternate;
  height: 100%;
}
@keyframes glow {
  0% {
    border-color: #FF1A25;
    box-shadow: 0 0 200px rgba(234,31,25,.5), inset 0 0 200px rgba(234,31,25,.5), 0 0 0 #FF1A25;
  }
  100% {
    border-color: #FF1A25;
    box-shadow: 0 0 1000px rgba(234,31,25,.9), inset 0 0 1000px rgba(234,31,25,.9), 0 0 0 #FF1A25;
  }
}
</style>

<script>
export default {
  data () {
    return {
      innerVisible: false,
    };
  },
  props: {
    visible: {
      type: Boolean,
      default () {
        return false;
      }
    },
    width: {
      type: String,
      default () {
        return '1164px';
      }
    },
    headerType: {
      type: String,
      default () {
        return 'primary';
      }
    },
    borderType: {
      type: String,
      default () {
        return 'primary';
      }
    },
    title: {
      type: String
    },
    dialogProps: {
      type: Object,
      default () {
        return {}
      }
    }
  },
  computed: {
    headerClass () {
      return {
        'header-primary': this.headerType === 'primary',
        'header-secondary': this.headerType === 'secondary',
        'header-none': this.headerType === 'none',
        'header-warning': this.headerType === 'warninig'
      };
    },
    borderClass () {
      return {
        'border-primary': this.borderType === 'primary',
        'border-secondary': this.borderType === 'secondary',
      };
    },
    flashing () {
      if (this.title === '发现疑似火警!!!') {
        return 'flashing'
      }
      return ''
    }
  },
  watch: {
    visible: {
      immediate: true, // 解决watch props的问题
      handler (val) {
        this.innerVisible = val;
      }
    },
  },
  methods: {
    onClose () {
      this.$emit('update:visible', false);
    }
  }
};
</script>
