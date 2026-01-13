<template>
  <d2-container class="page" id="page-device">
<!--    <template slot="header">-->
<!--      <div class="tab">-->
<!--        <div class="tabs">-->
<!--          <el-tabs v-model="activePanel" @tab-click="handleClick" :stretch="true">-->
<!--            <el-tab-pane :label="tab.label" :name="tab.name" :key="tab.name" v-for="tab in tabList">-->
<!--            </el-tab-pane>-->
<!--          </el-tabs>-->
<!--        </div>-->
<!--      </div>-->
<!--    </template>-->
    <router-view></router-view>
  </d2-container>
</template>

<script>

export default {
  components: {
  },
  computed: {

  },
  data () {
    return {
      activePanel: 'handling',
      tabList: [
        {
          label: '全部',
          name: 'all'
        }],
      filename: __filename
    }
  },
  mounted () {
    this.checkActivePanel()
  },
  beforeDestroy () {

  },
  methods: {
    checkActivePanel () {
      const realActivePanel = this.$route.path.replace('/alarm/', '')
      this.activePanel = realActivePanel
    },
    handleClick (tab, event) {
      this.$router.push({
        path: `/alarm/${tab.name}`
      })
    }
  },
}
</script>

<style lang="scss" scoped>
  @import '~@/assets/style/theme/register.scss';
  .page.container-component {
      color: #fff;
  }
  #page-device::v-deep {
    @include el-tabs-reset();
  }
</style>
