<template>
  <div id="app">
      <router-view/>
<!--      <realTimeAlarm></realTimeAlarm>-->
  </div>
</template>

<script>
import realTimeAlarm from '@/components/realtime-alarm'
import BootstrapMixin from './mixins/BootstrapMixin'

export default {
  name: 'app',
  components: {
    // realTimeAlarm
  },
  mixins: [
    BootstrapMixin
  ]
}
</script>

<style lang="scss">
@import '~@/assets/style/public-class.scss';

</style>
