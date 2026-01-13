<!-- 暂时使用background来代替，暂时没时间细究svg的loader，已尝试object与svg+use未能正常使用 -->
<template>
  <span :style="style" class="sn-icon" :class="rotate ? 'rotate-animate' : ''"></span>
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
    },
    rotate: {
      type: Boolean,
      default () {
        return false
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
    }
  }
}
</script>

<style scoped lang="scss">
</style>
