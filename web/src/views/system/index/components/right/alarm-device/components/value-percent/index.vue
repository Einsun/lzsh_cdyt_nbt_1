<template>
  <div class="percent-item flex-center-cross flex-justify-main">
    <span class="label">{{label}}</span>
    <span class="percent">
      <span class="value-bar" :style="barStyle"></span>
    </span>
    <span class="value">{{value}}</span>
  </div>
</template>

<script>
export default {
  name: 'value-percent',
  props: {
    standard: {
      type: Number,
      default () {
        return 0
      }
    },
    value: {
      type: Number,
      default () {
        return 0
      }
    },
    label: {
      type: String,
      default () {
        return ''
      }
    }
  },
  computed: {
    percent () {
      const { value, standard } = this
      return value / standard * 100
    },
    barStyle () {
      return { width: `${this.percent}%` }
    }
  }
}
</script>

<style scoped lang="scss">
.percent-item{
  width: 49%;
  margin-bottom: 12px;
  .label{
    color: rgba(255,255,255,.8);
    font-weight:500;
    font-size: 10px;
  }
  .percent{
    height: 14px;
    border:1px solid rgba(46,192,243,.4);
    border-radius:7px;
    flex: 1;
    margin: 0 5px;
    display: inline-flex;
    padding: 0 2px;
    .value-bar{
      display: inline-flex;
      align-self: center;
      justify-self: center;
      height: 8px;
      border-radius: 4px;
      background: #389CC7;
    }
  }
  .value{
    min-width: 2em;
    color: #2EC0F3;
    font-weight:400;
    font-size: 10px;
  }
}
</style>
