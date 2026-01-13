<template>
  <div class="dash-card-wrapper" v-loading="loading" element-loading-text="数据量较大,正在加载..." element-loading-background="rgba(0, 0, 0, 0.8)" >
    <dashCard :title="title">
      <div class="content">
       <sn-histogram v-if="!loading" :chartData="dataList" width="100%"></sn-histogram>
      </div>
    </dashCard>
  </div>
</template>

<script>
import dashCard from '../card/index'
import snHistogram from './components/sn-histogram'
// import waveData from "@/request/modules/waveData";

export default {
  name: 'alarm-statistics',
  components: {
    dashCard,
    snHistogram
  },
  props: ['dataList','loading','title']
}
</script>

<style scoped lang="scss">
.dash-card-wrapper{
  height: 320px;
}
</style>
