<template>
  <div class="weather">
    <div>{{ city }}</div>
    <span :style="style"></span>
    <div>{{ tem }} ℃</div>
  </div>
</template>

<script>
export default {
  data () {
    return {
    }
  },
  props: {
    city: {
      type: String,
      default () {
        return '郑州'
      }
    },
    tem: {
      type: String,
      default () {
        return '20'
      }
    },
    name: {
      type: String,
      required: true
    },
    size: {
      type: Number,
      default () {
        return 16
      }
    },
    margin: {
      type: String,
      default () {
        return '0 10px 0 10px'
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
      return require(`./images/${this.name}.svg`)
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

<style lang="scss" scoped>
.weather {
  display: inline-flex;
  color: #2FC0F3;
  font-size: 16px;
}
</style>
