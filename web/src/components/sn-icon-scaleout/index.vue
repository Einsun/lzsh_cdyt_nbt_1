<!-- 暂时使用background来代替，暂时没时间细究svg的loader，已尝试object与svg+use未能正常使用 -->
<template>
  <div class="outDiv">
    <span :style="style" class="sn-icon"></span>
    <div class="blingbling" :style="scaleout(name)"></div>
  </div>
</template>

<script>
export default {
  name: 'sn-icon',
  props: {
    name: {
      type: String,
      required: true
    },
    size: {
      type: Number,
      default () {
        return 14
      }
    },
    margin: {
      type: String,
      default () {
        return '0 10px 0 0'
      }
    },
    path: {
      type: String,
      default () {
        return ''
      }
    }
  },
  computed: {
    icon () {
      return require(`./icons/${this.path ? this.path + '/' : ''}${this.name}.svg`)
    },
    style () {
      const icon = this.icon
      return {
        'background-image': 'url(' + icon + ')',
        'background-repeat': 'no-repeat',
        'background-size': 'contain',
        'background-position': 'center',
        width: this.size + 'px',
        height: this.size + 'px',
        display: 'inline-flex',
        margin: this.margin
      }
    },
    scaleout () {
      return function (val) {
        if (val === 'fire-alarm') {
          return {
            'border': '10px solid rgba(233, 82, 70, 0.5)'
          }
        } else if (val === 'abnormal') {
          return {
            'border': '10px solid rgba(224, 91, 68, 0.5)'
          }
        } else if (val === 'normal') {
          return {
            'border': '10px solid rgba(110, 212, 217, 0.5)'
          }
        } else if (val === 'fault') {
          return {
            'border': '10px solid rgba(231, 149, 72, 0.5)'
          }
        } else {
          return {
            'border': '10px solid rgba(234, 147, 69, 0.5)'
          }
        }
      }
    }
  }
}
</script>

<style scoped lang="scss">
.outDiv {
  width:30px;
  height:30px;
  display: flex;
  justify-content: center;
  align-items: center;
  .blingbling{
    width: 20px;
    height: 20px;
    border-radius: 50%;
    position: absolute;
    -webkit-animation: scaleout 2.5s infinite ease-in-out;
    animation: scaleout 2.5s infinite ease-in-out;
  }
  @-webkit-keyframes scaleout {
    0% { -webkit-transform: scale(1.0) }
    100% {
      -webkit-transform: scale(1.1);
      opacity: 0;
    }
  }
  @keyframes scaleout {
    0% {
      transform: scale(1.0);
      -webkit-transform: scale(1.0);
    } 100% {
        transform: scale(1.1);
        -webkit-transform: scale(1.1);
        opacity: 0;
      }
  }
}
</style>
